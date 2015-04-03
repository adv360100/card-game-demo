using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static public GameManager Instance;

	public Text InstructionsText;
	public Text PlayerInfoText;
	public ObstacleArea ObstacleArea;
	public BlackMarketArea BlackMarketArea;
	public BasicArea CrossfireArea;
	public PlayerArea MyPlayer; // The player on this machine
	public PlayerArea[] Players;
	public ObstacleActions ObstaclePanel;
	public bool IsSyncing = true;

	private enum GamePhases {GamePhasesCrossfire = 0, GamePhasesPlayer, GamePhasesEnd, GamePhasesMAX};
	private enum PlayerPhases {PlayerPhasesPlay = 0, PlayerPhasesApplyDamage, PlayerPhasesTakeDamage, PlayerPhasesDraw, PlayerPhasesBuy, PlayerPhasesEnd, PlayerPhasesMAX};
	private GamePhases CurGamePhase = 0;
	private PlayerPhases CurPlayerPhase = PlayerPhases.PlayerPhasesEnd;
	private int NumOfPlayers;
	private int FirstPlayerIndex = 0;
	private int CurPlayerIndex = 0;

	// Use this for initialization
	void Start () {
		List<PersistantManager.PlayerInfo> playerInfos = PersistantManager.GetInstance ().Players;
		NumOfPlayers = playerInfos.Count;

		if (Instance == null) {
			Instance = this;
		} else {
			DestroyObject (this);
			return;
		}

		// Load all the decks
		BlackMarketArea.SetMainDeck (JSONImporter.LoadAllFromFolder ("PlayerCards", (uint)CardIDs.CardIDPlayerCards));
		ObstacleArea.SetMainDeck (JSONImporter.LoadAllFromFolder ("Obstacles", (uint)CardIDs.CardIDObstacles));
		ObstacleArea.SetHardDeck (JSONImporter.LoadAllFromFolder ("HardObstacles", (uint)CardIDs.CardIDHardObstacles));
		CrossfireArea.SetMainDeck (JSONImporter.LoadAllFromFolder ("Crossfire", (uint)CardIDs.CardIDCrossfire));

		if (Network.isServer) {
			MyPlayer = Players[0];
			if (NumOfPlayers > 1) {
				for (int i = 1; i < NumOfPlayers; i++) {
					networkView.RPC ("SetMyPlayer", PersistantManager.GetInstance ().Players[i].Player, Players[i].networkView.viewID);
				}
			}

			for (int i = 0; i < NumOfPlayers; i++) {
				Players[i].SetMainDeck (BlackMarketArea.PullPlayerDeck (playerInfos[i].Role, Players[i]));
				Players[i].ShuffleMainDeck ();
				networkView.RPC ("SetPlayerDeckOrder", RPCMode.Others, new object[] {ArrayToString (Players[i].GetMainDeckOrder ()), Players[i].networkView.viewID});
			}

			BlackMarketArea.ShuffleMainDeck ();
			ObstacleArea.ShuffleMainDeck ();
			ObstacleArea.ShuffleHardDeck ();
			CrossfireArea.ShuffleMainDeck ();

			networkView.RPC ("SetBlackMarketDeckOrder", RPCMode.Others, ArrayToString (BlackMarketArea.GetMainDeckOrder ()));
			networkView.RPC ("SetObstacleDeckOrder", RPCMode.Others, ArrayToString (ObstacleArea.GetMainDeckOrder ()));
			networkView.RPC ("SetHardObstacleDeckOrder", RPCMode.Others, ArrayToString (ObstacleArea.GetHardDeckOrder ()));
			networkView.RPC ("SetCrossfireDeckOrder", RPCMode.Others, ArrayToString (CrossfireArea.GetMainDeckOrder ()));

			IsSyncing = false;
		}

		SetPlayerInfo ();
		DrawCrossFire ();
	}

	[RPC]
	void SetPlayerDeckOrder (string order, NetworkViewID id) {
		GetPlayerAreaForNetworkViewID (id).SetMainDeckOrder (StringToArray (order));
	}

	[RPC]
	void SetBlackMarketDeckOrder (string order) {
		BlackMarketArea.SetMainDeckOrder (StringToArray (order));
	}

	[RPC]
	void SetObstacleDeckOrder (string order) {
		ObstacleArea.SetMainDeckOrder (StringToArray (order));
	}

	[RPC]
	void SetHardObstacleDeckOrder (string order) {
		ObstacleArea.SetHardDeckOrder (StringToArray (order));
	}

	[RPC]
	void SetCrossfireDeckOrder (string order) {
		CrossfireArea.SetMainDeckOrder (StringToArray (order));
		IsSyncing = false;
	}

	[RPC]
	void SetMyPlayer (NetworkViewID id) {
		for (int i = 0; i < Players.Length; i++) {
			if (Players[i].networkView.viewID == id) {
				MyPlayer = Players[i];
				return;
			}
		}
	}

	string ArrayToString (uint[] array) {
		string str = "";
		foreach (var item in array) {
			if (str != "") {
				str += ",";
			}
			str += item.ToString ();
		}

		return str;
	}

	uint[] StringToArray (string str) {
		string[] strArray = str.Split (',');
		uint[] uintArray = new uint[strArray.Length];

		for (int i = 0; i < strArray.Length; i++) {
			uintArray[i] = uint.Parse (strArray[i]);
		}

		return uintArray;
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
		networkView.RPC ("NextStepAction", RPCMode.All, null);
	}

	[RPC]
	void NextStepAction () {
		
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

		card.GetComponent<Card> ().QuadraticOutMoveTo (card.transform.position, ObstacleArea.DiscardPile.transform.position, 1.0f, () => {
			ObstacleArea.DiscardPile.AddCard (card.gameObject);
			card.tag = "";
			ObstacleActions.Instance.CheckObstacleButtons ();
			card.GetComponent<Renderer>().enabled = false;
		});
	}

	void SetPlayerInfo()
	{
		string str = PersistantManager.GetInstance ().SelectedRace.ToString ();
		str += " " + PersistantManager.GetInstance ().SelectedRole.ToString ();
		str = str.Replace ("Race", "");
		str = str.Replace("Role","");
		PlayerInfoText.text = str;
	}

	public PlayerArea GetPlayerAreaForNetworkViewID (NetworkViewID id) {
		for (int i = 0; i < Players.Length; i++) {
			if (Players[i].networkView.viewID == id) {
				return Players[i];
			}
		}

		return null;
	}
}
