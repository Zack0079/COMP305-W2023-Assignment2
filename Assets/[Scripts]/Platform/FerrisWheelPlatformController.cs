using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelPlatformController : PlatformController
{
  [SerializeField] private float stopTime = 3.0f;
  [SerializeField] private int stopFrequencyInCircle = 4;
  [SerializeField] private float rotationSpeed = 1.0f;
  [SerializeField] private float rotationRadius = 5.0f;
  [SerializeField] private Transform rotationCenter;

  public float posX, posY, angle = 0f;
  private float pi = Mathf.PI;
  public bool stop = false;

  private void Start()
  {
    StartCoroutine(rotationPlatform());
  }

  private IEnumerator rotationPlatform()
  {
    while (true)
    {
      rotating();
      yield return new WaitForEndOfFrame();
      float stopAngle = 2 * pi / stopFrequencyInCircle;

      if ((angle % stopAngle > 0.01 && angle % stopAngle < stopAngle - 0.01) && stop)
      {
        stop = false;
      }
      else if ((angle % stopAngle < 0.01 || angle % stopAngle > stopAngle - 0.01) && !stop)
      {
        stop = true;
        yield return new WaitForSeconds(stopTime);
      }
    }
  }

  private void rotating()
  {
    posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
    posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
    transform.position = new Vector3(posX, posY, 0f);
    angle += Time.deltaTime * rotationSpeed;
    if (angle >= pi * 2 * 50)
    {
      angle = 0;
    }
  }


  private void OnDrawGizmos()
  {
    Gizmos.color = Color.magenta;
    // Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    Gizmos.DrawLine(transform.position, rotationCenter.position);
  }
}
