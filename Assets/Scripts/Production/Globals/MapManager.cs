using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapCreation
{


    [Serializable]
    public class MapDataExposed
    {
        [SerializeField] private TileType m_Type;
        [SerializeField] private GameObject m_Prefab;

        public TileType TileType { get => m_Type; }
        public GameObject Prefab { get => m_Prefab; }
    }


    public class MapManager : MonoBehaviour
    {

        #region Serialized Fields
        [SerializeField]
        private TextAsset mapDoc;

        [SerializeField]
        [Tooltip("The size of the blocks.")]
        private int m_BlockSize;

        [SerializeField]
        [Tooltip("Requires at least six slots for the different tiles.")]
        private MapDataExposed[] mapDatas;
        #endregion

        private static int blockSize;
        private static List<MapData> data = new List<MapData>();
        private static List<string> fileContent = new List<string>();

        private void Awake()
        {
            data = MapCreator.CheckDataMaps(mapDatas);
            fileContent = MapCreator.CheckTextAsset(mapDoc);
            blockSize = m_BlockSize;

            StartMapSpawn();
        }

        public static void StartMapSpawn()
        {
            MapCreator.GetArrayAndSpawnMap(fileContent, data, blockSize);
        }

        public static Dictionary<int, int> GetWavesFromFile()
        {
            Dictionary<int, int> waves = new Dictionary<int, int>();
            waves = Files.FileReader.GetWavesFileContent(Files.FileReader.ReadMapFile());
            return waves;
        }

        public static List<Vector3> GetPathVec3()
        {
            return MapCreator.GetAccessableTiles();
        }

        public static List<Vector2Int> AccessableTilesConverter(List<Vector3> accessTilesVec3)
        {
            List<Vector2Int> accessTilesVec2Int = new List<Vector2Int>();

            foreach (Vector3 pos in accessTilesVec3)
            {
                accessTilesVec2Int.Add(ConvertVec3PosToVec2int(pos));
            }
            return accessTilesVec2Int;
        }

        public static Vector2Int ConvertVec3PosToVec2int(Vector3 pos)
        {
            return new Vector2Int((int)pos.x, (int)pos.z);
        }



    }
}
