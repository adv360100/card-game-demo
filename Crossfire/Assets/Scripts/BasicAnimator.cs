using UnityEngine;
using System.Collections;

public class BasicAnimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void AnimateTarget(GameObject target, float rate) {
		Debug.LogWarning ("Please use a derived version of this function");
	}

	virtual public void AnimateTo(Vector3 target, float rate) {
		Debug.LogWarning ("Please use a derived version of this function");
	}
}
