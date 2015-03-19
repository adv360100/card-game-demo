using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JSONImporter : MonoBehaviour {

	private const string kResourcesPath = "Assets/Resources/";

	static public List<GameObject> LoadAllFromFolder (string path) {
		DirectoryInfo info = new DirectoryInfo (kResourcesPath + path);
		FileInfo[] fileInfo = info.GetFiles ();
		List<GameObject> cards = new List<GameObject> (28);

		foreach (FileInfo file in fileInfo) {
			string[] extensions = file.Name.Split ('.');
			if (extensions[extensions.Length - 1] == "cards") {
				JSONNode json = Load (kResourcesPath + path + "/" + file.Name);
				if (json != null) {
					uint id = 1;
//					Debug.Log ("json.Count: " + json.Count);
//					Debug.Log ("json[i]['quantity']: " + json[0]["quantity"]);
//					Debug.Log ("json[i]['name']: " + json[0]["name"]);
//					Debug.Log ("json[i]['texture']: " + json[0]["texture"]);
//					Debug.Log ("texture: " + extensions[0] + "/" + json[0]["texture"]);
					for (int i = 0; i < json.Count; i++) {
						for (int j = 0; j < json[i]["quantity"].AsInt; j++, id++) {
							GameObject card = new GameObject ();
							card.AddComponent<Card> ();
							card.GetComponent<Card> ().ID = id;
							card.GetComponent<Card> ().name = json[i]["name"];
							card.GetComponent<Card> ().FrontTexture = Resources.Load (path + "/" + extensions[0] + "/" + json[i]["texture"]) as Texture;
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