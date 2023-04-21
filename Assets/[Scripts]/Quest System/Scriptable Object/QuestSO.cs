using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericQuest", menuName = "Quests/GenericQuest")]
public class QuestSO : ScriptableObject
{
  [Header("Quest Properties")]
  public string name = null;
  public ProgressState state = ProgressState.NOT_STARTED;
  public ProgressStateText text;
  public List<TaskSO> tasks;
  public TaskSO currentTask = null;
  public Vector2 targetPosition;

  void OnValidate()
  {
    currentTask = tasks[0];
  }


  public bool Condition()
  {
    bool completed = false;
    foreach (var task in tasks)
    {
      task.targetPosition = targetPosition;
      task.Condition();
      if (task.state == ProgressState.COMPLETED)
      {
        completed = true;
        currentTask = task;
      }
      else
      {
        completed = false;
        break;
      }
    }
    if (completed)
    {
      state = ProgressState.COMPLETED;
      // Debug.Log("The " + name + " quest has been completed");
    }
    return completed;
  }
}
