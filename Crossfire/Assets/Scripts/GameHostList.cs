using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHostList : MonoBehaviour {

	public GameObject OriginalGamePanel;
	public Button JoinBtn;

	private int Index = 0;
	private HostData[] HostList;
	private GamePanel SelectedGame = null;

	public HostData[] GetHosts ()
	{
		return HostList;
	}

	public void AddHostGames(HostData[] games)
	{
		HostList = games;
		ClearList ();
		foreach (HostData host in games) {
			AddHostGame(host);
		}
	}

	public void AddHostGame(HostData host)
	{
		Index++;
		GameObject theObject = Instantiate (OriginalGamePanel) as GameObject;
		GamePanel panel = theObject.GetComponent<GamePanel> ();
		panel.UpdatePanel (Index, host);
		theObject.name = host.gameName;
		theObject.transform.SetParent (transform);
		theObject.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	public void ClearList()
	{
		GamePanel[] games = GetComponentsInChildren<GamePanel> ();
		foreach (GamePanel comp in games) {
			Destroy(comp.gameObject);
		}

		Index = 0;
		OnGameSelected (null);
	}

	void OnGameSelected(GamePanel obj)
	{
		SelectedGame = obj;
		JoinBtn.interactable = (obj != null) ? true : false;
	}

	public void JoinPressed()
	{
		NetworkManager.ConnectToServer (SelectedGame.Host,"");
	}
}
