using UnityEngine;
using System.Collections;

public class Card : BasicAnimator {

	public int value;

	Transform AnimationTarget;
	float AnimateRate;

	// Use this for initialization
	void Start () {
		AnimationTarget = null;
		AnimateRate = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (AnimationTarget != null)
		{
			float fraction = Time.deltaTime * (1.0f / AnimateRate);
			transform.position = Vector3.Lerp(transform.position,AnimationTarget.position,fraction);
			//transform.rotation = Quaternion.Slerp(transform.rotation,AnimateToTransform.rotation,fraction);

			if(transform.position == AnimationTarget.position)
			{
				AnimationTarget = null;
				AnimateRate = 0.0f;
			}
		}
	}

	override public void AnimateTo(Transform target, float rate) {
		AnimationTarget = target;
		AnimateRate = rate;
	}
}
