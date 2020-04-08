using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Enemies
{

    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Scriptable.EnemyDefinition enemyData;

        private int pathIndex = 0;
        private List<Vector2Int> m_path;

        private int enemyHealth;
        private float enemySpeed;
        private int enemyAttack;

        public void ResetUnit(List<Vector2Int> path, Vector2Int start)
        {
            m_path = path;
            enemySpeed = enemyData.EnemySpeed;
            enemyHealth = enemyData.EnemyMaxHealth;
            enemyAttack = enemyData.EnemyAttack;


            transform.position = Vector3.zero;
            transform.localPosition += transform.forward * start.x;
            transform.localPosition += transform.right * start.y * -1;

            InvokeRepeating("Move", 0, enemySpeed);
        }

        private void Move()
        {
            if (pathIndex < m_path.Count - 1)
            {
                pathIndex++;
                transform.position = Vector3.zero;
                transform.localPosition += transform.forward * m_path[pathIndex].x;
                transform.localPosition += transform.right * m_path[pathIndex].y * -1;
            }
            else
            {
                transform.gameObject.SetActive(false);
                
            }

        }

        public void Attack()
        {

        }

    }
}
