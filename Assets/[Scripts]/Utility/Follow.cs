using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
  public Transform target;
  public Vector3 offset = new Vector3(0.0f, 0.2f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        // offset = new Vector3(0.0f, 0.2f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = new Vector3(target.position.x, target.position.y, 0.0f)+offset;
        var target_position = target.position + offset;
        transform.position = target_position;
    }
    
}
