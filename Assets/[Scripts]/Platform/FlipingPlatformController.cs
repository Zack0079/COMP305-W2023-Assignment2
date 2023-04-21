using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipingPlatformController : MonoBehaviour
{
  [SerializeField] private float flipingTime = 5.0f;
  private float direction = 1;
  private void Start()
  {
    StartCoroutine(Fliping());
  }

  private IEnumerator Fliping()
  {
    while (true)
    {
      yield return new WaitForSeconds(flipingTime);
      gameObject.transform.rotation = Quaternion.Euler(direction > 0 ? Vector3.forward  * 180 : Vector3.zero );
      direction = direction * -1;
    }

  }
}
