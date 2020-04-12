using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;


namespace Tower
{
    public class TowerManager
    {
        private List<GameObject> towerList = new List<GameObject>();

        private static GameObjectPool freezeBulletPool;
        private static GameObjectPool damageBulletPool;

        public static void SetBulletPools(GameObject towerOnePrefab, GameObject towerTwoPrefab)
        {
            if(towerOnePrefab.GetComponent<TowerController>())
            {
                //GameObject bulletOne = towerOnePrefab.GetComponent<TowerController>().
            }

        }

    }
}
