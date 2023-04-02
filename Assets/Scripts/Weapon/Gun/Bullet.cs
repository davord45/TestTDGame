using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponHitDetector
{
    private Vector3 initialPosition;

    private float speed = .3f;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    public void StartMovingBullet(Quaternion targetRot,Vector3 startPosition)
    {
        transform.position = startPosition;
        initialPosition = transform.localPosition;
        StartCoroutine(StartMovingDirection(targetRot, 2f));
    }

    IEnumerator StartMovingDirection(Quaternion targetRot,float travelDuration)
    {
        Vector3 targetForward = (targetRot * Vector2.left)*speed;
        for (float i = 0; i<travelDuration; i=i+Time.deltaTime)
        {
            transform.localPosition += targetForward;
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    public void RestartPosition()
    {
        StopAllCoroutines();
        transform.localPosition = initialPosition;
    }

    public override void OnWeaponHitLogic()
    {
        RestartPosition();
        gameObject.SetActive(false);
    }
}
