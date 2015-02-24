using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public int value;

	Transform animateToTransform;
	float animateRate;

	// Use this for initialization
	void Start () {
		animateToTransform = null;
		animateRate = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (animateToTransform != null)
		{
			float fraction = Time.deltaTime * (1.0f / animateRate);
			transform.position = Vector3.Lerp(transform.position,animateToTransform.position,fraction);
			//transform.rotation = Quaternion.Slerp(transform.rotation,animateToTransform.rotation,fraction);

			if(transform.position == animateToTransform.position /*&&
			   transform.rotation == animateToTransform.rotation*/)
			{
				animateToTransform = null;
				animateRate = 0.0f;
			}
		}
	}

	public void AnimateCard(Transform from, Transform to, float rate) {
		animateToTransform = to;
		animateRate = rate;
		transform.position = from.position;
		//transform.rotation = from.rotation;
	}
}
