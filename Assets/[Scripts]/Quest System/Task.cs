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
  public TaskState status;
  public TaskStateText text;

  public Task(string name, Task prevTask = null, Task nextTask = null, TaskState status = TaskState.NOT_STARTED)
  {
    this.id = DateTime.Now.Millisecond.ToString();
    this.name = name;
    this.prevTask = prevTask;
    this.nextTask = nextTask;
    this.status = status;
  }
}
