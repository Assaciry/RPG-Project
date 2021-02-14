using System;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Controller;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isAlreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if(!isAlreadyTriggered && other.TryGetComponent(out PlayerControl player))
            {
                GetComponent<PlayableDirector>().Play();

                isAlreadyTriggered = true;
            }
        }

    }
}
