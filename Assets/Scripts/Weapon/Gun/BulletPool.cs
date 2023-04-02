using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Weapon
{
    [SerializeField] private GameObject bulletPrefab;
    GameObject bulletParent;
    [SerializeField] private string shotFrom;
    private List<Bullet> bulletList;
    private int currentIndex = -1;

    private void Start()
    {
        bulletList = new List<Bullet>();
        bulletParent = BulletHolder.instance.gameObject;
        for (int i = 0; i < 20; i++)
        {
            var newBullet = Instantiate(bulletPrefab, bulletParent.transform);
            var bulletComp = newBullet.GetComponent<Bullet>();
            newBullet.SetActive(false);
            bulletList.Add(bulletComp);
        }
    }

    public override void ActivateWeapon()
    {
        ShootNextBullet();
    }

    public void ShootNextBullet()
    {
        var nextBullet = GetNextBullet();
        nextBullet.gameObject.SetActive(true);
        nextBullet.OnHitDetected -= OnWeaponHitDetected;
        nextBullet.OnHitDetected += OnWeaponHitDetected;
        nextBullet.SetIgnoreLayer(shotFrom);
        nextBullet.RestartPosition();
        nextBullet.StartMovingBullet(transform.parent.rotation,transform.position);
    }

    public Bullet GetNextBullet()
    {
        int nextIndex = (currentIndex + 1) % bulletList.Count;
        currentIndex = nextIndex;
        return bulletList[nextIndex];
    }
}
