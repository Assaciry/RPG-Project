using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        IController[] controllers;

        private void Awake()
        {
            GetComponent<Combat.Health>().OnCharacterDeath += ActionScheduler_OnCharacterDeath;
        }

        private void ActionScheduler_OnCharacterDeath(object sender, System.EventArgs e)
        {
            DisableAllController();
        }

        public void DisableAllController()
        {
            CancelCurrentAction();
            controllers = GetComponents<IController>();
            foreach(IController controller in controllers)
            {
                controller.Disable();
            }
        }

        public void EnableAllControllers()
        {
            foreach (IController controller in controllers)
            {
                controller.Enable();
            }
        }

        public void StartAction(IAction action)
        {
            if (action == currentAction) return;

            CancelCurrentAction();
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
        }
    }
}
