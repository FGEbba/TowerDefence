using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private Scriptable.BulletDefinition bulletData;

    public Scriptable.BulletTypes bulletType { get; private set; }
    private Rigidbody rb;

    private float speed;
    private int bulletDamage;

    private bool scaled = false;
    private Vector3 moveDir;

    private void Update()
    {
        transform.Translate(moveDir * Time.deltaTime * speed);
    }

    private void OnEnable()
    {
        if (rb == null) { rb = GetComponent<Rigidbody>(); }
    }

    //Make sure the bullet type gets set!
    public void SetBulletType()
    {
        bulletType = bulletData.BulletType;
    }

    public void ResetBullet(int blockSize, Vector3 target)
    {
        if (!scaled) { transform.localScale *= 0.5f * blockSize; scaled = true; }

        //Reset all data!
        speed = bulletData.BulletSpeed;
        bulletDamage = bulletData.Damage;
        SetBulletType();

        moveDir = (target - transform.position).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHealth otherCompHealth = (IHealth)collision.gameObject.GetComponent(typeof(IHealth));
        if (otherCompHealth != null) { otherCompHealth.Damage(bulletDamage); }
        transform.gameObject.SetActive(false);
    }
}
