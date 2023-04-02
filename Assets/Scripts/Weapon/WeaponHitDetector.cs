using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitDetector : MonoBehaviour
{
    public event Action<bool> OnHitDetected;

    public string ignoreLayerName;

    public void SetIgnoreLayer(string setTo)
    {
        ignoreLayerName = setTo;
    }

    public virtual void OnWeaponHitLogic()
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        var layerName = LayerMask.LayerToName(col.gameObject.layer);
        switch (layerName)
        {
            case "Bullet":
            case "Obstacle":
                OnWeaponHitLogic();
                break;
            case "Enemy":
            case "Player":
                if (layerName != ignoreLayerName)
                {
                    OnWeaponHitLogic();
                    var turretHit=col.GetComponent<TurretMechanic>();
                    if (turretHit != null)
                    {
                        OnHitDetected?.Invoke(turretHit.TakeDamage());
                    }
                }
                break;
        }
    }
}
