using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour {


    public Image background;
    public Text errorText;
    [Tooltip("How fast the message fades in and out.")]
    public float FADE_DURATION = .1f;
    [Tooltip("The alpha value of the error message when it is displayed.")]
    public float OPACITY = .8f;
    public Button ErrorOverlayButton;
    private Coroutine runningCoroutine;

    public void DisplayError(int numberOfSeconds) {
        StopError();
        runningCoroutine = StartCoroutine(RunError(numberOfSeconds));
    }

    public void OnErrorClick() {
        StopError();
        StartCoroutine(FadeOut());
    }

    public void StopError() {
        if(runningCoroutine != null) {
            StopCoroutine(runningCoroutine);
        }
    }

    IEnumerator RunError(int numberOfSeconds) {
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(numberOfSeconds);
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeIn() {
        ErrorOverlayButton.GetComponent<Image>().raycastTarget = true;
        for (float i = 0f; i < OPACITY; i += .05f)
        {
            Color c = background.color;
            c.a = i;
            background.color = c;

            Color tc = errorText.color;
            tc.a = i;
            errorText.color = tc;
            yield return new WaitForSeconds(FADE_DURATION);
        }
    }

    IEnumerator FadeOut() {
        for (float i = background.color.a; i > 0f; i -= .05f)
        {
            Color c = background.color;
            c.a = i;
            background.color = c;

            Color tc = errorText.color;
            tc.a = i;
            errorText.color = tc;
            yield return new WaitForSeconds(FADE_DURATION);
        }
        ErrorOverlayButton.GetComponent<Image>().raycastTarget = false;
    }
}
