using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;

public class StackMenuManager : MonoBehaviour {

	[Header("Gameobject References")]
	[SerializeField] private GameObject ButtonsRoot;

	[Header("Button References")]
	[SerializeField] private Button infoBtn;
	[SerializeField] private Button muteBtn;
	[SerializeField] private Button unmuteBtn;
	[SerializeField] private Button playAnimBtn;
	[SerializeField] private Button stopAnimBtn;

	[Header("Boolean References")]
	[SerializeField] private bool isControlEnabled;

	void Awake()
    {

    }

	public void OnClickSelectionButton()
	{
		isControlEnabled = !isControlEnabled;
		if(isControlEnabled)
			Tween.LocalPosition(ButtonsRoot.transform, new Vector3(170, -50f, 0), 1f, 0f, Tween.EaseSpring);
		else
			Tween.LocalPosition(ButtonsRoot.transform, new Vector3(-200f, -50f, 0), 1f, 0f, Tween.EaseOutBack);
	}
}
