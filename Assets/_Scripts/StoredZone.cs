using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

namespace Prizm {
	public class StoredZone : Zone {

		void Awake() {
			myType = TypeOfZone.storedZone;
		}

		void OnTriggerEnter(Collider other) {
			if (other.GetComponent<Piece> () != null) {
				if (other.GetComponent<Piece> ().myType == TypeOfPiece.cardPiece) {
					Vector3 setPosition = transform.position;
					setPosition.y = (float)GameLayers.pieceLayer;
					other.transform.position = setPosition;


					other.GetComponent<TransformGesture> ().Cancel ();
				}
			}
		}

	}


}