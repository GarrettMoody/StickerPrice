using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceModeButton : MonoBehaviour
{
	private Toggle toggle;
	private Image image;
	private AdjustPanel adjustPanel;

	private Color selectedColor = new Color32 (0x3F, 0xAE, 0x2A, 0xFF);
	private Color unselectedColor = new Color32 (0x90, 0x9C, 0x9C, 0xFF);

	// Use this for initialization
	void Start ()
	{
		adjustPanel = this.GetComponentInParent <AdjustPanel> ();
		toggle = this.GetComponent <Toggle> ();
		image = this.GetComponentInChildren <Image> ();
		toggle.onValueChanged.AddListener (delegate {
			ToggleChangeListener ();
		});
		ToggleChangeListener ();
	}

	public void ToggleChangeListener ()
	{
		ColorBlock cb = toggle.colors;
		if (toggle.isOn) {
			cb.normalColor = selectedColor;
			cb.highlightedColor = selectedColor;
		} else {
			cb.normalColor = unselectedColor;
			cb.highlightedColor = unselectedColor;
		}
		toggle.colors = cb;
	}
}
