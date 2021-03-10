using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Core;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad;
        [SerializeField] private Transform spawnPoint;

        public float fadeOutTime = .5f;
        public float fadeInTime = 1.5f;

        public void TeleportEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                FindObjectOfType<ActionScheduler>().CancelCurrentAction();
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            //Fade out effect
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            // Saving scene state
            SavingWrapper saving = FindObjectOfType<SavingWrapper>();
            saving.Save();

            //Loading next scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //saving.Load();

            // Spawning player on portal spawn point
            Portal portalToTeleport = GetOtherPortal();
            UpdatePlayer(portalToTeleport);

            //Fade in effect
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal portalToTeleport)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = portalToTeleport.transform.Find("SpawnPoint").position;
            player.transform.rotation = portalToTeleport.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach(var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.name == "Enter Portal") return portal;
            }
            return null;
        }
    }
}
