using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Vuforia;


public class LiDicSort : MonoBehaviour {

    ImageTargetBehaviour imagetargetbehav;
    AssetBundle newasset;
    
    List<string> storeAssetName = new List<string>();

    //Multiple Objects in arrays by using objects 
    object[] multipleTypeArray = new object[3];

    string[] names = new string[3];


    //Enums
    public enum stateMachine : byte
    {
        walkSpeed = (byte)1.0f,
        jumpAngle = 45, 
    }





    // Use this for initialization
    void Start()
    {
        //String Class
        names[0] = "Hari";
        names[1] = "Pradeep";
        names[2] = "Sumathi";
        names[3] = "Sandhiya";
        names[3] = "Mekala";

        var enumtype = stateMachine.jumpAngle;

        var jumpAngle = 500;

        //This is to convert it to enums.
        print((stateMachine)jumpAngle);


        //This is to change the other types of data to enums using Enum.Parse
        string addstringToEnum = "StringtoEnum";
        var stateMachines = (stateMachine)Enum.Parse(typeof(stateMachine), addstringToEnum);

        var arraysample = new int[5];

        var Personame = string.Join("-", names);

        //Objects Array
        multipleTypeArray[0] = "String";
        multipleTypeArray[1] = 1;
        multipleTypeArray[2] = 1.5f;
        multipleTypeArray[3] = true;
        multipleTypeArray[4] = false;


        //verbatim strings
        string links = @"hello, " +
            @"how are you?   who am i?" +
            @"c:folder\documents\projects.zip";

        System.Console.WriteLine(links);

        imagetargetbehav = GameObject.Find("New Game Object").GetComponent<ImageTargetBehaviour>();

        Debug.Log("targetName: " + imagetargetbehav.ImageTarget.Name);

        for (int i = 0; i < GameObject.FindObjectsOfType<GameObject>().Length; i++)
        {

            if (GameObject.Find("New Game Object"))
            {
                GameObject.Find("New Game Object").gameObject.name = imagetargetbehav.ImageTarget.Name;

            }

        }
        

        foreach (string item in newasset.GetAllAssetNames() )
        {
            //storeAssetName.AddRange();  
        }
    }


    IEnumerator Getbundle()
    {
        using (UnityWebRequest bundles = UnityWebRequest.GetAssetBundle("http://myserver/myBundle.unity3d"))
        {
            yield return bundles.SendWebRequest();

            // Get an asset from the bundle and instantiate it.
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(bundles);
            var loadAsset = bundle.LoadAssetAsync<GameObject>("Assets/Players/MainPlayer.prefab");
            yield return loadAsset;

            Instantiate(loadAsset.asset);
        }
    }


    void Checktrycatch()
    {
        try
        {
            var getTex = new RC_Get_Texture();
            //getTex.RenderCamera = 
        }
        catch (System.Exception)
        {

            throw;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
