using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

namespace Prizm {
	public class SnapZone : Zone {

		void Awake() {
			myType = TypeOfZone.storedZone;
		}

		void OnTriggerEnter(Collider other) {
			//if we collided with a piece object
			if (other.GetComponent<Piece> () != null) {
				other.GetComponent<Piece> ().mySnapZone = this;
			}
		}

		void OnTriggerExit(Collider other) {
			if (other.GetComponent<Piece> () != null) {
				other.GetComponent<Piece> ().mySnapZone = null;
			}
		}

	}


}