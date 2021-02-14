using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistenceObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistenceObjectPrefab;

        static bool hasSpawned;

        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistenceObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
