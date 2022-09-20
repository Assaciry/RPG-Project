using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup faderCanvas;
        private Coroutine currentFaderRoutine;

        private void Awake()
        {
            faderCanvas = GetComponent<CanvasGroup>();
        }

        public void FadeOutTo1()
        {
            faderCanvas.alpha = 1f;
        }

        public IEnumerator FadeOut(float time)
        {
            // Cancel running coroutine
            // Run fadeout coroutine

            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currentFaderRoutine != null)
                StopCoroutine(currentFaderRoutine);

            currentFaderRoutine = StartCoroutine(FadeRoutine(target, time));
            yield return currentFaderRoutine;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(faderCanvas.alpha, target))
            {
                faderCanvas.alpha = Mathf.MoveTowards(faderCanvas.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
