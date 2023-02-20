using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  private Rigidbody2D rigidbody2D;
  [Header("Enemy Movement Properties")]
  public float horizontalSpeed = 1.0f;
  public Transform inFrontCheck;
  public Transform groundAheadCheck;
  public Transform groundPoint;
  public float groundRadius;
  public LayerMask groundLayerMask;
  public bool isObstracleInFront;
  public bool isGrounded;
  public bool isGroundAhead;
  public Vector2 direction;

  // Start is called before the first frame update
  void Start()
  {
    direction = Vector2.left;

    rigidbody2D = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    isObstracleInFront = Physics2D.Linecast(groundPoint.position, inFrontCheck.position, groundLayerMask);
    isGroundAhead = Physics2D.Linecast(groundPoint.position, groundAheadCheck.position, groundLayerMask);
    isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);

    if (isGroundAhead)
    {
      Move();
    }
    if (!isGroundAhead || isObstracleInFront)
    {
      Flip();
    }
  }

  private void Move()
  {
    transform.position += new Vector3(direction.x * horizontalSpeed * Time.deltaTime, 0.0f);
  }



  private void Flip()
  {
    var x = transform.localScale.x * -1.0f;
    direction *= -1.0f;
    transform.localScale = new Vector3(x, 1, 1);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    Gizmos.DrawLine(groundPoint.position, inFrontCheck.position);
    Gizmos.DrawLine(groundPoint.position, groundAheadCheck.position);
  }
}
