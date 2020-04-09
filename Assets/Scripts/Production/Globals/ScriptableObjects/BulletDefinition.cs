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
        private Shit bulletType;

        [SerializeField]
        private float maxBulletSpeed;

        [SerializeField]
        private float minBulletSpeed;
    }

    public enum Shit
    {
        Freeze,
        AreaDamage
    }
}
