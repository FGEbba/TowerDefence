using UnityEngine;


namespace Scriptable
{

    [CreateAssetMenu(fileName = "Data", menuName = "Boxymon", order = 51)]
    public class EnemyDefinition : ScriptableObject
    {
        [SerializeField]
        private int enemyMaxHealth;

        [SerializeField]
        private float enemySpeed;

        [SerializeField]
        private int enemyAttack;

        [SerializeField]
        private GameObject enemyPrefab;

        public int EnemyMaxHealth { get { return enemyMaxHealth; } }
        public float EnemySpeed { get { return enemySpeed; } }
        public int EnemyAttack { get { return enemyAttack; } }
        public GameObject EnemyPrefab { get { return enemyPrefab; } }

    }



}