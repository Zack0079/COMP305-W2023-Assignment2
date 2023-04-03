using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
  [Header("Player Movement Properties")]
  public float horizontalForce;
  public float maxSpeed;
  public float verticalForce;
  public float airFactor;
  public Transform groundPoint;
  public float groundRadius;
  public bool isGrounded;
  public LayerMask groundLayerMask;

  [Header("Ramp Detection")]
  public Transform inFrontCheck;
  public LayerMask rampLayerMask;
  public RampDirection rampDirection;
  public bool isRampInFront;

  [Header("Screen Shake Properties")]
  public CinemachineVirtualCamera virtualCamera;
  public CinemachineBasicMultiChannelPerlin perlin;
  public float shakeIntensity;
  public float shakeDuration;
  public float shakeTimer;
  public bool isCameraShaking;

  [Header("Animation Properties")]
  public PlayerAnimationState animationState;

  [Header("Health System")]
  public HealthSystem health;
  public LifeCounter life;
  public bool isCollidingWithEnemy;

  [Header("Collision Response")]
  public float bounceFore;


  [Header("Dust Trail Properties")]
  public ParticleSystem dustTrailParticleSystem;
  public Color dustTrailColor;

  private Rigidbody2D playerRigidbody2D;
  private Animator animator;
  private SoundManager soundManager;
  private DeathPlaneController deathPlaneController;

  // Start is called before the first frame update
  void Start()
  {
    rampDirection = RampDirection.NONE;
    playerRigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    soundManager = FindObjectOfType<SoundManager>();
    health = FindObjectOfType<PlayerHealthSystem>().GetComponent<HealthSystem>();
    life = FindObjectOfType<LifeCounter>();
    deathPlaneController = FindObjectOfType<DeathPlaneController>();
    isCollidingWithEnemy = false;

    // camera
    isCameraShaking = false;
    shakeTimer = shakeDuration;
    virtualCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    // dust trail
    dustTrailParticleSystem = GetComponent<ParticleSystem>();

  }

  // Update is called once per frame
  void Update()
  {
    var y = Convert.ToInt32(Input.GetKeyDown(KeyCode.Space));
    Jump(y);

    if (health.value <= 0)
    {
      life.LoseLife();

      if (life.value > 0)
      {
        health.RestHealth();
        deathPlaneController.ReSpawn(this.gameObject);
        soundManager.PlaySoundFX(Channel.PLAYER_DEATH_FX, SoundFX.DEATH);
      }
    }
    if (life.value <= 0)
    {
      SceneManager.LoadScene("End");
    }
  }

  void FixedUpdate()
  {
    isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
    var x = Input.GetAxis("Horizontal");

    isRampInFront = Physics2D.Raycast(
      transform.position,
      (inFrontCheck.position - transform.position).normalized,
      (inFrontCheck.position - transform.position).magnitude,
      rampLayerMask
    );


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
    playerRigidbody2D.AddForce(Vector2.right * x * horizontalForce * (isGrounded ? 1.0f : airFactor));
    playerRigidbody2D.AddForce(Vector2.up * (isRampInFront ? verticalForce : 0.0f));

    playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -maxSpeed, maxSpeed), playerRigidbody2D.velocity.y);

    if (isGrounded)
    {
      if (x != 0.0f)
      {
        CreateDustTrail();
      }
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
      CreateDustTrail();
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

  public void CreateDustTrail()
  {
    dustTrailParticleSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", dustTrailColor);
    dustTrailParticleSystem.Play();
  }

  private void ShakeCamera()
  {
    perlin.m_AmplitudeGain = shakeIntensity;
    isCameraShaking = true;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.magenta;
    Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    Gizmos.DrawLine(transform.position, inFrontCheck.position);
  }

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
      health.TakeDamge(30);
      playerRigidbody2D.AddForce(new Vector2(bounceFore * (playerRigidbody2D.velocity.x > 0.0 ? -1.0f : 1.0f), 0.0f), ForceMode2D.Impulse);
    }

    if (other.gameObject.CompareTag("Bullet"))
    {
      ShakeCamera();
      soundManager.PlaySoundFX(Channel.PLAYER_HURT_FX, SoundFX.HURT);
      health.TakeDamge(10);
      playerRigidbody2D.AddForce(new Vector2(bounceFore * 0.5f * (other.GetComponent<Rigidbody2D>().velocity.x > 0.0 ? 1.0f : -1.0f), 0.0f), ForceMode2D.Impulse);
    }
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Ramp"))
    {
      if (isRampInFront)
      {
        rampDirection = RampDirection.UP;
        transform.eulerAngles = new Vector3(0, 0, 25f * transform.localScale.x);
      }
      else
      {
        rampDirection = RampDirection.DOWN;
        transform.eulerAngles = new Vector3(0, 0, 25f * transform.localScale.x);

      }
    }
  }
  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Ramp"))
    {
      rampDirection = RampDirection.NONE;
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Enemy"))
    {
      ShakeCamera();
      soundManager.PlaySoundFX(Channel.PLAYER_HURT_FX, SoundFX.HURT);
      health.TakeDamge(20);
      playerRigidbody2D.AddForce(new Vector2(bounceFore * (playerRigidbody2D.velocity.x > 0.0 ? -1.0f : 1.0f), 0.0f), ForceMode2D.Impulse);
    }
  }

  private void OnCollisionStay2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Enemy"))
    {
      ShakeCamera();
      if (!isCollidingWithEnemy)
      {
        soundManager.PlaySoundFX(Channel.GROWL, SoundFX.GROWL);
        isCollidingWithEnemy = true;
      }
      health.TakeDamge(1);
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Enemy"))
    {
      soundManager.StopSoundFX(Channel.GROWL, SoundFX.GROWL);
      isCollidingWithEnemy = false;
    }
  }
}
