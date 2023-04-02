using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private AILerp ailerpComponent;

    private void OnTriggerEnter2D(Collider2D col)
    {
        int obstacleLayer=LayerMask.NameToLayer("Player");
        if (col.gameObject.layer == obstacleLayer)
        {
            ailerpComponent.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int obstacleLayer=LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == obstacleLayer)
        {
            ailerpComponent.enabled = true;
        }
    }
}
