using UnityEngine;
using System;
using System.Collections;

public class BasicAnimator : MonoBehaviour {

	public delegate void AnimationCompletionCallback();
	private delegate void UpdateFunction(float deltaTime);
	private UpdateFunction MyUpdateFunction = null;


	float doubleClickStart = 0;
	
	void OnMouseUp()
	{
		if ((Time.time - doubleClickStart) < 0.3f)
		{
			this.OnDoubleClick();
			doubleClickStart = -1;
		}
		else
		{
			doubleClickStart = Time.time;
		}
	}
	
	virtual protected void OnDoubleClick()
	{
		Debug.Log("Double Clicked!");
	}

	// Update is called once per frame
	protected void Update () {



		if (MyUpdateFunction != null) {
			MyUpdateFunction(Time.deltaTime);
		}
	}

	public void QuadraticOutMoveTo (Vector3 originalPosition, Vector3 targetPosition, float animationDuration, AnimationCompletionCallback completionCallback) {
//		if (MyUpdateFunction != null) {
//			return;
//		}

		transform.position = originalPosition;
		float currentTime = 0;
		MyUpdateFunction = deltaTime => {
			currentTime += deltaTime;
			float fraction = currentTime / animationDuration;

			transform.position = CalclulateQuadraticOut (originalPosition, targetPosition, fraction);

			if (Vector3.Distance(transform.position, targetPosition) <= 0.01f) {
				transform.position = targetPosition;
				if (completionCallback != null) {
					completionCallback();
				}
				MyUpdateFunction = null;
			}
		};
	}
	
	private Vector3 CalclulateQuadraticOut (Vector3 start, Vector3 end, float delta) {
		return new Vector3 (CalclulateQuadraticOut(start.x, end.x, delta), CalclulateQuadraticOut(start.y, end.y, delta), CalclulateQuadraticOut(start.z, end.z, delta));
	}

	private float CalclulateQuadraticOut (float start, float end, float delta) {
		return -(end - start) * delta * (delta - 2) + start;
	}
}
