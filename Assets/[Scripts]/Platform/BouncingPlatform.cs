using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
  [SerializeField] private float bounce = 17f;

  private void OnCollisionEnter2D(Collision2D other)
  {
    BoxCollider2D collider = other.otherCollider as BoxCollider2D;

    if (other.gameObject.CompareTag("Player") && collider != null)
    {
      other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
    }
  }
}
