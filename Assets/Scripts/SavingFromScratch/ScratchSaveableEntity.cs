using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace RPG.ScratchSaving
{
    [ExecuteAlways]
    public class ScratchSaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, ScratchSaveableEntity> globalLookUp = new Dictionary<string, ScratchSaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            SetUniqueIdentifier();
        }
#endif

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach(IScratchSaveable saveable in GetComponents<IScratchSaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (IScratchSaveable saveable in GetComponents<IScratchSaveable>())
            {
                string typeString = saveable.GetType().ToString();

                if(stateDict.ContainsKey(typeString))
                    saveable.RestoreState(stateDict[typeString]);
            }
        }

        private void SetUniqueIdentifier()
        {
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookUp[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            //return globalLookUp.ContainsKey(candidate) ? globalLookUp[candidate] == this : true;

            if (!globalLookUp.ContainsKey(candidate)) return true;
            if (globalLookUp[candidate] == this) return true;
            if (globalLookUp[candidate] == null)
            {
                globalLookUp.Remove(candidate);
                return true;
            }
            if(globalLookUp[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookUp.Remove(candidate);
                return true;
            }
            return false;
        }
    }
}
