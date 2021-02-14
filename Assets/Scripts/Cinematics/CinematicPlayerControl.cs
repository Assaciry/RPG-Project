using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Controller;
using RPG.SceneManagement;

namespace RPG.Cinematics
{
    public class CinematicPlayerControl : MonoBehaviour
    {
        GameObject player;

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisablePlayerControls;
            GetComponent<PlayableDirector>().stopped += EnablePlayerControls;
        }

        private void DisablePlayerControls(PlayableDirector director)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();

            player.GetComponent<PlayerControl>().enabled = false;
        }

        private void EnablePlayerControls(PlayableDirector director)
        {
            player.GetComponent<PlayerControl>().enabled = true;
        }
    }
}

