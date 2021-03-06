﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scan_uiHandler : MonoBehaviour
{

    public GameObject ProductScreen, ActivationScreen, ScanScreen, downloadimage, downloadingText, backbtn;
    public Text errortext;
    public Button playAudioBtn, pauseAudioBtn;
    public bool _load_downloded_bundle;

    // Use this for initialization
    void Start()
    {
        backbtn.SetActive(true);
        errortext.text = Application.persistentDataPath.Contains("delta.unity3d").ToString();

        if (!File.Exists(Application.persistentDataPath + "/" + "delta.unity3d"))
        {
            ProductScreen.SetActive(true);
            downloadimage.GetComponent<Image>().fillAmount = 1;
            UI_Manager.changescr = UI_Manager.screenStates.productsList;
            Debug.Log("StartScanFileNotExist");
        }
        else
        {
            ScanScreen.SetActive(true);
            StartCoroutine(BundlerHander.Instance.LoadBundles());
            _load_downloded_bundle = true;
            UI_Manager.changescr = UI_Manager.screenStates.ScanScreen;
            Debug.Log("StartScanFileExist");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Backbtn();
        }
    }



    public void Chapter1()
    {

        if (!File.Exists(Application.persistentDataPath + "/" + "delta.unity3d"))
        {
            Screen.orientation = ScreenOrientation.Portrait;
            ActivationScreen.SetActive(true);
            downloadimage.GetComponent<Image>().fillAmount = 1;
            UI_Manager.changescr = UI_Manager.screenStates.ActivationScreen;
            Debug.Log("ChapterCalled");
        }
        else
        {
            Screen.orientation = ScreenOrientation.Landscape;
            ScanScreen.SetActive(true);
            ProductScreen.SetActive(false);
            StartCoroutine(BundlerHander.Instance.LoadBundles());
            downloadimage.GetComponent<Image>().fillAmount = 0;
            UI_Manager.changescr = UI_Manager.screenStates.ScanScreen;
        }
    }


    public void ClickActivation()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        ActivationScreen.SetActive(false);
        ProductScreen.gameObject.SetActive(true);
        StartCoroutine(BundlerHander.Instance.AssetBundleDownload());

        UI_Manager.changescr = UI_Manager.screenStates.downloading;
    }


    public void Backbtn()
    {

        switch (UI_Manager.changescr)
        {
            case UI_Manager.screenStates.ActivationScreen:

                ActivationScreen.SetActive(false);
                ProductScreen.SetActive(true);
                UI_Manager.changescr = UI_Manager.screenStates.productsList;
                break;

            case UI_Manager.screenStates.downloading:
                //Print to wait
                break;

            case UI_Manager.screenStates.productsList:
                downloadimage.GetComponent<Image>().fillAmount = 0;
                downloadingText.GetComponent<Text>().text = "";
                SceneManager.LoadScene("DeltaAR");
                UI_Manager.screenChange = "FromScan";
                UI_Manager.changescr = UI_Manager.screenStates.menuscreen;
                break;

            case UI_Manager.screenStates.ScanScreen:
                ScanScreen.SetActive(false);
                ProductScreen.SetActive(true);
                UI_Manager.changescr = UI_Manager.screenStates.productsList;
                downloadimage.GetComponent<Image>().fillAmount = 0;
                downloadingText.GetComponent<Text>().text = "";
                break;
        }
    }

    public void UIBackBtn()
    {
        downloadimage.GetComponent<Image>().fillAmount = 0;
        downloadingText.GetComponent<Text>().text = "";
        ScanScreen.SetActive(false);
        ProductScreen.SetActive(true);
        backbtn.SetActive(true);
        UI_Manager.changescr = UI_Manager.screenStates.menuscreen;
    }

    public bool audioplayenable;
    public void AudioPlayPause()
    {
        if (!audioplayenable)
        {
            playAudioBtn.gameObject.SetActive(false);
            pauseAudioBtn.gameObject.SetActive(true);
            // GameObject.Find("ScanController").AddComponent<AudioSource>();
            //GameObject.Find("ScanController").GetComponent<AudioSource>().clip = testClip;
            //GameObject.Find("ScanController").GetComponent<AudioSource>().Play();
            BundlerHander.Instance.ParentRef.GetComponentInChildren<AudioSource>().Pause();
            audioplayenable = true;
        }
        else
        {
            pauseAudioBtn.gameObject.SetActive(false);
            playAudioBtn.gameObject.SetActive(true);
            //GameObject.Find("ScanController").GetComponent<AudioSource>().Pause();
            BundlerHander.Instance.ParentRef.GetComponentInChildren<AudioSource>().Play();
            audioplayenable = false;
        }
    }
}
