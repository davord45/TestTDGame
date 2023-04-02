using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void ActivateWeapon();

    public void OnWeaponHitDetected(bool killedEnemy)
    {
        if(killedEnemy)OnWeaponHit?.Invoke();
    }
    
    public event Action OnWeaponHit;
}
