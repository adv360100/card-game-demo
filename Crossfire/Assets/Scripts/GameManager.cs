using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	static public GameManager Instance;

	public Text InstructionsText;
	public BasicArea ObstacleArea;
	public PlayerArea MyPlayer; // The player on this machine
	public PlayerArea[] Players = new PlayerArea[4];

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
		NumOfPlayers = 1;

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
		if (GetCurrentPlayerArea ().Hand.CardList.Count > 3)
		{
			NextStep ();
		} 
		else
		{
			InstructionsText.text = "Player " + (CurPlayerIndex + 1) + " draw 2 cards from your deck";
		}
	}

	void BuyCards () {
		//TODO: check if current player has money
		//TODO: check if can even buy anything in the market
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

	public static PlayerArea GetCurrentPlayerArea () {
		return Instance.Players[Instance.CurPlayerIndex];
	}

	public void AddObstacleToMyPlayer (GameObject card) {
		MyPlayer.ObstacleSection.AddCard (card);
	}

	public void DiscardObstacle (GameObject card) {
		if (ObstacleArea.DiscardPile.ContainsCard (card.GetComponent<Card> ())) {
			return;
		}

		ObstacleArea.DiscardPile.AddCard (card);

		card.GetComponent<Card> ().QuadraticOutMoveTo (card.transform.position, ObstacleArea.DiscardPile.transform.position, 1.0f, () => {
			ObstacleArea.DiscardPile.AddCard (card.gameObject);
			card.tag = "";
			card.GetComponent<Renderer>().enabled = false;
		});
	}
}
