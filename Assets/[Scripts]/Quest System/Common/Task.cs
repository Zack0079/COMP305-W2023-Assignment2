using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Task
{
  [Header("Task Properties")]
  public string id;
  public string name;
  public Task prevTask;
  public Task nextTask;
  public ProgressState state;
  public ProgressStateText text;

  public Task(string name, Task prevTask = null, Task nextTask = null, ProgressState state = ProgressState.NOT_STARTED)
  {
    this.id = DateTime.Now.Millisecond.ToString();
    this.name = name;
    this.prevTask = prevTask;
    this.nextTask = nextTask;
    this.state = state;

    if(this.prevTask != null && this.prevTask.state != ProgressState.COMPLETED){
      this.state = ProgressState.INVALID;
    }
  }

  public virtual bool Condition(){
    return false;
  }
}
