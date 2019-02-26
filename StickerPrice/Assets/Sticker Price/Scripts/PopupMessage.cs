using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour {

    public CanvasGroup canvasGroup;
    [Tooltip("How fast the message fades in and out.")]
    public float FADE_DURATION = .1f;
    [Tooltip("The alpha value of the error message when it is displayed.")]
    public float OPACITY = .8f;
    public Button PopupOverlayButton;
    private Coroutine runningCoroutine;

    public void DisplayPopup(int numberOfSeconds) {
        StopPopup();
        runningCoroutine = StartCoroutine(RunPopup(numberOfSeconds));
    }

    public void OnPopupClick() {
        StopPopup();
        StartCoroutine(FadeOut());
    }

    public void ClosePopup(bool immediately = false)
    {
        if(immediately)
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            FadeOut();
        }
    }

    public void StopPopup() {
        if(runningCoroutine != null) {
            StopCoroutine(runningCoroutine);
        }
    }

    IEnumerator RunPopup(int numberOfSeconds) {
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(numberOfSeconds);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn() {
        PopupOverlayButton.GetComponent<Image>().raycastTarget = true;
        for (float i = 0f; i <= OPACITY; i += .05f)
        {
            canvasGroup.alpha = i;

            yield return new WaitForSeconds(FADE_DURATION);
        }

        //Since the opacity is only increased by increments of .05, this makes the opacity set to exactly the desired setting
        canvasGroup.alpha = OPACITY;
    }

    IEnumerator FadeOut() {
        for (float i = canvasGroup.alpha; i >= 0f; i -= .05f)
        {
            canvasGroup.alpha = i;

            yield return new WaitForSeconds(FADE_DURATION);
        }

        //Since the opacity is only increased by increments of .05, this makes the opacity set to exactly 0
        canvasGroup.alpha = 0;
        PopupOverlayButton.GetComponent<Image>().raycastTarget = false;
    }
}
