using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleArea : BasicArea {
	public Deck HardDeck;

	public void SetHardDeck (List<GameObject> cards) {
		SetDeck (cards, HardDeck);
	}

	public void ShuffleHardDeck () {
		HardDeck.Shuffle ();
	}

	public uint[] GetHardDeckOrder () {
		return GetDeckOrder (HardDeck);
	}

	public void SetHardDeckOrder (uint[] order) {
		SetDeckOrder (HardDeck, order);
	}
}
