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

    public class MapCreator : MonoBehaviour
    {

        private static List<Vector3> accessableTiles = new List<Vector3>();
        public static Vector3 start { get; private set; }
        public static Vector3 end { get; private set; }

        private static Dictionary<int, int[]> theMap = new Dictionary<int, int[]>();
        public static int BlockSize { get; private set; }
        public static Dictionary<int, int[]> GetMapDictionary() { return theMap; }
        public static List<Vector3> GetAccessableTiles() { return accessableTiles; }
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

        private static void CreateEnemyPath(float x, float z)
        {
            accessableTiles.Add(new Vector3(x * BlockSize, 0, z * BlockSize));
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
                            if (TileMethods.IsWalkable(currentTileType))
                            {
                                CreateEnemyPath(j, i);
                                if (currentTileType == TileType.Start) { start = new Vector3(j * BlockSize, 0, i * BlockSize); }
                                if (currentTileType == TileType.End) { end = new Vector3(j * BlockSize, 0, i * BlockSize); }

                            }
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
            GameObject spawnedTile = Instantiate(currentTilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            spawnedTile.transform.parent = GameObject.Find("Map" + FindParent(currentTileType)).transform;
            spawnedTile.transform.position += spawnedTile.transform.forward * coloumn * BlockSize;
            spawnedTile.transform.position += spawnedTile.transform.right * row * BlockSize * -1;

            spawnedTile.transform.localScale *= 0.5f * BlockSize;

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
