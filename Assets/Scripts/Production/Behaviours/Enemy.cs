using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int m_EnemyMaxHealth;

        [SerializeField]
        private int m_EnemySpeed;

        [SerializeField]
        private int m_EnemyAttack;

        private IEnumerable<Vector3> m_path;


        public void SetPathFromDijkstra(IEnumerable<Vector3> path) { m_path = path; }


        private void Move()
        {

        }

        private void Attack()
        {

        }

    }
}
