using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
  [Header("Player Movement Properties")]
  public float horitzontalForce;
  public float maxSpeed;
  public float verticalForce;
  public float airFactor;
  public Transform groundPoint;
  public float groundRadius;
  public bool isGrounded;
  public LayerMask groundLayerMask;
  public PlayerAnimationState animationState;

  [Header("Screen Shake Properties")]
  public CinemachineVirtualCamera virtualCamera;
  public CinemachineBasicMultiChannelPerlin perlin;
  public float shakeIntensity;
  public float shakeDuration;
  public float shakeTimer;
  public bool isCameraShaking;


  [Header("Player Body Properties")]
  public Transform playerBody;
  public Rigidbody2D playerRigidbody2D;
  public Animator animator;

  private SoundManager soundManager;

  // Start is called before the first frame update
  void Start()
  {
    // playerRigidbody2D = GetComponentInChildren<Rigidbody2D>();
    // animator = GetComponentInChildren<Animator>();
    // playerBody = GetComponentInChildren<Transform>();
    soundManager = FindObjectOfType<SoundManager>();

    // camera
    isCameraShaking = false;
    shakeTimer = shakeDuration;
    virtualCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
  }

  // Update is called once per frame
  void Update()
  {
    var y = Convert.ToInt32(Input.GetKeyDown(KeyCode.Space));
    Jump(y);
  }

  void FixedUpdate()
  {
    isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
    var x = Input.GetAxis("Horizontal");

    Move(x);
    Flip(x);
    AirCheck();

    if (isCameraShaking)
    {
      shakeTimer -= Time.deltaTime;
      if (shakeTimer <= 0.0f)
      {
        perlin.m_AmplitudeGain = 0.0f;
        shakeTimer = shakeDuration;
        isCameraShaking = false;
      }
    }
  }

  private void Move(float x)
  {
    playerRigidbody2D.AddForce(Vector2.right * x * horitzontalForce * (isGrounded ? 1 : airFactor));
    playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -maxSpeed, maxSpeed), playerRigidbody2D.velocity.y);

    if (isGrounded)
    {
      animationState = x != 0.0f ? PlayerAnimationState.RUN : PlayerAnimationState.IDLE;
      animator.SetInteger("AnimationState", (int)animationState);
    }
  }

  private void Jump(int y)
  {
    if (isGrounded && y > 0.0f)
    {
      //ForceMode2D.Impulse -> one time force
      playerRigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
      soundManager.PlaySoundFX(Channel.PLAYER_SOUND_FX, SoundFX.JUMP);
    }
  }

  private void AirCheck()
  {
    if (!isGrounded)
    {
      // play jump Animation
      animationState = PlayerAnimationState.JUMP;
      animator.SetInteger("AnimationState", (int)animationState);

    }
  }

  private void Flip(float x)
  {
    if (x != 0)
    {
      playerBody.localScale = new Vector3((x > 0) ? 1 : -1, 1, 1);

    }
  }

  private void ShakeCamera()
  {
    perlin.m_AmplitudeGain = shakeIntensity;
    isCameraShaking = true;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
  }

//TODO: Need to move to player body
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Pickup"))
    {
      other.gameObject.SetActive(false);
      soundManager.PlaySoundFX(Channel.PICKUP, SoundFX.GEM);
      // get game points;
    }

    if (other.gameObject.CompareTag("Hazard"))
    {
      ShakeCamera();
      soundManager.PlaySoundFX(Channel.PLAYER_HURT_FX, SoundFX.HURT);
      // lose lealth;
    }

  }
}
