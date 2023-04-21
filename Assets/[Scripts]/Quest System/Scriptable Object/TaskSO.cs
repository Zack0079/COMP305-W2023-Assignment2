using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericTask", menuName = "Tasks/GenericTask")]
public class TaskSO : ScriptableObject
{
  [Header("Task Properties")]
  public string name = "";
  public TaskSO prevTask = null;
  public TaskSO nextTask = null;
  public ProgressState state = ProgressState.NOT_STARTED;
  public ProgressStateText text;
  public Vector2 targetPosition;

  void OnValidate()
  {
    // id = this.GetHashCode().ToString();
    //Debug.Log("Task created");

    if ((this.prevTask != null) && (this.prevTask.state != ProgressState.COMPLETED))
    {
      this.state = ProgressState.INVALID;
    }
  }


  public virtual bool Condition()
  {
    return false;
  }
}
