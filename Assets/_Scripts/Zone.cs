using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

namespace Prizm {
	public class Zone : MonoBehaviour {

		private TypeOfZone _myType;
		public TypeOfZone myType {
			get { return _myType; }
			set 
			{
				//Debug.LogWarning ("setting type of " + gameObject.name + "piece to: " + value.ToString ());
				_myType = value; 
			}
		}

		[System.NonSerialized]
		public List<Piece> hostedPieces = new List<Piece>();

		private AudioSource myAudioSource;

		public List<AudioClip> myAudioClips;


		void Awake() {
			myAudioSource = GetComponent<AudioSource> ();
		}



		//this should be called everytime a specialized, custom macro fires
		private void OnMacro() {
			PlayRandomSound ();
		}


		//execute this on macro
		private void PlayRandomSound() {
			myAudioSource.Play ();
			
		}

	}
}