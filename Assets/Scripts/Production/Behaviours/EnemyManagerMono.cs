using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{


    public class EnemyManagerMono : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_enemyPrefabList;

        private void Start()
        {
            EnemyManager.CheckEnemyPrefabs(m_enemyPrefabList);

        }

    }

}