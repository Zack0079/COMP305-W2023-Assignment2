using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
  public GameObject miniMap;
  public TMP_Text buttonLabel;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (miniMap != null)
    {
      if (Input.GetKeyDown(KeyCode.M))
      {
        miniMap.SetActive(!miniMap.activeInHierarchy);
      }
    }

  }

  public void OnStartButton_Press()
  {
    SceneManager.LoadScene("Main");
  }

    public void OnRestartButton_Press()
  {
    SceneManager.LoadScene("Main");
  }

  public void OnButton_Down()
  {
    buttonLabel.rectTransform.localPosition = new Vector3(0.0f, -6.0f, 0.0f);
  }

  public void OnButton_Up()
  {
    buttonLabel.rectTransform.localPosition = new Vector3(0.0f, 6.0f, 0.0f);
  }

}
