using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathNode
{
  public Vector2 position;
  public PathNode next;
  public PathNode prev;

  //constructer
  public PathNode(Vector2 position, PathNode nextNode, PathNode prevNode)
  {
    this.position = position;
    this.next = nextNode;
    this.prev = prevNode;
  }
}
