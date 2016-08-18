using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using System.Linq;
using System;


namespace Prizm {

	[System.Serializable]
	public class Piece : MonoBehaviour {

		[SerializeField]
		private TypeOfPiece _myType;
		public TypeOfPiece myType {
			get { return _myType; }
			set 
			{
				Debug.LogWarning ("setting type of " + gameObject.name + "piece to: " + value.ToString ());
				_myType = value; 
			}
		}



		public static Color defaultNewPieceColor = Color.white;

		private ObjectCreatorButtons _myCategory;
		public ObjectCreatorButtons myCategory {
			get { return _myCategory; }
			set 
			{
				_myCategory = value; 

			}
		}


		private Color _myColor;
		public Color myColor {
			get { return _myColor;}
			set {
				_myColor = value;

				if (bootstrapped) {
					SetMeshesColors (_myColor);
				} else {
					SetMeshesColorsDelay (_myColor);
				}
			}
		}

		private SnapZone _mySnapZone;
		public SnapZone mySnapZone {
			get { return _mySnapZone; }
			set {
				_mySnapZone = value;
			}
		}


		[SerializeField]
		public AudioSource audioSource;


		[SerializeField]
		public bool bootstrapped = false;
		[SerializeField]
		public List<AudioClip> myAudioClips = new List<AudioClip>();

		//used only if the piece is a deck of cards
		[SerializeField]
		public List<GameObject> myPotentialCardPrefabs;

		[SerializeField]
		public Color myStoredColor;

		void Awake(){
			audioSource = GetComponent<AudioSource> ();
		}

		void Start () {
			gameObject.AddComponent<ApplyTransform> ();

		}



		void OnEnable() {
			GetComponent<TransformGesture>().TransformCompleted += transformCompleted;
		}

		void transformCompleted (object sender, EventArgs e)
		{
			Debug.Log ("transform completed");

			//Vector3 setPosition = transform.position;
			//setPosition.y = (float)GameLayers.pieceLayer;
			//other.transform.position = setPosition;

		}

		void DiceFlick (object sender, System.EventArgs e)
		{
			//Debug.Log ("dice flicked");
			//Debug.Log ("direction: " + GetComponent<FlickGesture> ().Direction.ToString ());

			Vector3 flickDirection = new Vector3(GetComponent<FlickGesture>().ScreenFlickVector.x, 50.0f, GetComponent<FlickGesture>().ScreenFlickVector.y);
			//Debug.Log("vector: " + flickDirection);
			GetComponent<Rigidbody> ().AddForce (flickDirection);
			GetComponent<Rigidbody> ().AddTorque (flickDirection);
		}

		public void Bootstrap() {
			Debug.Log ("Bootstrap() called");
			StartCoroutine (_Bootstrap ());
		}

		private IEnumerator _Bootstrap() {
			yield return StartCoroutine (LoadAudio ());



			//GetComponent<TransformGesture> ().Type = (TouchScript.Gestures.Base.TransformGestureBase.TransformType) 0x3;
			bootstrapped = true;

			myStoredColor = myColor;
			PlayASound ();
		}
			

		private IEnumerator LoadAudio() {
			myAudioClips = new List<AudioClip>(Resources.LoadAll (myCategory.ToString() + "/" + myCategory.ToString () + "Sounds", typeof(AudioClip)).Cast<AudioClip>().ToArray());
			if (myAudioClips.Count == 0) {
				Debug.LogError ("no sounds for: " + myCategory + "/" + myType.ToString ());
			}
			yield return null;
		}


		void SetMeshesColorsDelay(Color newColor) {
			StartCoroutine (WaitToSetMeshes (newColor));
		}

		IEnumerator WaitToSetMeshes(Color theColor) {
			yield return new WaitForSeconds (Constants.timeDelayToLoad / 2);
			SetMeshesColors (theColor);
		}
			

		void SetMeshesColors(Color newColor) {
			//Debug.Log ("setting all the meshes colors to: " + newColor.ToString ());

			//correct the color on all of the meshes
			if (transform.childCount == 0) return;

			if (transform.GetChild(0).gameObject.GetComponent<MeshRenderer> () != null) {
				transform.GetChild(0).gameObject.GetComponent<MeshRenderer> ().sharedMaterial.color = newColor;
			}

			for (int i = 0; i < transform.GetChild(0).childCount; i++) {
				if (transform.GetChild(0).GetChild(i).gameObject.GetComponent<MeshRenderer>() != null) {
					MeshRenderer tempMesh = transform.GetChild(0).GetChild (i).gameObject.GetComponent<MeshRenderer> ();
					tempMesh.sharedMaterial.color = newColor;		//set the unowned objects color to gray
				}
			}
		}

		void OnPressed (object sender, EventArgs e)
		{
			PlayASound ();
		}

		public void PlayASound() {
			if (myAudioClips.Count == 0) {
				Debug.LogError ("we have no audioClips for: " + transform.GetChild(0).name);
				return;
			}
			audioSource.volume = SoundManager.Instance.globalSFXVolume;
			audioSource.clip = myAudioClips [UnityEngine.Random.Range (0, myAudioClips.Count - 1)];
			audioSource.Play ();
		}


		//adds flick gesture, rigidbody, limits dragging to 2 finger drags
		public void ThisPieceIsADice() {
			_myColor = Color.white;
			GetComponent<TransformGesture> ().MinTouches = 2;

			gameObject.AddComponent<FlickGesture> ();
			GetComponent<FlickGesture>().Flicked += DiceFlick;

			if (gameObject.GetComponent<Rigidbody>() == null)
				gameObject.AddComponent<Rigidbody> ();
			GetComponent<Rigidbody> ().mass = 0.08f;
			GetComponent<Rigidbody> ().drag = 0.1f;
			GetComponent<Rigidbody> ().angularDrag = 0.01f;

			if (gameObject.GetComponent<BoxCollider>() == null)
				gameObject.AddComponent<BoxCollider> ();
			//gameObject.GetComponent<BoxCollider> ().size = new Vector3 (5.0f, 5.0f, 5.0f);
			gameObject.GetComponent<BoxCollider> ().isTrigger = false;
			gameObject.layer = 8;	//Dice Layer

			gameObject.tag = "Dice";
		}

		//adds double tap gesture to spawn a card of its category
		private void ThisPieceIsADeckOfCards() {
			myPotentialCardPrefabs = new List<GameObject>(Resources.LoadAll (myCategory.ToString() + "/" + myType.ToString () + "Cards", typeof(GameObject)).Cast<GameObject>().ToArray());

			if (gameObject.GetComponent<TapGesture> () == null) {
				gameObject.AddComponent<TapGesture> ();
			}
			gameObject.GetComponent<TapGesture> ().NumberOfTapsRequired = 2;
			gameObject.GetComponent<TapGesture>().Tapped += DeckTapped;

			//if (gameObject.GetComponent<BoxCollider>() == null)
			//	gameObject.AddComponent<BoxCollider> ();
			//gameObject.GetComponent<BoxCollider> ().size = new Vector3 (5.0f, 5.0f, 5.0f);S
		}


		void DeckTapped (object sender, EventArgs e)
		{
			DrawRandomCardFromDeck ();
		}

		private void DrawRandomCardFromDeck() {
			int randomCardIndex = UnityEngine.Random.Range (0, myPotentialCardPrefabs.Count - 1);
			GameObject theCardPrefab = myPotentialCardPrefabs[randomCardIndex];

			GameObject newCard = Instantiate (theCardPrefab);
			string theDataPath = "Cards/deckCards";

			if (theCardPrefab.name.ToLower ().Contains ("risk")) {
				theDataPath += "_riskCards/" + theCardPrefab.name;
			} else {
				theDataPath += "_playingCards/" + theCardPrefab.name;
			}

			Debug.Log("after drawing a random card, setting the card's data path to: " + theDataPath);


			newCard.GetComponent<CardPiece> ().GiveMeMyDataPath (theDataPath);

			newCard.transform.position = transform.position + Vector3.up * 8;
			if (newCard.name.ToLower ().Contains ("risk")) {
				newCard.transform.localScale = Vector3.one * 10.0f;
			} else {
				newCard.transform.localScale = Vector3.one * 150.0f;
			}
			newCard.GetComponent<CardPiece> ().FlipMeOver ();
		}

	}

}