using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanel : Button {

	public Text NumberText;
	public Text GameNameText;
	public Text PasswordText;
	[HideInInspector]public HostData Host;

	public void UpdatePanel(int index, HostData hostInfo)
	{
		NumberText.text = index.ToString ();
		GameNameText.text = hostInfo.gameName;
		PasswordText.text = (hostInfo.passwordProtected) ? "Yes" : "No";
		Host = hostInfo;
	}

	public void SelectGame()
	{
		foreach (Selectable item in Selectable.allSelectables) {
			item.targetGraphic.color = item.colors.normalColor;
		}
		targetGraphic.color = colors.pressedColor;
		SendMessageUpwards ("OnGameSelected", this);
	}
}
