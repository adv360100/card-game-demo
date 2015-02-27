using UnityEngine;
using System;
using System.Collections;

public class BasicAnimator : MonoBehaviour {

	private Func<float, int> UpdateFunction = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected void Update () {
		if (UpdateFunction != null) {
			UpdateFunction(Time.deltaTime);
		}
	}

	public void QuadraticOutMoveTo (Vector3 originalPosition, Vector3 targetPosition, float animationDuration, Func<int, int> completionCallback) {
		if (UpdateFunction != null) {
			return;
		}

		float currentTime = 0;
		UpdateFunction = deltaTime => {
			currentTime += deltaTime;
			float fraction = currentTime / animationDuration;

			transform.position = CalclulateQuadraticOut (originalPosition, targetPosition, fraction);

			if (Vector3.Distance(transform.position, targetPosition) <= 0.01f) {
				transform.position = targetPosition;
				if (completionCallback != null) {
					completionCallback(1);
				}
				UpdateFunction = null;
			}

			return 0;
		};
	}
	
	private Vector3 CalclulateQuadraticOut (Vector3 start, Vector3 end, float delta) {
		return new Vector3 (CalclulateQuadraticOut(start.x, end.x, delta), CalclulateQuadraticOut(start.y, end.y, delta), CalclulateQuadraticOut(start.z, end.z, delta));
	}

	private float CalclulateQuadraticOut (float start, float end, float delta) {
		return -(end - start) * delta * (delta - 2) + start;
	}
}
