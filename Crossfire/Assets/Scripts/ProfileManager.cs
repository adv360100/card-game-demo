using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

	public string DisplayName = "";
	public Button CreateButton;

	public const string kNameKey = "name";

	private const string kResourcesPath = "Assets/Resources/Profile";

	public static bool ProfileExists () {
//		FileInfo info = new FileInfo (kResourcesPath);
		return new FileInfo (kResourcesPath).Exists;
	}

	public void SetDisplayName (string name) {
		DisplayName = name;

		CreateButton.interactable = DisplayName != "";
	}

	public void CreateProfile () {
		string file = "{";

		file += kNameKey + ":" + DisplayName;

		file += "}";

		try {
			StreamWriter writer = new StreamWriter (kResourcesPath, false, System.Text.Encoding.Default);
			using (writer) {
				writer.WriteLine (file);
				writer.Flush ();
				writer.Close ();
			}
		} catch (System.Exception ex) {
			Debug.Log (ex);
		}
	}

	public static JSONNode LoadPlayerInfo () {
		try {
			StreamReader reader = new StreamReader (kResourcesPath, System.Text.Encoding.Default);
			using (reader) {
				JSONNode json = JSON.Parse (reader.ReadToEnd ());
				reader.Close ();

				return json;
			}
		} catch (System.Exception ex) {
			Debug.Log (ex);
		}

		return null;
	}
}
