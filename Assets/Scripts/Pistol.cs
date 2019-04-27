using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, Weapon
{
    public float FireInterval = 0.03f;
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public int Damage = 1;
    private float lastFireTime = 0;
    private bool firing = false;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void Firing(bool down) {
        if (down && Time.time - lastFireTime > 0.03f) {
            GameObject newBullet = Instantiate(BulletPrefab);
            newBullet.transform.position = FirePoint.position;
            NormalBullet bulletData = newBullet.AddComponent<NormalBullet>();
            bulletData.Damage = Damage;
            bulletData.FireAngle = transform.rotation.eulerAngles.z;
            bulletData.Speed = 15f;
            bulletData.Lifespan = 1.5f;
            bulletData.Target = 1 << LayerMask.NameToLayer("Enemy");
            lastFireTime = Time.time;
        }
    }
}
