using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {

	public List<BasicArea> TargetAreas;

	private int CurrentAreaIndex = 0;
	private Camera MainCamera;
	private float OriginalOrthographicSize;
	private Transform ZoomedObject;
	private Vector3 ZoomedObjectOldPos;
	private IEnumerator coroutine;
	private float ZoomSpeed = 1f;

	// Use this for initialization
	void Start () {
		OriginalOrthographicSize = camera.orthographicSize;
	}

	public void Update()
	{
		if (ZoomedObject != null && ZoomedObject.position != ZoomedObjectOldPos) 
		{
			ZoomOnObject(null);
		}
	}

	public void MoveToArea(int targetIndex)
	{
		//check if valid
		if (targetIndex > TargetAreas.Count)
			return;
		BasicArea b;
		if (targetIndex == CurrentAreaIndex)
		{
			b = TargetAreas [CurrentAreaIndex];
			//inform current area
			b.OnFocusExit ();
		}
		//grab the position
		b = TargetAreas [targetIndex];
		transform.position = new Vector3(b.transform.position.x,b.transform.position.y, transform.position.z);
		CurrentAreaIndex = targetIndex;
		//inform area
		b.OnFocusEnter ();
	}

	public void ZoomOnObject(Renderer target)
	{
		if (target == null || target.gameObject.transform == ZoomedObject)
		{
			ZoomedObject = null;
			//unzoom
			StopMyCoroutine ();
			coroutine = ChangeCameraSize (OriginalOrthographicSize);
			StartCoroutine (coroutine);
			MoveToArea(CurrentAreaIndex);
			return;
		}
		ZoomedObject = target.transform;
		ZoomedObjectOldPos = ZoomedObject.position;

		//move camera over the object
		transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z);

		float w = target.bounds.size.x;
		float h = target.bounds.size.y;
	
		//adjust camera size
		StopMyCoroutine ();
		coroutine = ChangeCameraSize (Mathf.Max (w, h) * .5f);
		StartCoroutine (coroutine);
	}

	IEnumerator ChangeCameraSize(float size) {
		float start = camera.orthographicSize;
		float diff = size - start;

		while (Mathf.Abs(camera.orthographicSize - size) > .2f) {
			start = camera.orthographicSize;
			camera.orthographicSize = diff * Time.deltaTime / ZoomSpeed + start;
			yield return null;
		}

		camera.orthographicSize = size;
		StopMyCoroutine ();

	}

	void StopMyCoroutine()
	{
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}
	}

	public void MoveToMyPlayer () {
		for (int i = 0; i < GameManager.Instance.NumOfPlayers; i++) {
			if (GameManager.Instance.Players[i] == GameManager.Instance.MyPlayer) {
				CurrentAreaIndex = i;
				break;
			}
		}
		MoveToArea (CurrentAreaIndex);
	}
}
