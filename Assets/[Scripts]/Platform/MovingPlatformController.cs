using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
  [Header("Movement Properties")]
  [Range(0.01f, 0.2f)]
  public float speedValue = 0.02f;
  public bool timeIsActive = true;
  public bool isLooping;
  public bool isReverse;

  [Header("Platform Path Points")]
  public List<PathNode> pathNodes;

  private Vector2 startPoint;
  private Vector2 endPoint;
  private PathNode currentNode;
  private float timer;


  // Start is called before the first frame update
  void Start()
  {
    timer = 0.0f;
    startPoint = transform.position;
    BuildPathNodes();
  }

  // Update is called once per frame
  private void BuildPathNodes()
  {
    foreach (Transform child in transform)
    {
      var pathNode = new PathNode(child.position, null, null);
      pathNodes.Add(pathNode);
    }

    for (var i = 0; i < pathNodes.Count; i++)
    {
      pathNodes[i].next = (i == pathNodes.Count - 1) ? pathNodes[0] : pathNodes[i + 1];
      pathNodes[i].prev = (i == 0) ? pathNodes[^1] : pathNodes[i - 1];
    }

    currentNode = pathNodes[0];
    startPoint = currentNode.position;
    endPoint = currentNode.next.position;
  }


  void Update()
  {
    Move();
  }

  void FixedUpdate()
  {
    if (timeIsActive)
    {
      if (timer <= 1.0f)
      {
        timer += speedValue;
      }

      if (timer > 1.0f)
      {
        timer = 0.0f;
        Traverse((isReverse) ? 0 : ^1, (isReverse) ? currentNode.prev : currentNode.next);
      }
    }
  }

  private void Move()
  {
    transform.position = Vector2.Lerp(startPoint, endPoint, timer);
  }

  private void Traverse(System.Index boundaryIndex, PathNode nextNode)
  {
    startPoint = currentNode.position;
    endPoint = nextNode.position;

    if (currentNode != pathNodes[boundaryIndex])
    {
      currentNode = nextNode;
    }
    else if (currentNode == pathNodes[boundaryIndex] && (isLooping))
    {
      currentNode = nextNode;
    }
    else
    {
      timeIsActive = false;
    }
  }
}
