using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

namespace Prizm {
	public class DeckZone : Zone {

		[SerializeField]
		private List<StoredZone> targetDealLocations;

		private TapGesture myTapGesture;
		private PressGesture myPressGesture;

		public bool generateCardsByScript = false;


		void Awake() {
			myType = TypeOfZone.deckZone;

			myPressGesture = GetComponent<PressGesture> ();
			myTapGesture = GetComponent<TapGesture> ();

			if (myPressGesture == null || myTapGesture == null) {
				Debug.LogError("this zone does not have a press or tap gesture!");
			}

		}


		public void DealCards() {
			foreach (StoredZone sz in targetDealLocations) {
				//if there is not a card in our target location, deal a card from our list and put it there
				if (sz.hostedPieces.Count == 0) {
					MovePieceToZone(hostedPieces[0], sz);
					hostedPieces.RemoveAt (0);
				}
			}
		}

	
		public void MovePieceToZone(Piece pieceToMove, Zone targetZone) {
			pieceToMove.gameObject.transform.position = targetZone.gameObject.transform.position;
		}

		//shuffle all the cards around
		public void ShuffleCards() {
			foreach (Piece p in hostedPieces) {
				

			}
		}
	}


}