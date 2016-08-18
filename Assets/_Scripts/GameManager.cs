using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


	private Camera mainCamera;

	public GameObject wordCardPrefab;

	public static GameManager Instance;

	public List<GameObject> cardsOutInField = new List<GameObject> ();

	[System.NonSerialized]
	public static float DistanceFromCamera;
	private float BoundariesHeight = 5000.0f;

	public List<string> gameWordList = new List<string>();

	public void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();

		DistanceFromCamera = (mainCamera.gameObject.transform.position.y - GameObject.Find ("Floor").transform.position.y) * 0.3f;
		//Debug.Log ("distance from camera is set at: " + DistanceFromCamera.ToString ());

		//CreateBoundariesDice ();

	}

	List<string> CreateTempWordList() {
		gameWordList.Clear ();

		foreach (string word in MasterWordList.masterWordList) {
			gameWordList.Add (word);
		}
		Debug.Log ("we have " + gameWordList.Count.ToString () + " words in game word list");

		List<string> tempWordList = new List<string> ();

		for (int i = 0; i < 25; i++) {
			int randNum = Random.Range (0, gameWordList.Count);
			tempWordList.Add (gameWordList [randNum]);
			//Debug.Log ("added " + gameWordList [randNum].ToString () + " to word list");
			gameWordList.RemoveAt (randNum);
			//Debug.Log ("size of word list: " + gameWordList.Count.ToString ());
		}

		return tempWordList;
	}

	public void ClearCards() {

		foreach (GameObject go in cardsOutInField) {
			Destroy (go);
		}

		cardsOutInField.Clear ();

	}

	public void DealCards() {
		ClearCards ();


		int xPos = -160;
		int zPos = 110;

		foreach (string word in CreateTempWordList()) {
			GameObject newCard = Instantiate (wordCardPrefab) as GameObject;
			newCard.transform.GetChild (0).Find ("topText").GetComponent<Text> ().text = word;
			newCard.transform.GetChild (0).Find ("botText").GetComponent<Text> ().text = word;

			newCard.transform.position = new Vector3 (xPos, -260, zPos);
			xPos += 80;
			if (xPos > 161) {
				zPos -= 40;
				xPos = -160;
			}

			cardsOutInField.Add (newCard);
		}
	}


	//creates walls so balls can't escape world
	public void CreateBoundariesDice() {
		List<GameObject> boundaries = new List<GameObject> ();

		Vector3 lowerLeft = mainCamera.ViewportToWorldPoint (new Vector3 (0.05f, 0.05f, DistanceFromCamera));
		Vector3 lowerRight = mainCamera.ViewportToWorldPoint (new Vector3 (0.95f, 0.05f, DistanceFromCamera));
		Vector3 upperLeft = mainCamera.ViewportToWorldPoint (new Vector3 (0.05f, 0.95f, DistanceFromCamera));
		Vector3 upperRight = mainCamera.ViewportToWorldPoint (new Vector3 (0.95f, 0.95f, DistanceFromCamera));

		float width = lowerRight.x - lowerLeft.x;
		float height = upperRight.z - lowerRight.z; 


		Vector3 bottom = (lowerLeft + lowerRight ) / 2;
		Vector3 top = (upperLeft + upperRight ) / 2;
		Vector3 left = (upperLeft + lowerLeft ) / 2;
		Vector3 right = (lowerRight + upperRight ) / 2;


		GameObject bottomBound = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bottomBound.transform.position = bottom;
		bottomBound.transform.localScale = new Vector3 (width, BoundariesHeight, 5f);

		GameObject topBound = GameObject.CreatePrimitive(PrimitiveType.Cube);
		topBound.transform.position = top;
		topBound.transform.localScale = new Vector3 (width, BoundariesHeight, 5f);

		GameObject leftBound = GameObject.CreatePrimitive(PrimitiveType.Cube);
		leftBound.transform.position = left;
		leftBound.transform.localScale = new Vector3 (5f, BoundariesHeight, height);

		GameObject rightBound = GameObject.CreatePrimitive(PrimitiveType.Cube);
		rightBound.transform.position = right;
		rightBound.transform.localScale = new Vector3 (5f, BoundariesHeight, height);

		boundaries.Add (bottomBound);
		boundaries.Add (topBound);
		boundaries.Add (leftBound);
		boundaries.Add (rightBound);

		foreach (GameObject bond in boundaries) {
			bond.AddComponent<Rigidbody> ();
			bond.GetComponent<Rigidbody> ().useGravity = false;
			bond.GetComponent<Rigidbody> ().isKinematic = true;
			bond.name = "diceBoundary";
		}

		bottomBound.layer = 8;	//Dice layer
		topBound.layer = 8;	//Dice layer
		leftBound.layer = 8;	//Dice layer
		rightBound.layer = 8;	//Dice layer

		//make boundaries invisible
		Destroy(bottomBound.GetComponent<MeshRenderer>());
		Destroy(bottomBound.GetComponent<MeshCollider>());

		Destroy(topBound.GetComponent<MeshRenderer>());
		Destroy(topBound.GetComponent<MeshCollider>());

		Destroy(leftBound.GetComponent<MeshRenderer>());
		Destroy(leftBound.GetComponent<MeshCollider>());

		Destroy(rightBound.GetComponent<MeshRenderer>());
		Destroy(rightBound.GetComponent<MeshCollider>());
	}
}
