using RPG.Core;
using RPG.SceneManagement;
using System.Collections;
using UnityEngine;

namespace RPG.ScratchSaving
{
    public class ScratchSavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        const float fadeInTime = 0.5f;

        private ScratchSavingSystem savingSystem;
        private Fader fader;
        private ActionScheduler scheduler;

        private void Awake() {
            savingSystem = GetComponent<ScratchSavingSystem>();
            fader = FindObjectOfType<Fader>();
            scheduler = GameObject.FindWithTag("Player").GetComponent<ActionScheduler>();
        }

        private IEnumerator Start()
        {
            fader.FadeOutTo1();
            yield return new WaitForSeconds(0.5f);

            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }
    }
}