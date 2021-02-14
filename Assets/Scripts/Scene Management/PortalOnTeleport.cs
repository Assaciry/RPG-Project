using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class PortalOnTeleport : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponentInParent<Portal>().TeleportEnter(other);
        }
    }
}
