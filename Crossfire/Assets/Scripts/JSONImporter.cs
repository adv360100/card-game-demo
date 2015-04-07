using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public enum CardIDs { CardIDPlayerCards = 1, CardIDObstacles = 10000, CardIDHardObstacles = 20000, CardIDCrossfire = 30000 }; 

public class JSONImporter : MonoBehaviour {

	private const string kResourcesPath = "Resources";

	static string ThePath () {
		return Application.persistentDataPath + Path.AltDirectorySeparatorChar + kResourcesPath + Path.AltDirectorySeparatorChar;
	}

	static public List<GameObject> LoadAllFromFolder (string path, uint startingID) {
		DirectoryInfo info = new DirectoryInfo (ThePath() + path);
		FileInfo[] fileInfo = info.GetFiles ();
		List<GameObject> cards = new List<GameObject> (28);

		foreach (FileInfo file in fileInfo) {
			string[] extensions = file.Name.Split ('.');
			if (extensions[extensions.Length - 1] == "cards") {
				JSONNode json = Load (ThePath() + path + Path.AltDirectorySeparatorChar + file.Name);
				if (json != null) {
					uint id = startingID;
//					Debug.Log ("json.Count: " + json.Count);
//					Debug.Log ("json[i]['quantity']: " + json[0]["quantity"]);
//					Debug.Log ("json[i]['name']: " + json[0]["name"]);
//					Debug.Log ("json[i]['texture']: " + json[0]["texture"]);
//					Debug.Log ("texture: " + extensions[0] + "/" + json[0]["texture"]);
					WWW textureLoader;
					for (int i = 0; i < json.Count; i++) {
						textureLoader = new WWW("file:///" + ThePath() + path + Path.AltDirectorySeparatorChar + extensions[0] + Path.AltDirectorySeparatorChar + json[i]["texture"] + ".png");
						for (int j = 0; j < json[i]["quantity"].AsInt; j++, id++) {
							GameObject card = new GameObject ();
							card.AddComponent<Card> ();
							card.GetComponent<Card> ().ID = id;
							card.GetComponent<Card> ().name = json[i]["name"];
							card.GetComponent<Card> ().FrontTexture = textureLoader.texture;
//							GameManager.Instance.StartCoroutine (LoadTexture (card.GetComponent<Card> (), "file:///" + ThePath() + path + Path.AltDirectorySeparatorChar + extensions[0] + Path.AltDirectorySeparatorChar + json[i]["texture"] + ".png"));
//							Debug.Log ("count: " + j);
//							Debug.Log (card.GetComponent<Card> ().FrontTexture);
							cards.Add (card);
						}
					}
				}
			}
		}

		return cards;
	}

	static IEnumerator LoadTexture(Card card, string path) {
		WWW textureLoader = new WWW(path);
		yield return textureLoader;
		
		if (textureLoader.error == null) {
			card.GetComponent<Renderer> ().material.SetTexture (0, textureLoader.texture);
			card.FrontTexture = textureLoader.texture;
		}    
	}

	static public JSONNode Load (string fileName) {
		// Handle any problems that might arise when reading the text
		try {
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader (fileName, Encoding.Default);
			
			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using (theReader) {
				JSONNode json = JSON.Parse (theReader.ReadToEnd ());

				// Done reading, close the reader and return true to broadcast success    
				theReader.Close ();
				return json;
			}
		}
		
		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (Exception e)
		{
			Debug.Log (e.Message);
			return null;
		}
	}
}