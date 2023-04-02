using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public void TeleportToPosition(Transform teleportGO)
    {
        teleportGO.position = transform.position;
    }
}
