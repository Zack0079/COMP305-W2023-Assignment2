using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
  [Header("Quest Manager Properties")]
  public GameObject player;
  // public List<QuestSO> quests;
  public QuestSO quest;

  // Update is called once per frame
  void Update()
  {
    quest.targetPosition = player.transform.position;
    quest.Condition();
  }
}
