using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vuforia;

public class ChangeName : MonoBehaviour, IPointerClickHandler {


	static string buttonName;

	ImageTargetBehaviour imageTargetBehaviourObject;

	private void Awake()
	{
	   
	}

	// Use this for initialization
	void Start () {
		imageTargetBehaviourObject = GetComponent<ImageTargetBehaviour>();

		ChangeObjName();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//I named my button the name of the scene i want to load
		//SceneManager.LoadScene(eventData.pointerPress.name);
		Debug.Log("Switching level: " + eventData.pointerPress.name);

		buttonName = eventData.pointerPress.name;


		switch (buttonName)
		{
			case "Image":

				Debug.Log("FirstImage");
				break;

			case "Image (3)":
				Debug.Log("ThirdImage");
				break;

			case "Image (2)":
				Debug.Log("SecondImage");
				break;

			case "Image (1)":
				Debug.Log("FirstImage");
				break;
		}
	}


	void ChangeObjName()
	{
	
		Debug.Log(DataSet.Exists("MagicToonz"));


	   
		Debug.Log("1");
		foreach (GameObject item in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			Debug.Log("2");
			Debug.Log(item.name);
			if (item.name == "New Game Object")
			{
				Debug.Log("3");

				item.AddComponent<DefaultTrackableEventHandler>();
				item.AddComponent<TurnOffBehaviour>();
				//imageTargetBehaviourObject = item.GetComponent<ImageTargetBehaviour>();
				item.name = DefaultTrackableEventHandler.mTrackableBehaviour.TrackableName;

			}
		}


	   

		
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
