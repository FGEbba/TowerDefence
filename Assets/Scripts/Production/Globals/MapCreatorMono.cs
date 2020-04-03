using System;
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

    public class MapCreatorMono : MonoBehaviour
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

        private List<MapData> data = new List<MapData>();
        private List<string> fileContent = new List<string>();

        private void Awake()
        {
            data = MapReader.CheckDataMaps(mapDatas);
            fileContent = MapReader.CheckTextAsset(mapDoc);

            MapReader.GetArrayAndSpawnMap(fileContent, data, m_BlockSize);
        }
    }

}