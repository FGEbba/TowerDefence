using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapCreation
{
    public class MapData
    {
        public MapData(TileType type, GameObject prefab)
        {
            tileType = type;
            tilePrefab = prefab;
        }
        public TileType tileType { get; private set; }
        public GameObject tilePrefab { get; private set; }
    }

    public class MapReader : MonoBehaviour


    {
        private static Dictionary<int, int[]> theMap = new Dictionary<int, int[]>();
        public static int BlockSize { get; private set; }
        public static Dictionary<int, int[]> GetMapDictionary()
        {
            return theMap;
        }

        #region Checkers
        public static List<MapData> CheckDataMaps(MapDataExposed[] mapDatas)
        {
            List<MapData> data = new List<MapData>();
            if (mapDatas.Length <= 0) { throw new NullReferenceException("The datamaps are empty! Fill them with info."); }
            else
            {
                foreach (MapDataExposed dataReader in mapDatas)
                {
                    data.Add(new MapData(dataReader.TileType, dataReader.Prefab));
                }
            }
            return data;
        }

        public static List<string> CheckTextAsset(TextAsset mapDoc)
        {
            List<string> fileContent = new List<string>();
            if (mapDoc == null) { throw new NullReferenceException("No file found! Please set a map file"); }
            else
            {
                Files.FileReader.setMapFile(mapDoc);
                fileContent = Files.FileReader.ReadMapFile();
            }
            return fileContent;
        }
        #endregion

        public static void GetArrayAndSpawnMap(List<string> fileContent, List<MapData> data, int m_BlockSize)
        {
            BlockSize = m_BlockSize;
            theMap = Files.FileReader.GetMapDictionary(fileContent);
            SpawnMap(data, Files.FileReader.m_mapWidth);
        }

        private static void SpawnMap(List<MapData> data, int mapWidth)
        {
            int mapLength = theMap.Count;
            for (int iRun = mapLength - 1, i = 0; iRun >= 0; iRun--, i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    GameObject currentTilePrefab = null;
                    TileType currentTileType;
                    TileMethods.TypeById.TryGetValue(theMap[iRun][j], out currentTileType);

                    foreach (MapData mapData in data)
                    {
                        if (mapData.tileType.Equals(currentTileType))
                        {
                            currentTilePrefab = mapData.tilePrefab;
                            SpawnTile(currentTilePrefab, j, i, currentTileType);
                            break;
                        }
                    }
                    if (currentTilePrefab == null)
                    {
                        throw new NullReferenceException($"Unable to find prefab for {currentTileType}. No reference has been created by the user.");
                    }
                }

            }

        }

        private static void SpawnTile(GameObject currentTilePrefab, int coloumn, int row, TileType currentTileType)
        {
            GameObject spawnedTile = Instantiate(currentTilePrefab, new Vector3(coloumn * BlockSize, 0, row * BlockSize), Quaternion.identity);
            spawnedTile.transform.localScale *= 0.5f * BlockSize;
            spawnedTile.transform.parent = GameObject.Find("Map" + FindParent(currentTileType)).transform;

        }

        private static string FindParent(TileType currentTileType)
        {
            if (currentTileType == TileType.Path)
                return "Paths/";

            if (currentTileType == TileType.Obstacle)
                return "Obstacles/";

            if (currentTileType == TileType.TowerOne || currentTileType == TileType.TowerTwo)
                return "TowerUnits/";

            return "";
        }

    }
}
