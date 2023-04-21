using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingHazardPlatformController : PlatformController
{
  [Header("Movement Properties")]
  [Range(0.001f, 0.2f)]
  public float speedValue = 0.02f;
  public bool timeIsActive = true;
  public GameObject scrollingHazard;


  [Header("Hazard Path Points")]
  public List<PathNode> pathNodes;
  private Vector2 startPoint;
  private Vector2 endPoint;
  private PathNode currentNode;
  private bool isScrollBack = false;
  private float timer;
  private int size = 0;
  private int currentIndex = 0;

  // Start is called before the first frame update
  void Start()
  {
    timer = 0.0f;
    startPoint = scrollingHazard.transform.position;
    BuildPathNodes();
  }

  // Update is called once per frame
  private void BuildPathNodes()
  {
    foreach (Transform child in this.gameObject.transform.GetChild(0))
    {
      var pathNode = new PathNode(child.position, null, null);
      pathNodes.Add(pathNode);
    }
    size = pathNodes.Count;
    for (var i = 0; i < size; i++)
    {
      pathNodes[i].next = (i == pathNodes.Count - 1) ? pathNodes[0] : pathNodes[i + 1];
      pathNodes[i].prev = (i == 0) ? pathNodes[^1] : pathNodes[i - 1];
    }
    currentNode = pathNodes[0];
    startPoint = currentNode.position;
    endPoint = currentNode.next.position;
    Traverse(^1, currentNode.next);
  }

  void FixedUpdate()
  {
    Move();

    if (timeIsActive)
    {
      if (timer <= 1.0f)
      {
        timer += speedValue;
      }

      if (timer > 1.0f)
      {

        timer = 0.0f;
        Traverse((isScrollBack) ? 0 : ^1, (isScrollBack) ? currentNode.prev : currentNode.next);
      }
    }
  }

  private void Move()
  {
    scrollingHazard.transform.position = Vector2.Lerp(startPoint, endPoint, timer);
  }

  private void Traverse(System.Index boundaryIndex, PathNode nextNode)
  {

    if (currentNode != pathNodes[boundaryIndex])
    {
      startPoint = currentNode.position;
      endPoint = nextNode.position;
      currentNode = nextNode;
    }
    else if (currentNode == pathNodes[boundaryIndex])
    {
      startPoint = currentNode.position;
      currentNode = boundaryIndex.Equals(0) ? currentNode.next : currentNode.prev;

      endPoint = currentNode.position;
      isScrollBack = !isScrollBack;
    }
  }



}
