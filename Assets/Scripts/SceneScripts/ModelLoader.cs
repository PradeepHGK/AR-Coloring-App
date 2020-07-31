using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Collections;

public class ModelLoader : Pixelplacement.Singleton<ModelLoader>
{
    public GameObject Model { get; private set; }

    private void OnEnable()
    {
        EventManager.Instance.OnTrackingFound += LoadModel_OnTrackingFound;
        EventManager.Instance.OnTrackingLost += OnTrackingLost;
    }


    private void OnDisable()
    {
        EventManager.Instance.OnTrackingFound -= LoadModel_OnTrackingFound;
        EventManager.Instance.OnTrackingLost -= OnTrackingLost;
    }
    private void OnTrackingLost()
    {
        if (Model != null)
            Model.SetActive(false);
        Debug.Log("ModelDisabled");
    }

    private void LoadModel_OnTrackingFound(string trackableName, GameObject trackableObject)
    {
        Debug.LogWarning("trackableName -->" + trackableName + "------->" + trackableObject.name);
        if (trackableObject.transform.childCount == 0)
        {
            AssetBundleRequest targetAssetBundleRequest1 = AssetbundleManager.Instance.DeltaAssetbundle.LoadAssetAsync(trackableName + ".fbx", typeof(GameObject));
            GameObject ModelGameObject1 = targetAssetBundleRequest1.asset as GameObject;
            Model = Instantiate(ModelGameObject1);
            Model.transform.SetParent(trackableObject.transform);
            AssignRC(trackableObject);
            ApplyModelScaleRotation(Model);

            if (AssetbundleManager.Instance.DeltaAssetbundle.LoadAsset<AudioClip>(trackableName + ".mp3") != null)
                EventManager.Instance.PlayAudioInvoke(AssetbundleManager.Instance.DeltaAssetbundle.LoadAsset<AudioClip>(trackableName + ".mp3"));
        }
        else
        {
                Model.SetActive(true);
        }
    }

    private void AssignRC(GameObject trackableObj)
    {
        Component[] ModelSkinMesh = trackableObj.transform.GetChild(0).GetComponentsInChildren<SkinnedMeshRenderer>();
        Component[] ModelMesh = trackableObj.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();

        foreach (SkinnedMeshRenderer Mesh in ModelSkinMesh)
        {
            Mesh.gameObject.AddComponent<RC_Get_Texture>();
        }

        foreach (MeshRenderer Mesh in ModelMesh)
        {
            Mesh.gameObject.AddComponent<RC_Get_Texture>();
        }
    }

    private void ApplyModelScaleRotation(GameObject Model)
    {
        Model.transform.localScale = new Vector3(3, 3, 3);
        Model.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
