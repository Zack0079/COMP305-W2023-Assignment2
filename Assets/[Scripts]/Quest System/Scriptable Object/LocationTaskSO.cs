using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationTask", menuName = "Tasks/LocationTask")]
public class LocationTaskSO : TaskSO
{
  public Vector2 taskPosition = Vector2.zero;

  public override bool Condition()
  {
    var distance = Vector2.Distance(targetPosition, taskPosition);
    var result =  distance < 1.0f;

    if(result){
      state = ProgressState.COMPLETED;
      Debug.Log("Target made it to: "+name);
    }
    
    return result;
  }
}
