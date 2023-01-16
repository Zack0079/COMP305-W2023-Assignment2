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
  public LayerMask groundLayerMask;
  public bool isGround;

  private Rigidbody2D rigidbody2D;
  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    float x = Input.GetAxis("Horizontal") * Time.deltaTime;
    // float y = Input.GetAxis("Vertical");
    Move(x);
    Flip(x);
  }

  void FixedUpdate() {
    isGround = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);  
  }

  private void Move(float x){
    rigidbody2D.AddForce(Vector2.right * x * horitzontalForce);
    rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxSpeed, maxSpeed), rigidbody2D.velocity.y);
  }

  private void Flip(float x){
    if(x!=0){
      transform.localScale = new Vector3((x > 0) ? 1: -1,1,1);

    }

  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(groundPoint.position, groundRadius); 
  }
}
