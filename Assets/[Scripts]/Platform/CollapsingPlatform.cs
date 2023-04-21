using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
  [SerializeField] private float fallDelay = 1.0f;
  [SerializeField] private float DestroyDelay = 3.0f;
  private Rigidbody2D rb;
  private GameObject player;
  // Start is called before the first frame update
  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      other.transform.SetParent(transform);
      player = other.gameObject;
      StartCoroutine(Fall());
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      other.transform.SetParent(null);
      player = null;
    }
  }

  private IEnumerator Fall()
  {
    yield return new WaitForSeconds(fallDelay);
    rb.bodyType = RigidbodyType2D.Dynamic;
    yield return new WaitForSeconds(DestroyDelay);
    if (player)
    {
      player.transform.SetParent(null);
    }
    Destroy(gameObject);

  }
}
