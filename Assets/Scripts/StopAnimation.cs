using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopAnimation : MonoBehaviour
{
    public GameObject loadingScreen;
    public Button CloseButton;

    private void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(QuitApp);
    }

    void CloseScreen()
    {
        CloseButton.gameObject.SetActive(true);
        loadingScreen.SetActive(false);
    }

    void QuitApp()
    {
        Application.Quit();
    }


}
