using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRow : MonoBehaviour {


	private Vector2 startPositionX;
	// Use this for initialization
	void Start () {
		
	}


	public void OnPointerDown() {
		startPositionX = Input.mousePosition.x;
	}

	public void OnPointerUp() {
		
	}

	public void OnMouseDrag() {
		float dragDistanceX = startPositionX - Input.mousePosition.x;
		this.GetComponent<RectTransform> ().position += dragDistanceX;
	}
}
