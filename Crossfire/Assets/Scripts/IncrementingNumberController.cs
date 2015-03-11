using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IncrementingNumberController : MonoBehaviour {

	public Text CurrentValue;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void AddPressed () {
		int val = int.Parse (CurrentValue.text);
		CurrentValue.text = (++val).ToString ();
	}

	public virtual void SubtractPressed () {
		int val = int.Parse (CurrentValue.text);
		CurrentValue.text = (--val).ToString ();
	}
}
