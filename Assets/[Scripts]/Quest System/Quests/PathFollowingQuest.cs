using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingQuest : Quest
{
  [Header("Path Following Quest Properties")]
  public GameObject target;
  public Transform startLocation;
  public List<Transform> wayPoints;


  public PathFollowingQuest(string name, LocationTask rootTask, List<Transform> wayPoints, ProgressState state = ProgressState.NOT_STARTED)
  : base(name, rootTask, state)
  {
    target = rootTask.target;
    startLocation = rootTask.location;
    this.wayPoints = wayPoints;
    BuildQuest();
  }


  public override void BuildQuest()
  {
    //add as many location tasks as there are wayPoints
    for (int i = 0; i < wayPoints.Count; i++)
    {
      tasks.Add(new LocationTask("Location " + i, target, wayPoints[i], (i > 0) ? tasks[i - 1] : null, null));
    }

    // create links (relationships) between tasks
    for (int i = 0; i < wayPoints.Count; i++)
    {
      tasks[i].nextTask = (i < tasks.Count - 1) ? tasks[i + 1] : null;
    }
  }
}

