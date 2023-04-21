using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPlatformController : MonoBehaviour
{

  public Rigidbody2D rb;
  public float moveSpeed = 30.0f;
  public float rightAngle = 0.3f;
  public float leftAngle = -0.3f;
  public float vel;
  public float rot;

  public bool moveClockwise;

  void Update()
  {
    Push();
  }

  void Push()
  {

    vel = rb.angularVelocity;
    rot = rb.transform.rotation.z;


    if (rb.transform.rotation.z >= rightAngle)
    {
      moveClockwise = true;

    }
    else if (rb.transform.rotation.z <= leftAngle)
    {
      moveClockwise = false;
    }

    if ((!moveClockwise && rb.angularVelocity < moveSpeed) || (moveClockwise && rb.angularVelocity > -moveSpeed))
    {
      // Debug.Log("add");
      rb.AddTorque(moveClockwise ? -moveSpeed : moveSpeed);
    }
  }
  private void OnCollisionEnter2D(Collision2D other)
  {
    // Debug.Log("a");
    if (other.gameObject.CompareTag("Player"))
    {
      other.transform.SetParent(transform);
      moveSpeed += 10f;
    }
  }

  private void OnCollisionStay2D(Collision2D other)
  {
    // Debug.Log("c");
    if (other.gameObject.CompareTag("Player"))
    {

      if (rb.transform.rotation.z >= rightAngle)
      {
        other.rigidbody.AddForce(new Vector2(-1f, -1f) * moveSpeed);

      }
      else if (rb.transform.rotation.z <= leftAngle)
      {
        other.rigidbody.AddForce(new Vector2(1f, -1f) * moveSpeed);
      }
    }
  }
  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {

      other.transform.SetParent(null);
      moveSpeed -= 10f;
    }
  }

  // private void OnCollisionExit2D(Collision2D other)
  // {
  //   // Debug.Log("b");

  //   if (other.gameObject.CompareTag("Player"))
  //   {

  //     other.transform.SetParent(null);
  //     moveSpeed -= 10f;
  //   }
  // }


}
