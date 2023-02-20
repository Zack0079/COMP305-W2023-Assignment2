using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
  [Header("Health Properties")]
  public int value = 100;

  [Header("Display Properties")]
  public Slider healthBar;

  // Start is called before the first frame update
  void Start()
  {
    healthBar = GetComponentInChildren<Slider>();
    RestHealth();
  }

  public void TakeDamge(int damage)
  {
    healthBar.value -= damage;
    if (healthBar.value < 0)
    {
      healthBar.value = 0;
    }
  }


  public void HealDamge(int heal)
  {
    healthBar.value += heal;
    if (healthBar.value > 100)
    {
      healthBar.value = 100;
    }
  }

  public void RestHealth()
  {
    healthBar.value = 100;
  }

  public void OnHealthValue_Change()
  {
    value = (int)healthBar.value;
  }
}
