using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
  public Transform player;

  public Vector3 offset;
  public Vector2 direction;
  public Rigidbody2D rigidbody2D;
  [Range(1.0f, 100.0f)]
  public float force;

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    player = FindObjectOfType<PlayerBehaviour>().transform;
    direction = Vector3.Normalize((player.position + ((offset.y <= player.position.y) ? offset : new Vector3(0.0f, 1.0f, 0.0f))) - transform.position);
    Activate();
  }
  // Update is called once per frame
  void Update()
  {
    Rotate();
  }

  // Update is called once per frame
  void Activate()
  {
    Move();
    Invoke("DestroyBullet", 2.0f);
  }

  public void Move()
  {
    rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
  }

  // Update is called once per frame
  void Rotate()
  {
    transform.RotateAround(transform.position, Vector3.forward, 5.0f);
  }

  public void DestroyBullet()
  {
    if (gameObject.activeInHierarchy)
    {
      Destroy(gameObject);
    }
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      DestroyBullet();
    }
  }
}
