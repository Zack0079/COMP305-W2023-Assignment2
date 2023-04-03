using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTask : Task
{
  [Header("Location Properties")]
  public GameObject target;
  public Transform location;
  public float distance;

  public LocationTask(string name, GameObject taskTarget, Transform taskLocation, Task prevTask = null, Task nextTask = null, ProgressState state = ProgressState.NOT_STARTED)
   : base(name, prevTask, nextTask, state)
  {
    this.target = taskTarget;
    this.location = taskLocation;
  }

  public override bool Condition()
  {
    return Vector2.Distance(target.transform.position, location.position) < distance;
  }
}
