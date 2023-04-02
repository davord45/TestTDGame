using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHolder : MonoBehaviour
{
    private static BulletHolder _instance;

    public static BulletHolder instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BulletHolder>();
            }

            return _instance;
        }
    }
}
