using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{

    public class EnemyController : MonoBehaviour, IHealth
    {
        [SerializeField]
        private Scriptable.EnemyDefinition enemyData;

        private int pathIndex = 0;
        private List<Vector2Int> m_path;

        private int enemyHealth;
        private float enemySpeed;
        private int enemyAttack;
        private int block;

        private Rigidbody rb;
        private bool scaled = false;

        private void OnEnable()
        {
            //rb = GetComponent<Rigidbody>();
        }

        public void ResetUnit(List<Vector2Int> path, Vector2Int start, int blockSize)
        {
            m_path = path;
            transform.gameObject.SetActive(true);
            block = blockSize;

            if (!scaled) { transform.localScale *= 0.5f * block; scaled = true; }

            enemySpeed = enemyData.EnemySpeed;
            enemyHealth = enemyData.EnemyMaxHealth;
            enemyAttack = enemyData.EnemyAttack;
            pathIndex = 0;

            transform.position = Vector3.zero;
            transform.localPosition += transform.forward * start.x;
            transform.localPosition += transform.right * start.y * -1;
            transform.localPosition += transform.up * transform.localScale.y;

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

                transform.localPosition += transform.up * transform.localScale.y;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            IHealth otherCompHealth = (IHealth)collision.gameObject.GetComponent(typeof(IHealth));
            if (otherCompHealth != null)
            {
                otherCompHealth.Damage(enemyAttack);
                CancelInvoke();
                transform.gameObject.SetActive(false);
            }
        }
        


        public void Damage(int damageTaken)
        {
            throw new System.NotImplementedException();
        }
    }
}
