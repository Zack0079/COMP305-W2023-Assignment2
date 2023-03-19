using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackAction : MonoBehaviour, Action
{
  public bool hasLOS;
  [Range(1, 100)]
  public int fireDelay = 20;
  public Transform bulletSpawn;

  public BulletManger bulletManger;

  void Start()
  {
    bulletManger = FindObjectOfType<BulletManger>();

  }

  void Update()
  {
    hasLOS = GetComponent<PlayerDetection>().LOS;
  }

  void FixedUpdate()
  {
    if (hasLOS && Time.frameCount % fireDelay == 0)
    {
      Execute();
    }
  }

  public void Execute()
  {
    bulletManger.GetBullet(bulletSpawn.position);
    // bullet.GetComponent<BulletController>();
    // throw new System.NotImplementedException();
  }
}
