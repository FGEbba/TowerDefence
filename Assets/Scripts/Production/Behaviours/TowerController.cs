using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BulletSpawnPoint;

    [SerializeField]
    private GameObject m_BulletPrefab;

    [SerializeField]
    private int m_ShootSpeed;

    private GameObject bulletSpawnPoint;
    private GameObject bulletPrefab;
    private static Tools.GameObjectPool normalBulletPool;
    private static Tools.GameObjectPool freezeBulletPool;
    private Tools.GameObjectPool bulletPool;

    private GameObject cannonHead;
    private Quaternion originalRotationValue;


    public List<GameObject> activeTargets = new List<GameObject>();

    private Rigidbody rb;


    private void Start()
    {
        if (rb == null) { rb = GetComponent<Rigidbody>(); }
        //Make sure there's a spawnpoint!
        if (m_BulletSpawnPoint == null) { throw new NullReferenceException("No bullet spawnpoint was found!"); }
        else { bulletSpawnPoint = m_BulletSpawnPoint; }
        cannonHead = bulletSpawnPoint.transform.parent.parent.gameObject;
        originalRotationValue = cannonHead.transform.rotation;

        //Check so there's a bullet Prefab
        if (m_BulletPrefab != null) { bulletPrefab = m_BulletPrefab; bulletPrefab.GetComponent<BulletController>().SetBulletType(); }

        //Setup the Object Pools!
        if (bulletPrefab.GetComponent<BulletController>().bulletType == Scriptable.BulletTypes.Freeze)
        {
            if (freezeBulletPool == null) { freezeBulletPool = new Tools.GameObjectPool(1, bulletPrefab); }
            bulletPool = freezeBulletPool;
        }

        if (bulletPrefab.GetComponent<BulletController>().bulletType == Scriptable.BulletTypes.AreaDamage)
        {
            if (normalBulletPool == null) { normalBulletPool = new Tools.GameObjectPool(1, bulletPrefab); }
            bulletPool = normalBulletPool;
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = bulletPool.Rent(false);
        BulletController bulletComponent = bullet.GetComponent<BulletController>();
        bulletComponent.SetBulletType();

        if (GameObject.Find("BulletPool"))
        {
            if (GameObject.Find("FreezeBullet") && bulletComponent.bulletType == Scriptable.BulletTypes.Freeze) { bullet.transform.parent = GameObject.Find("BulletPool/FreezeBullet").transform; }
            if (GameObject.Find("FreezeBullet") && bulletComponent.bulletType == Scriptable.BulletTypes.AreaDamage) { bullet.transform.parent = GameObject.Find("BulletPool/NormalBullet").transform; }
        }

        bullet.transform.position = Vector3.zero;
        bullet.transform.position = bulletSpawnPoint.transform.position;

        bullet.SetActive(true);
        Tools.EmitOnDisable emitOnDisable = bullet.GetComponent<Tools.EmitOnDisable>();
        emitOnDisable.OnDisableGameObject += BulletDisable;

        bulletComponent.ResetBullet(MapCreation.MapCreator.BlockSize, activeTargets[0].transform.position);

    }
    private void BulletDisable(GameObject bullet)
    {
        bullet.GetComponent<Tools.EmitOnDisable>().OnDisableGameObject -= BulletDisable;
    }

    private void ResetPosition()
    {
        CancelInvoke();
        cannonHead.transform.rotation = originalRotationValue;
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger");
        IHealth otherComp = (IHealth)other.gameObject.GetComponentInParent(typeof(IHealth));

        if (otherComp != null && otherComp.Dead) { activeTargets.Remove(other.gameObject); }
        if (activeTargets.Count > 0)
        { cannonHead.transform.LookAt(activeTargets[0].transform.position); }
        else { ResetPosition(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        IHealth otherComp = (IHealth)other.gameObject.GetComponentInParent(typeof(IHealth));
        if (otherComp != null)
        {
            //Add to List if not already in list
            if (!activeTargets.Contains(other.gameObject)) { activeTargets.Add(other.gameObject); }
            if (activeTargets.Count > 0)
            {
                InvokeRepeating("ShootBullet", 0, m_ShootSpeed);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject.transform.parent.gameObject.name}");
        activeTargets.Remove(other.gameObject);
        if (activeTargets.Count < 1)
        {
            ResetPosition();
        }
    }

}
