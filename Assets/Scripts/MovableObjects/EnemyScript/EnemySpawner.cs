using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Stage stage;

    void Start()
    {
        StartCoroutine(DelayStart());
    }
    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.1f);
        stage.SpawnStage(transform);


    }

    // Update is called once per frame
    void Update()
    {

    }

    [Serializable]
    class Stage
    {
        [SerializeField] List<Wave> waves;

        public void SpawnStage(Transform transform)
        {
            foreach (Wave wave in waves)
            {
                wave.SpawnWave(transform);
            }
        }

        [Serializable]
        class Wave
        {
            [SerializeField] float timePerWave;
            [SerializeField] List<Crowd> crowds;

            public void SpawnWave(Transform transform)
            {
                foreach (var crowd in crowds) { crowd.SpawnCrowd(transform); }
            }

            [Serializable]
            class Crowd
            {
                [SerializeField] string CrowdName;
                [SerializeField] int count;
                [SerializeField] float timePerCrowd;
                [SerializeField] GameObject enemyPrefab;

                public void SpawnCrowd(Transform transform)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var unit = GameObject.Instantiate(enemyPrefab, transform);
                        
                    }
                       
                }
            }
        }
    }




}
