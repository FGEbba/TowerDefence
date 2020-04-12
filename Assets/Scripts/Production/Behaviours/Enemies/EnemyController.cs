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

        private Rigidbody rb;
        private Animator anim;
        private bool scaled = false;

        private bool dead = false;
        bool IHealth.Dead { get { return dead; } set { dead = value; } }

        private void OnEnable()
        {
            if (rb == null) { rb = GetComponent<Rigidbody>(); }
            if (anim == null) { anim = GetComponent<Animator>(); }
        }

        public void ResetUnit(List<Vector2Int> path, Vector2Int start, int blockSize)
        {
            m_path = path;
            transform.gameObject.SetActive(true);

            if (!scaled) { transform.localScale *= 0.5f * blockSize; scaled = true; }

            dead = false;
            enemySpeed = enemyData.EnemySpeed;
            enemyHealth = enemyData.EnemyMaxHealth;
            enemyAttack = enemyData.EnemyAttack;
            pathIndex = 0;

            transform.position = Vector3.zero;
            transform.localPosition += transform.forward * start.x;
            transform.localPosition += transform.right * start.y * -1;
            transform.localPosition += transform.up * transform.localScale.y;

            InvokeRepeating("Move", 0, enemySpeed);
            anim.SetBool("isWalking", true);
        }

        private void Move()
        {
            if (pathIndex < m_path.Count - 1)
            {
                //Rotate the enemy towards the next "block"
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
            anim.SetTrigger("Damaged");

            enemyHealth -= damageTaken;

            if (enemyHealth <= 0)
            {
                //I dunno man
                dead = true;
                CancelInvoke();
                anim.SetTrigger("Killed");
                while (anim.GetCurrentAnimatorStateInfo(0).IsName("Dead") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                { Debug.Log("Dead"); }

                transform.gameObject.SetActive(false);
            }

        }

    }
}
