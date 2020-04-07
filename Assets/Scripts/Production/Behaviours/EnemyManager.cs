using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{

    public class EnemyManager : MonoBehaviour
    {
        public static bool waveEnd { get; private set; }

        private static Dictionary<int, KeyValuePair<int, int>> waves = new Dictionary<int, KeyValuePair<int, int>>();

        private static List<Vector2Int> accessableTiles = new List<Vector2Int>();
        private static Vector2Int start, end;
        private static List<Vector2Int> path;

        public static void GetWaves()
        {
            Dictionary<int, int> allWaves = MapCreation.MapManager.GetWavesFromFile();

            int i = 0;
            foreach (KeyValuePair<int, int> wave in allWaves)
            {
                waves.Add(i, wave);
                i++;
            }
        }
        public static void GetPath()
        {
            accessableTiles = MapCreation.MapManager.AccessableTilesConverter(MapCreation.MapManager.GetPathVec3());
            start = MapCreation.MapManager.ConvertVec3PosToVec2int(MapCreation.MapCreator.start);
            end = MapCreation.MapManager.ConvertVec3PosToVec2int(MapCreation.MapCreator.end);
        }

        public static void SpawnWave(Tools.GameObjectPool smallPool, Tools.GameObjectPool bigPool, int waveNumber)
        {
            if (waveNumber > waves.Count - 1)
            {
                waveEnd = true;
            }
            foreach (KeyValuePair<int, KeyValuePair<int, int>> wave in waves)
            {
                if (wave.Key == waveNumber)
                {
                    for (int i = 0; i < wave.Value.Key; i++)
                    {
                        SpawnUnits(smallPool);
                    }
                    for (int i = 0; i < wave.Value.Value; i++)
                    {
                        SpawnUnits(bigPool);
                    }
                    break;
                }
            }

        }

        public static void DoTheDijkstra()
        {
            AI.IPathFinder pathFinder = new AI.Dijkstra(accessableTiles);
            path = (List<Vector2Int>)pathFinder.FindPath(start, end);
        }

        public static void SpawnUnits(Tools.GameObjectPool pool)
        {
            GameObject enemy = pool.Rent(false);
            EnemyController enemyComponent = enemy.GetComponent<EnemyController>();

            enemy.transform.position = Vector3.zero;

            enemy.SetActive(true);
            enemyComponent.ResetUnit(path, start);


        }

    }

}