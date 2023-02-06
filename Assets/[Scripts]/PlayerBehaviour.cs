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

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
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

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.CompareTag("Pickup")){
      other.gameObject.SetActive(false);
    }
  }
}
