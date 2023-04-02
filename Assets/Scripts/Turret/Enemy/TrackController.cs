using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : TurretController
{
    private Transform trackingTarget;
    [SerializeField] private float shootInterval=2f;

    private void Start()
    {
        trackingTarget = PlayerController.instance.turretMechanic.transform;
        EnableTracking(true);
    }

    void Update () 
    {
        if (trackingTarget != null)
        {
            var trackingPosition=Camera.main.WorldToScreenPoint(trackingTarget.transform.position);
            turretMechanic.RotateTowards(trackingPosition);
        }
    }

    public void EnableTracking(bool enable)
    {
        if (enable)
        {
            StartCoroutine(WaitShoot());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator WaitShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            turretMechanic.ShotTurret();
        }
    }
}
