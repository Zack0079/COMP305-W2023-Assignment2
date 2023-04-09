using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
  [Header("Quest Properties")]
  public string id;
  public string name;
  public ProgressState state;
  public ProgressStateText text;
  public List<Task> tasks;
  public Task currentTask;

  public Quest(string name, Task rootTask, ProgressState state = ProgressState.NOT_STARTED)
  {
    this.id = DateTime.Now.Millisecond.ToString();
    this.name = name;
    this.state = state;
    tasks = new List<Task>();
    this.currentTask = rootTask;
    tasks.Add(rootTask);
  }

  public virtual void BuildQuest()
  {

  }
}