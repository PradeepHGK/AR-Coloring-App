using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Collections;

public class ModelLoader : Pixelplacement.Singleton<ModelLoader>
{
    private GameObject Model { get; set; }
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
    }

    private void LoadModel_OnTrackingFound(string trackableName, GameObject trackableObject)
    {
        if (trackableObject.transform.childCount == 0)
        {
            AssetBundleRequest targetAssetBundleRequest1 = AssetbundleManager.Instance.DeltaAssetbundle.LoadAssetAsync(trackableName + ".prefab", typeof(GameObject));
            GameObject ModelGameObject1 = targetAssetBundleRequest1.asset as GameObject;
            Model = Instantiate(ModelGameObject1);
            Model.transform.SetParent(trackableObject.transform);
            AssignRC(trackableObject);
            ApplyModelScaleRotation(Model);
            EventManager.Instance.PlayAudioInvoke(AssetbundleManager.Instance.DeltaAssetbundle.LoadAsset<AudioClip>(trackableName + ".mp3"));
        }
        else
        {
            trackableObject.transform.GetChild(0).gameObject.SetActive(true);
            var audioClip = AssetbundleManager.Instance.DeltaAssetbundle.LoadAsset<AudioClip>(trackableName + ".mp3");
            //Debug.Log($"SecondTimeAudio: {audioClip.name}");
            EventManager.Instance.PlayAudioInvoke(audioClip);
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
