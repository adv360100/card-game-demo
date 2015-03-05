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

	// Use this for initialization
	void Start () {
		OriginalOrthographicSize = camera.orthographicSize;
		MoveToArea (CurrentAreaIndex);
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
		if (target == null)
		{
			ZoomedObject = null;
			//unzoom
			camera.orthographicSize = OriginalOrthographicSize;
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
		camera.orthographicSize = Mathf.Max (w, h) * .5f;
	}
}
