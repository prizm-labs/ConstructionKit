﻿using UnityEngine;
using System.Collections;

public enum ObjectCreatorButtons {Player = 0, Dice, Cards, Common, Custom, LoadBG, BGMusic}

//public enum TypeOfPiece {dice6 = 0, playerCircle, sprite2d, card, cannon, deckCards_playing, deckCards_risk, circle, coneHalf, cross, diamond, hexagon, icosphere, pyramid, star, token, GUICoin, attackBot, cavalry, heart, meeple, meeple_standing, pawn, soldier, dice6_red, dice6_blue, RiskBoard0, RiskBoard1, RiskBoard2, RiskBoard3, RiskBoard4, RiskBoard5, zMonopoly0}

public enum TypeOfPiece {gamePiece = 0, playerPiece, cardPiece, dicePiece, currencyPiece, tutorialPiece}
public enum TypeOfZone {snapZone = 0, oceanZone, dockZone, deckZone, storedZone, targetZone, throwZone}
public enum GameLayers {floorLayer = 0, pieceLayer = -250}

public enum TypeOfSound {spawned = 0, pickedUp, moved, macro} 

public enum Location {onBoard = 0, inDrawer}


public class Constants {
	public static float timeDelayToLoad = 0.25f;

}