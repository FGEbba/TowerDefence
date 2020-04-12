using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{

    public class TowerManagerMono : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_TowerOnePrefab;

        [SerializeField]
        private GameObject m_TowerTwoPrefab;

        private void Start()
        {
            TowerManager.SetBulletPools(m_TowerOnePrefab, m_TowerTwoPrefab);
        }
    }

}