using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup faderCanvas;

        private void Start()
        {
            faderCanvas = GetComponent<CanvasGroup>();
        }

        IEnumerator FadeOutIn(float fadeTime)
        {
            yield return FadeOut(fadeTime);
            yield return FadeIn(fadeTime / 2);
        }

        public IEnumerator FadeOut(float time)
        {
            while(faderCanvas.alpha < 1f)
            {
                faderCanvas.alpha +=  (Time.deltaTime / time);
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (faderCanvas.alpha > 0f)
            {
                faderCanvas.alpha -= (Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
