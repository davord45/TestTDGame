using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [HideInInspector]
    public TurretMechanic turretMechanic;

    private void Awake()
    {
        turretMechanic = GetComponent<TurretMechanic>();
    }
}
