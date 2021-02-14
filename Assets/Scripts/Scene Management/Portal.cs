using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Core;
using RPG.Controller;

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

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal portalToTeleport = GetOtherPortal();
            UpdatePlayer(portalToTeleport);

            yield return new WaitForSeconds(fadeOutTime);

            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal portalToTeleport)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = portalToTeleport.spawnPoint.position;
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
