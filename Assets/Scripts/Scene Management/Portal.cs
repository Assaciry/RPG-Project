using RPG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.ScratchSaving;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad;
        [SerializeField] private Transform spawnPoint;

        public float fadeOutTime = 0.5f;
        public float fadeInTime = 0.5f;


        public void TeleportEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                other.GetComponent<ActionScheduler>().CancelCurrentAction();
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            GameObject.FindWithTag("Player").GetComponent<ActionScheduler>().DisableAllController();

            //Fade out effect
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            // Saving scene state
            //SavingWrapper saving = FindObjectOfType<SavingWrapper>();
            //saving.Save();
            ScratchSavingWrapper scratchSaving = FindObjectOfType<ScratchSavingWrapper>();
            scratchSaving.Save();

            //Loading next scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            ActionScheduler actionScheduler = GameObject.FindWithTag("Player").GetComponent<ActionScheduler>();
            actionScheduler.DisableAllController();

            //Load the last save file of that scene
            //saving.Load();
            scratchSaving.Load();


            // Spawning player on portal spawn point
            Portal portalToTeleport = GetOtherPortal();
            UpdatePlayer(portalToTeleport);

            // Checkpointing
            //saving.Save();
            scratchSaving.Save();

            // Wait for player to be updated
            yield return new WaitForSeconds(0.5f);

            yield return fader.FadeIn(fadeInTime);

            // Enable back the controls
            actionScheduler.EnableAllControllers();

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal portalToTeleport)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = portalToTeleport.transform.Find("SpawnPoint").position;
            Debug.Log(portalToTeleport.transform.localEulerAngles);
            player.GetComponent<NavMeshAgent>().enabled = true;
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
