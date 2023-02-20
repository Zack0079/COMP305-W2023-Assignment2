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
  public Transform GroundPoint;
  public float groundRadius;
  public LayerMask groundLayerMask;
  public bool isObstracleInFront;
  public bool isGrounded;
  public bool isGroundAhead;

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(GroundPoint.position, groundRadius);
    Gizmos.DrawLine(GroundPoint.position, inFrontCheck.position);
    Gizmos.DrawLine(GroundPoint.position, groundAheadCheck.position);
  }
}
