using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DeckActionUT
{
	[TestFixture]
	[Category("Deck Action")]
	internal class DeckActionTests
	{
		[Test]
		public void AddCards_insertOneCard_deckHasOneCard()
		{
			GameObject test = new GameObject ();
			test.name = "AddCards_insertOneCard_deckHasOneCard";
			//arrange
			GameObject obj = Helpers.MakeDeckObject(test.transform);
			obj.transform.parent = test.transform;
			DeckActions deck = obj.GetComponent<DeckActions> ();
			GameObject[] cardToAdd = Helpers.MakeCards (1,test.transform);
			//act
			deck.AddCards (cardToAdd);
			//assert
			bool deckHasOneCard = (deck.DeckCount () == 1);
			Assert.That (deckHasOneCard);
			//cleanup
			GameObject.DestroyImmediate (test);
		}

		[Test]
		public void AddCards_insertTwoCards_deckHasTwoCards()
		{
			GameObject test = new GameObject ();
			test.name = "AddCards_insertTwoCards_deckHasTwoCards";
			GameObject obj = Helpers.MakeDeckObject(test.transform);
			DeckActions deck = obj.GetComponent<DeckActions> ();
			GameObject[] cardsToAdd = Helpers.MakeCards (2,test.transform);

			deck.AddCards (cardsToAdd);

			bool deckHasTwoCards = (deck.DeckCount () == 2);
			Assert.That (deckHasTwoCards);
			GameObject.DestroyImmediate (test);
		}

		[Test]
		public void DrawCards_drawOneCard_deckHasOneLessCard()
		{
			GameObject test = new GameObject ();
			test.name = "DrawCards_drawOneCard_deckHasOneLessCard";
			GameObject obj = Helpers.MakeDeckObject(test.transform);
			DeckActions deck = obj.GetComponent<DeckActions> ();
			GameObject[] cardsToAdd = Helpers.MakeCards (2,test.transform);
			deck.AddCards (cardsToAdd);
			int deckCountBeforeDraw = deck.DeckCount ();

			deck.DrawCards (1);

			int deckCountAfterDraw = deck.DeckCount ();
			bool deckIsOneLess = ((deckCountBeforeDraw - deckCountAfterDraw) == 1);
			Assert.That (deckIsOneLess);
			GameObject.DestroyImmediate (test);
		}

		[Test]
		public void DrawCards_drawTwoCards_deckHasTwoLessCards()
		{
			GameObject test = new GameObject ();
			test.name = "DrawCards_drawTwoCards_deckHasTwoLessCards";
			GameObject obj = Helpers.MakeDeckObject(test.transform);
			DeckActions deck = obj.GetComponent<DeckActions> ();
			GameObject[] cardsToAdd = Helpers.MakeCards (4,test.transform);
			deck.AddCards (cardsToAdd);
			int deckCountBeforeDraw = deck.DeckCount ();
			
			deck.DrawCards (2);
			
			int deckCountAfterDraw = deck.DeckCount ();
			bool deckIsTwoLess = ((deckCountBeforeDraw - deckCountAfterDraw) == 2);
			Assert.That (deckIsTwoLess);
			GameObject.DestroyImmediate (test);
		}

		[Test]
		public void ShuffleDeck_shuffleOrderedDeck_deckIsNotInOrder()
		{
			GameObject test = new GameObject ();
			test.name = "ShuffleDeck_shuffleOrderedDeck_deckIsNotInOrder";
			GameObject obj = Helpers.MakeDeckObject(true,test.transform);
			DebugDeckActions deck = obj.GetComponent<DebugDeckActions> ();
			GameObject[] cardsToAdd = Helpers.MakeCards (4,test.transform);
			deck.AddCards (cardsToAdd);
			GameObject[] cardsInDeckBeforeDraw = deck.GetDeck ();
			//keep track of the order of the cards
			int[] beforeShuffleCardValues = new int[cardsInDeckBeforeDraw.Length];
			for (int i = 0; i < cardsInDeckBeforeDraw.Length; i++) {
				Card card = cardsInDeckBeforeDraw[i].GetComponent<Card>();
				if(null == card)
					Assert.Fail("Gameobject does not contain a Card component");
				beforeShuffleCardValues[i] = card.value;
			}

			deck.Shuffle ();

			GameObject[] cardsInDeckAfterDraw = deck.GetDeck ();
			int[] afterShuffleCardValues = new int[cardsInDeckAfterDraw.Length];
			for (int i = 0; i < cardsInDeckAfterDraw.Length; i++) {
				Card card = cardsInDeckAfterDraw[i].GetComponent<Card>();
				if(null == card)
					Assert.Fail("Gameobject does not contain a Card component");
				afterShuffleCardValues[i] = card.value;
			}
			//check that the arrays don't match
			bool deckOrderHasChanged = !(Helpers.ArraysEqual(beforeShuffleCardValues,afterShuffleCardValues));
			Assert.That (deckOrderHasChanged);
			GameObject.DestroyImmediate (test);
		}

		[Test]
		public void DrawCards_draw4CardsOutOfDeckOf2With2Discards_deckAndDiscardsEmpty()
		{
			GameObject test = new GameObject ();
			test.name = "DrawCards_draw4CardsOutOfDeckOf2With2Discards_deckAndDiscardsEmpty";
			GameObject obj = Helpers.MakeDeckObject(test.transform);
			DeckActions deck = obj.GetComponent<DeckActions> ();
			GameObject[] cardsToAdd = Helpers.MakeCards (2,test.transform);
			deck.AddCards (cardsToAdd);
			GameObject[] cardsToDiscard = Helpers.MakeCards (2,test.transform);
			deck.DiscardCards (cardsToDiscard);

			deck.DrawCards (4);

			int deckCount = deck.DeckCount ();
			int discardCount = deck.DiscardCount ();
			//Debug.Log (deckCount);
			//Debug.Log (discardCount);
			bool decksAreEmpty = (0 == (deckCount + discardCount));
			Assert.That (decksAreEmpty);
			GameObject.DestroyImmediate (test);
		}
	}

	class Helpers
	{
		//Helpers
		static public GameObject[] MakeCards(int count, Transform test)
		{
			GameObject cardOriginal = GameObject.CreatePrimitive (PrimitiveType.Plane);
			cardOriginal.AddComponent<Card> ();

			GameObject[] cardsToAdd = new GameObject[count];
			for (int i=0; i < count; i++) {
				GameObject obj = GameObject.Instantiate(cardOriginal) as GameObject;
				obj.transform.parent = test;
				Card card = obj.GetComponent<Card>();
				card.value = i;
				cardsToAdd [i] = obj;
			}
			GameObject.DestroyImmediate (cardOriginal);
			return cardsToAdd;
		}
		
		static public bool ArraysEqual<T>(T[] a1, T[] a2)
		{
			if (ReferenceEquals(a1,a2))
				return true;
			
			if (a1 == null || a2 == null)
				return false;
			
			if (a1.Length != a2.Length)
				return false;
			
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < a1.Length; i++)
			{
				if (!comparer.Equals(a1[i], a2[i])) return false;
			}
			return true;
		}

		static public GameObject MakeDeckObject(Transform test)
		{
			return MakeDeckObject (false,test);
		}
		
		static public GameObject MakeDeckObject(bool debug, Transform test)
		{
			GameObject obj = new GameObject ();
			obj.transform.parent = test;
			obj.name = "deckObject";
			if(debug)
				obj.AddComponent<DebugDeckActions> ();
			else
				obj.AddComponent<DeckActions> ();

			return obj;
		}
	}
}
