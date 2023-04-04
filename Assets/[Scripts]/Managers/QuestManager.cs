using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestManager : MonoBehaviour
{

  [Header("Quest Manager Properties")]
  public GameObject player;
  public Transform startLocation;
  public List<Quest> quests;
  public PathFollowingQuest currentQuest;
  public List<Transform> path;


  // Start is called before the first frame update
  void Start()
  {
    currentQuest = new PathFollowingQuest(
      "Follow the Path",
      new LocationTask("Path Start Location", player, startLocation),
      path
    );

    quests.Add(currentQuest);
    currentQuest.state = ProgressState.IN_PROGRESS;
    currentQuest.BuildQuest();
  }

  // Update is called once per frame
  void Update()
  {

    if (currentQuest.state == ProgressState.IN_PROGRESS)
    {
      if (currentQuest.currentTask.Condition())
      {
        currentQuest.currentTask.state = ProgressState.COMPLETED;
        print(currentQuest.currentTask.name + " is complete!");
        if (currentQuest.currentTask.nextTask != null)
        {
          currentQuest.currentTask = currentQuest.currentTask.nextTask;
          currentQuest.currentTask.state = ProgressState.IN_PROGRESS;
        }
        else
        {
          print(currentQuest.name + " is complete!");
          currentQuest.state = ProgressState.COMPLETED;
        }
      }
    }
  }
}
