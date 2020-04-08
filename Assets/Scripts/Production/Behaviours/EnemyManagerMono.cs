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

        private Tools.GameObjectPool m_EnemyPoolSmall;

        private Tools.GameObjectPool m_EnemyPoolBig;
        private int waveCounter = 0;

        private void Start()
        {
            EnemyManager.GetPath();
            EnemyManager.GetWaves();
            EnemyManager.DoTheDijkstra();

            m_EnemyPoolSmall = new Tools.GameObjectPool(1, m_prefabSmall);
            m_EnemyPoolBig = new Tools.GameObjectPool(1, m_prefabBig);

            InvokeRepeating("Waves", 0, 20);
        }

        public void Waves()
        {
            if(EnemyManager.waveEnd)
            {
                CancelInvoke();
            }

            EnemyManager.SpawnWave(m_EnemyPoolSmall, m_EnemyPoolBig, waveCounter);
            waveCounter++;
        }

    }

}