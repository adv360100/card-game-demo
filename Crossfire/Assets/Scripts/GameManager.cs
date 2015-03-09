using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static public GameManager Instance;

	public Text InstructionsText;
	public PlayerArea MyPlayer; // The player on this machine

	private enum GamePhases {GamePhasesCrossfire = 0, GamePhasesPlayer, GamePhasesEnd, GamePhasesMAX};
	private enum PlayerPhases {PlayerPhasesPlay = 0, PlayerPhasesApplyDamage, PlayerPhasesTakeDamage, PlayerPhasesDraw, PlayerPhasesBuy, PlayerPhasesEnd, PlayerPhasesMAX};
	private GamePhases CurGamePhase = 0;
	private PlayerPhases CurPlayerPhase = PlayerPhases.PlayerPhasesEnd;
	private int NumOfPlayers;
	private int FirstPlayerIndex = 0;
	private int CurPlayerIndex = 0;

	// Use this for initialization
	void Start () {
		//TODO: find out the number of players
		NumOfPlayers = 4;

		if (Instance == null) {
			Instance = this;
		} else {
			DestroyObject(this);
			return;
		}

		DrawCrossFire ();
	}

	void DrawCrossFire () {
		InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " draw Crossfire card";
		//first player can
	}

	void PlayCards () {
		InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " play cards from your hand";
	}

	void ApplyDamage () {
		InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " apply damage";
	}

	void TakeDamage () {
		InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " take damage from obstacles";
	}

	void DrawCards () {
		InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " draw 2 cards from your deak";
	}

	void BuyCards () {
		InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " can buy from the Black Market";
	}

	void EndTurn () {
		InstructionsText.text = "End Turn";
		CurPlayerIndex++;
		if (CurPlayerIndex >= NumOfPlayers) {
			CurPlayerIndex = 0;
		}

		//TODO: check if end scene condition is met

		NextStep ();
	}

	void EndRound () {
		InstructionsText.text = "End Round";
		FirstPlayerIndex++;
		if (FirstPlayerIndex >= NumOfPlayers) {
			FirstPlayerIndex = 0;
		}
		CurPlayerIndex = FirstPlayerIndex;

		NextStep ();
	}

	public void NextStep () {

		if (FirstPlayerIndex == CurPlayerIndex && CurPlayerPhase == PlayerPhases.PlayerPhasesEnd) {
			CurGamePhase++;
			if (CurGamePhase == GamePhases.GamePhasesMAX)
				CurGamePhase = 0;
		}
		switch (CurGamePhase) {
		case GamePhases.GamePhasesCrossfire:
			DrawCrossFire ();
			break;
		case GamePhases.GamePhasesPlayer:
			NextPlayerStep ();
			break;
		case GamePhases.GamePhasesEnd:
			EndRound ();
			break;
		}
	}

	void NextPlayerStep () {
		CurPlayerPhase++;
		if (CurPlayerPhase == PlayerPhases.PlayerPhasesMAX)
			CurPlayerPhase = 0;

		switch (CurPlayerPhase) {
		case PlayerPhases.PlayerPhasesPlay:
			PlayCards ();
			break;
		case PlayerPhases.PlayerPhasesApplyDamage:
			ApplyDamage ();
			break;
		case PlayerPhases.PlayerPhasesTakeDamage:
			TakeDamage ();
			break;
		case PlayerPhases.PlayerPhasesDraw:
			DrawCards ();
			break;
		case PlayerPhases.PlayerPhasesBuy:
			BuyCards ();
			break;
		case PlayerPhases.PlayerPhasesEnd:
			EndTurn ();
			break;
		}
	}

	public static BasicArea GetCurrentPlayerArea () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		//TODO: do some real stuff

		return player.GetComponent<BasicArea> ();
	}

	public void AddObstacleToMyPlayer (GameObject card) {
		MyPlayer.ObstacleSection.AddCard (card);
	}
}
