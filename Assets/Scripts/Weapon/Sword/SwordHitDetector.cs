using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitDetector : WeaponHitDetector
{
    public event Action ResetAnimation;
    public override void OnWeaponHitLogic()
    {
        ResetAnimation?.Invoke();
    }
}
