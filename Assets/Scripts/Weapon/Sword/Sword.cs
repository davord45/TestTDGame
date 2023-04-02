using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : Weapon
{
    private Sequence swingSeq;
    private SwordHitDetector _swordHitDetector;

    private void Start()
    {
        _swordHitDetector = GetComponentInChildren<SwordHitDetector>();
        _swordHitDetector.OnHitDetected += OnWeaponHitDetected;
        // _swordHitDetector.ResetAnimation += AnimateSword;
    }

    public override void ActivateWeapon()
    {
        AnimateSword();
    }

    public void AnimateSword()
    {
        if(swingSeq!=null)swingSeq.Kill();
        swingSeq = DOTween.Sequence();
        for (int i = 0; i < 2; i++)
        {
            swingSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, 50f), .3f).SetEase(Ease.OutQuint));
            swingSeq.Append(transform.DOLocalRotate(new Vector3(0, 0, -50f), .3f).SetEase(Ease.OutQuint));
        }
        swingSeq.Append(transform.DOLocalRotate(Vector3.zero, .3f).SetEase(Ease.OutQuint));
    }
}
