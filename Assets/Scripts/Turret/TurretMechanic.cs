using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurretMechanic : MonoBehaviour
{
    private Weapon weapon;
    [SerializeField] int lifeAmount=5;
    private int startLifeAmount;
    [SerializeField] private Image lifeBar;
    
    public event Action<TurretMechanic> TurretDestroy;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        // weapon.OnWeaponHit += TargetDestroyed;
        startLifeAmount = lifeAmount;
    }

    public void ShotTurret()
    {
        weapon.ActivateWeapon();
    }
    
    public void RotateTowards(Vector3 screenPosition)
    {
        Vector2 positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        
        Vector2 viewportPoint = screenPosition;
         
        float angle = AngleBetweenTwoPoints(positionOnScreen, viewportPoint);

        var rotateTowards = Quaternion.Euler(new Vector3(0f, 0f, angle));
        
        transform.rotation =  Quaternion.Lerp(transform.rotation,rotateTowards,.5f);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    
    public bool TakeDamage()
    {
        if (lifeAmount <= 0) return true;
        transform.DOComplete();
        
        lifeAmount--;
        SetLifeBar();
        if (lifeAmount == 0)
        {
            DestroyTurret();
            return true;
        }
        transform.DOShakeScale(.5f,.5f,10);

        return false;
    }

    public void RestoreLifeBar()
    {
        lifeAmount = startLifeAmount;
        SetLifeBar();
    }

    public void DestroyTurret()
    {
        var deathSeq = DOTween.Sequence();
        deathSeq.Append(transform.parent.DOScale(Vector3.zero, .4f).SetEase(Ease.InBounce));
        deathSeq.AppendCallback(() =>
        {
            TurretDestroy?.Invoke(this);
        });
    }

    public void SetLifeBar()
    {
        float percent = startLifeAmount / 100f;
        float lifeBarAmount = lifeAmount/percent;
        lifeBarAmount /= 100f;
        lifeBar.fillAmount = lifeBarAmount;
    }
}
