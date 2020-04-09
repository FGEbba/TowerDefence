using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{


    public class EnemyManagerMono : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_prefabSmall;

        [SerializeField]
        private GameObject m_prefabBig;

        [SerializeField]
        private int m_unitTimer;

        
        private void Start()
        {
            EnemyManager.GetPath();
            EnemyManager.DoTheDijkstra();

            EnemyManager.GetWaves();
            EnemyManager.SetPools(m_prefabSmall, m_prefabBig);

            InvokeRepeating("Waves", 0, m_unitTimer);

        }

        public void Waves()
        {
            EnemyManager.WaveChecker();
        }

    }

}