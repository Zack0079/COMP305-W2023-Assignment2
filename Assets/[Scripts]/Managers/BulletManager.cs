using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletManager : MonoBehaviour
{
  [Header("Bullet Pool Properties")]
  public int poolSize;
  public GameObject bulletPrefab;
  public Transform bulletParent;

  private Queue<GameObject> bulletPool;


  void Awake()
  {
    bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    bulletParent = GameObject.Find("[BULLETS]").transform;
    bulletPool = new Queue<GameObject>();
  }


  void Start()
  {
    buildBulletPool();
  }

  private void buildBulletPool()
  {
    for (int i = 0; i < poolSize; i++)
    {
      bulletPool.Enqueue(CreateBullet());
    }
  }


  public GameObject CreateBullet()
  {
    var bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, bulletParent);
    bullet.SetActive(false);
    return bullet;
  }

  public GameObject GetBullet(Vector2 position){
    if(bulletPool.Count<1){
      bulletPool.Enqueue(CreateBullet());
    }

    var bullet = bulletPool.Dequeue();
    bullet.transform.position = position;
    bullet.SetActive(true);
    bullet.GetComponent<BulletController>().Activate();
    return bullet;
  }

  public void ReturnBullet(GameObject bullet){
    bullet.SetActive(false);
    bullet.transform.position = Vector3.zero;
    bullet.transform.rotation = Quaternion.identity;
    bullet.GetComponent<BulletController>().direction = Vector3.zero;
    bulletPool.Enqueue(bullet);
  }

}
