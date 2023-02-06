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

  private Rigidbody2D rigidbody2D;
  private Animator animator;

  [Header("Screen Shake Properties")]
  public CinemachineVirtualCamera virtualCamera;
  public CinemachineBasicMultiChannelPerlin perlin;
  public float shakeIntensity;
  public float shakeDuration;
  public float shakeTimer;
  public bool isCameraShaking;

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();

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
    rigidbody2D.AddForce(Vector2.right * x * horitzontalForce * (isGrounded ? 1 : airFactor));
    rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxSpeed, maxSpeed), rigidbody2D.velocity.y);

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
      rigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
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
      transform.localScale = new Vector3((x > 0) ? 1 : -1, 1, 1);

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

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Pickup"))
    {
      other.gameObject.SetActive(false);
      // make interesting sound;
      // get game points;
    }

    if (other.gameObject.CompareTag("Hazard"))
    {
      ShakeCamera();
      // make an interesting sound (hurt sound)
      // lose lealth;
    }

  }
}
