using UnityEngine;
using System.Collections;

public class Card : BasicAnimator {

	public int value;

	Vector3 AnimationTarget;
	float AnimateRate;

	// Use this for initialization
	void Start () {
		AnimateRate = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (AnimateRate > 0f)
		{
			float fraction = Time.deltaTime * (1.0f / AnimateRate);
			transform.position = Vector3.Lerp(transform.position,AnimationTarget,fraction);
			//transform.rotation = Quaternion.Slerp(transform.rotation,AnimateToTransform.rotation,fraction);

			if(transform.position == AnimationTarget)
			{
				AnimateRate = 0f;
			}
		}
	}

	override public void AnimateTo(Vector3 target, float rate) {
		AnimationTarget = target;
		AnimateRate = rate;
	}
}
