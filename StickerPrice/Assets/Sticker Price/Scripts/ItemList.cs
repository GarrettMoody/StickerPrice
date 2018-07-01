using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour {

	private ScrollRect scrollRect;
	private RectTransform viewport;
	private RectTransform scrollPanel;
	// Use this for initialization
	void Start () {
		scrollRect = this.GetComponent<ScrollRect> ();
		viewport = (RectTransform)this.transform.Find ("Viewport");
		scrollPanel = (RectTransform)viewport.transform.Find ("ContentPanel");
	}

	public void OnValueChange() {
		float verticalVelocity = Mathf.Abs (scrollRect.velocity.y);
		//Debug.Log (verticalVelocity);
//		if (verticalVelocity > 50f) {
//			isScrolling = true;
//		} else {
//			isScrolling = false;
//		}
//
		if (verticalVelocity < 2f) {
			//isMoving = false;
			scrollRect.velocity = Vector2.zero;
		} else {
			//isMoving = true;
		}

//		if (scrollRect.transform.localPosition.y < 0) {
//			float newY = Mathf.Lerp (scrollPanel.transform.localPosition.y,
//				             0,
//				             Time.deltaTime * 5f);
//
//			Vector2 newPosition = new Vector2 (scrollPanel.transform.localPosition.x, newY);
//			scrollPanel.transform.localPosition = newPosition;
//		}

	}
}
