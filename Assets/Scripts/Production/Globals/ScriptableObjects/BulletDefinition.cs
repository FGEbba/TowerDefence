using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptable
{
    [CreateAssetMenu(fileName = "Data", menuName = "Bullet", order = 52)]
    public class BulletDefinition : ScriptableObject
    {
        [SerializeField]
        private int damage;

        [SerializeField]
        private BulletTypes bulletType;

        [SerializeField]
        private float bulletSpeed;


        public int Damage { get { return damage; } }
        public BulletTypes BulletType { get { return bulletType; } }
        public float BulletSpeed { get { return bulletSpeed; } }
    }

    public enum BulletTypes
    {
        Freeze,
        AreaDamage
    }
}
