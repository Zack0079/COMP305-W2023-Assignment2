using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
  
  private void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject.CompareTag("Player")){
      other.transform.SetParent(transform);
    }
  }

  private void OnCollisionExit2D(Collision2D other) {
    if(other.gameObject.CompareTag("Player")){
      other.transform.SetParent(null);
    }
  }
}
