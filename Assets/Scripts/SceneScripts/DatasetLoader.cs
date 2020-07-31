using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;
using System.IO;

public class DatasetLoader : MonoBehaviour
{

    void Awake()
    {
        //StartCoroutine(DownloadVuforiaDataSet);
    }

    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }


    private IEnumerator DownloadVuforiaDataSet(string url, string fileName)
    {
        WWW www = new WWW(url);
        yield return null;

        while (!www.isDone)
        {
            Debug.Log("Download Vuforia Content: " + www.progress);
        }

        if (www.isDone)
        {
            File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, www.bytes);
        }
    }

    void OnVuforiaStarted()
    {
        // The 'path' string determines the location of xml file
        // For convinence the RealTime.xml is placed in the StreamingAssets folder
        // This file can be downloaded and the relative path will be used accordingly

        string path = "";
#if UNITY_IOS || UNITY_ANDROID
        //path = Application.dataPath + "/Raw/RealTime.xml";
        //path = "jar:file://" + Application.dataPath + "!/assets/RealTime.xml";
        //path = Application.dataPath + "/XML/MagicToonz.xml";
        path = Application.dataPath + "/StreamingAssets/Vuforia/MagicToonz.xml";
#else
		path = Application.dataPath + "/StreamingAssets/Vuforia/MagicToonz.xml";
#endif
        Debug.Log("Path" + path);

        bool status = LoadDataSet(path, VuforiaUnity.StorageType.STORAGE_ABSOLUTE);

        if (status)
        {
            Debug.Log("Dataset Loaded");
        }
        else
        {
            Debug.Log("Dataset Load Failed");
        }
    }

    private bool LoadDataSet(string dataSetPath, VuforiaUnity.StorageType storageType)
    {
        // Request an ImageTracker instance from the TrackerManager.
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        objectTracker.Stop();


        IEnumerable<DataSet> dataSetList = objectTracker.GetActiveDataSets();

        foreach (DataSet set in dataSetList.ToList())
        {
            objectTracker.DeactivateDataSet(set);
        }

        // Check if the data set exists at the given path.
        if (!DataSet.Exists(dataSetPath, storageType))
        {
            Debug.LogError("Data set " + dataSetPath + " does not exist.");
            return false;
        }

        // Create a new empty data set.
        DataSet dataSet = objectTracker.CreateDataSet();

        //if (!objectTracker.ActivateDataSet(dataSet))
        //{
        //    // Note: ImageTracker cannot have more than 100 total targets activated
        //    Debug.Log("<color=yellow>Failed to Activate DataSet: " + dataSetList.ToList<DataSet>()[0] +  "</color>");
        //}

        //if (!objectTracker.Start())
        //{
        //    Debug.Log("<color=blue>Tracker Failed to Start.</color>");
        //}

        // Load the data set from the given path.
        if (!dataSet.Load(dataSetPath, storageType))
        {
            Debug.LogError("Failed to load data set " + dataSetPath + ".");
            return false;
        }

        // (Optional) Activate the data set.
        objectTracker.ActivateDataSet(dataSet);
        objectTracker.Start();

        foreach (var item in objectTracker.GetActiveDataSets())
        {
            Debug.Log("Active DataSets: " + item.Path);
        }

        AttachContentToTrackables(dataSet);
        return true;

    }

    private void AttachContentToTrackables(DataSet dataSet)
    {
        // get all current TrackableBehaviours
        IEnumerable<TrackableBehaviour> trackableBehaviours =
        TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();


        // Loop over all TrackableBehaviours.
        foreach (TrackableBehaviour trackableBehaviour in trackableBehaviours)
        {
            if (trackableBehaviour.name == "New Game Object")
            {
                Debug.LogWarning("trackableBehaviours");
                Debug.LogWarning("Trackables Name" + trackableBehaviour.TrackableName);
                GameObject go = trackableBehaviour.gameObject;
                go.AddComponent<DefaultTrackableEventHandler>();
                go.gameObject.AddComponent<TurnOffBehaviour>();
                go.name = trackableBehaviour.TrackableName;

            }
            //         for (int i = 0; i < trackableBehaviours.Count<TrackableBehaviour>(); i++)
            //         {
            //}

            // check if the Trackable of the current Behaviour is part of this dataset
            /*			if (dataSet.Contains(trackableBehaviour.Trackable))
                        {
                            // Add a Trackable event handler to the Trackable.
                            // This Behaviour handles Trackable lost/found callbacks.
                            GameObject go = trackableBehaviour.gameObject;
                            go.AddComponent<DefaultTrackableEventHandler>();
                            go.name = trackableBehaviour.TrackableName;
                            Debug.Log("Attach Trackables");
                            // Instantiate the model.
                            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            //GameObject cube = Instantiate(Model) as GameObject;

                            //// Attach the cube to the Trackable and make sure it has a proper size.
                            //cube.transform.parent = trackableBehaviour.transform;
                            //cube.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                            //cube.transform.localPosition = new Vector3(0.0f, 0.35f, 0.0f);
                            //cube.transform.localRotation = Quaternion.identity;
                            //cube.SetActive(true);
                            //trackableBehaviour.gameObject.SetActive(true);
                        }
            */
        }
    }
}