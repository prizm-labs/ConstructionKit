using UnityEngine;
using System.Collections;
using TouchScript.Gestures;


public class CodeNamesDeck : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<TapGesture>().Tapped += DealCards;
	}

	void DealCards (object sender, System.EventArgs e)
	{
		Debug.Log ("double tapped, calling dealCards()");
		GameManager.Instance.DealCards ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
