using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ContentGrid : MonoBehaviour {

	class SetInfo {
		public string Name;
		public bool HasBlackMarket = false;
		public bool HasObstacle = false;
		public bool HasHardObstacle = false;
		public bool HasCrossfire = false;
	}

	private const string kResourcesPath = "Assets/Resources/";
	private List<SetInfo> SetList = new List<SetInfo>();
	private enum DeckTypes {DeckTypePlayerCards, DeckTypeObstacles, DeckTypeHardObstacles, DeckTypeCrossfire};
	// Use this for initialization
	void Start () {
		LoadSetInfo (DeckTypes.DeckTypePlayerCards);
		LoadSetInfo (DeckTypes.DeckTypeObstacles);
		LoadSetInfo (DeckTypes.DeckTypeHardObstacles);
		LoadSetInfo (DeckTypes.DeckTypeCrossfire);
	}

	void LoadSetInfo(DeckTypes type)
	{
		string folder = type.ToString ().Replace ("DectType", "");
		DirectoryInfo info = new DirectoryInfo (Application.dataPath + folder);
		DirectoryInfo[] directories = info.GetDirectories ();
		
		foreach (DirectoryInfo d in directories) {
			SetInfo si = SetList.Find(delegate(SetInfo obj) {
				return d.Name.Equals(obj.Name,System.StringComparison.Ordinal);
			});

			
			if(si == null)
			{
				si = new SetInfo();
				si.Name = d.Name;
				SetList.Add(si);
			}
			
			//mark if has deck
			switch(type)
			{
			case DeckTypes.DeckTypeCrossfire:
				si.HasCrossfire = true;
				break;
			case DeckTypes.DeckTypeHardObstacles:
				si.HasHardObstacle = true;
				break;
			case DeckTypes.DeckTypeObstacles:
				si.HasObstacle = true;
				break;
			case DeckTypes.DeckTypePlayerCards:
				si.HasBlackMarket = true;
				break;
			default:
				return;
			}
		}
	}

}
