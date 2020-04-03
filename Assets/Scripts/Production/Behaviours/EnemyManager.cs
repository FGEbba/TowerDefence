using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{

    public class EnemyManager : MonoBehaviour
    {
        private static List<Enemy> enemies = new List<Enemy>();
        private static Dictionary<int, int> waves = new Dictionary<int, int>();

        private static List<Vector3> accessableTiles = new List<Vector3>();
        private static Vector3 start, end;


        private void Update()
        {

        }

        public static void CheckEnemyPrefabs(GameObject[] prefabList)
        {
            if (prefabList.Length < 1) { throw new NullReferenceException("The prefab map is empty! Fill them with info."); }
        }

        //Funks!
        public static void GetWavesFromFile()
        {
            waves = Files.FileReader.GetWavesFileContent(Files.FileReader.ReadMapFile());
            foreach (KeyValuePair<int, int> currentWave in waves)
            {
                Debug.Log($"Unit 1: {currentWave.Key}, Unit 2: {currentWave.Value}");
            }
        }

        public static void FindPathFromMap()
        {
            Dictionary<int, int[]> theMap = new Dictionary<int, int[]>();
            theMap = MapCreation.MapReader.GetMapDictionary();

            int mapLength = Files.FileReader.m_mapLenght;
            int mapWidth = Files.FileReader.m_mapWidth;

            for (int iRun = mapLength - 1, i = 0; iRun >= 0; iRun--, i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    TileType currentTileType;
                    TileMethods.TypeById.TryGetValue(theMap[iRun][j], out currentTileType);
                    if (TileMethods.IsWalkable(currentTileType))
                    {
                        accessableTiles.Add(new Vector3(j * MapCreation.MapReader.BlockSize, 0, i * MapCreation.MapReader.BlockSize));

                        if (currentTileType == TileType.Start) { start = new Vector3(j * MapCreation.MapReader.BlockSize, 0, i * MapCreation.MapReader.BlockSize); }
                        if (currentTileType == TileType.End) { end = new Vector3(j * MapCreation.MapReader.BlockSize, 0, i * MapCreation.MapReader.BlockSize); }
                    }
                }
            }

        }

        public static void DoTheDijkstra()
        {
            AI.IPathFinder pathFinder = new AI.Dijkstra(accessableTiles);
            IEnumerable<Vector3> path = pathFinder.FindPath(start, end);
        }

        public static List<Vector3> GetAccessableTiles()
        {
            return accessableTiles;
        }


    }

}