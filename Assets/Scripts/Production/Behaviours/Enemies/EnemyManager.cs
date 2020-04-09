using System;
using System.Collections.Generic;
using UnityEngine;
using Tools;


namespace Enemies
{

    public class EnemyManager
    {
        public static bool waveComplete { get; private set; }

        private static Dictionary<int, KeyValuePair<int, int>> waves = new Dictionary<int, KeyValuePair<int, int>>();

        private static List<Vector2Int> accessableTiles = new List<Vector2Int>();
        private static Vector2Int start, end;
        private static List<Vector2Int> path;

        private static List<GameObject> activeUnits = new List<GameObject>();

        private static GameObjectPool smallEnemyPool;
        private static GameObjectPool bigEnemyPool;
        private static int smallEnemiesInCurrentWave = 0, bigEnemiesInCurrentWave = 0, disabledUnits = 0, currentWave = 0;
        private static int blockSize = 0;


        #region DijkstraFucks
        public static void GetPath()
        {
            blockSize = MapCreation.MapCreator.BlockSize;
            accessableTiles = MapCreation.MapManager.AccessableTilesConverter(MapCreation.MapManager.GetPathVec3());
            start = MapCreation.MapManager.ConvertVec3PosToVec2int(MapCreation.MapCreator.start);
            end = MapCreation.MapManager.ConvertVec3PosToVec2int(MapCreation.MapCreator.end);
        }
        public static void DoTheDijkstra()
        {
            AI.IPathFinder pathFinder = new AI.Dijkstra(accessableTiles);
            path = (List<Vector2Int>)pathFinder.FindPath(start, end);
        }
        #endregion

        public static void SetPools(GameObject smallEnemyPrefab, GameObject bigEnemyPrefab)
        {
            if (smallEnemyPrefab != null) { smallEnemyPool = new GameObjectPool(1, smallEnemyPrefab); }
            else { throw new NullReferenceException($"There's no prefab set!"); }
            if (bigEnemyPrefab != null) { bigEnemyPool = new GameObjectPool(1, bigEnemyPrefab); }
            else { throw new NullReferenceException($"There's no prefab set!"); }
        }

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

        public static void WaveChecker()
        {
            if (waveComplete)
            {
                if (disabledUnits == (smallEnemiesInCurrentWave + bigEnemiesInCurrentWave))
                {
                    //Reset everything
                    smallEnemiesInCurrentWave = 0;
                    bigEnemiesInCurrentWave = 0;
                    disabledUnits = 0;
                    waveComplete = false;
                    currentWave++;
                }
            }
            else
            {
                SpawnWave();
            }


        }

        public static void SpawnWave()
        {
            if (waves.ContainsKey(currentWave))
            {
                if (smallEnemiesInCurrentWave < waves[currentWave].Key)
                {
                    SpawnUnit(smallEnemyPool);
                    smallEnemiesInCurrentWave++;
                    Debug.Log($"smalls: {smallEnemiesInCurrentWave}/{waves[currentWave].Key}");
                }
                if (bigEnemiesInCurrentWave < waves[currentWave].Value)
                {
                    SpawnUnit(bigEnemyPool);
                    bigEnemiesInCurrentWave++;
                    Debug.Log($"Bigs: {bigEnemiesInCurrentWave}/{waves[currentWave].Value}");
                }

                //Just to make sure everything spawns correctly.
                else if ((smallEnemiesInCurrentWave + bigEnemiesInCurrentWave) == (waves[currentWave].Key + waves[currentWave].Value))
                {
                    waveComplete = true;
                    Debug.Log($"CurrentWave: {currentWave}, SmallUnits: {waves[currentWave].Key}, BigUnits: {waves[currentWave].Value}");

                }

            }
        }

        private static void SpawnUnit(GameObjectPool pool)
        {
            GameObject enemy = pool.Rent(false);
            EnemyController enemyComponent = enemy.GetComponent<EnemyController>();

            enemy.transform.position = Vector3.zero;
            enemy.SetActive(true);
            EmitOnDisable emitOnDisable = enemy.GetComponent<EmitOnDisable>();
            emitOnDisable.OnDisableGameObject += EnemyDisabled;

            enemyComponent.ResetUnit(path, start, blockSize);

            activeUnits.Add(enemy);
        }

        private static void EnemyDisabled(GameObject enemy)
        {
            disabledUnits++;
            enemy.GetComponent<EmitOnDisable>().OnDisableGameObject -= EnemyDisabled;
            activeUnits.Remove(enemy);
        }

    }

}