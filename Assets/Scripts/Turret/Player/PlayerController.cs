using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;

    public static PlayerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerController>();
            }

            return _instance;
        }
    }
    
    public TurretMechanic turretMechanic;
    [SerializeField] private Teleporter startingTeleporter;
    private bool teleportationActivated = false;
    private bool pressedDown = false;
    private IEnumerator shootCor;

    private void Awake()
    {
        turretMechanic.TurretDestroy += OnTurretDestroyed;
        GameOver.instance.GameOverActive += RespawnPlayer;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!turretMechanic.gameObject.activeInHierarchy) return;
        if (StartPage.instance.isPageActive()) return;
        if (Input.GetMouseButtonDown(0))
        {
            pressedDown = true;
            StartShooting();
            teleportationActivated = RayCastToPosition((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0))
        {
            if(!teleportationActivated)turretMechanic.RotateTowards(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopShooting();
            pressedDown = false;
            teleportationActivated = false;
            // RotateTowards(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                
                case TouchPhase.Began:
                    pressedDown = true;
                    StartShooting();
                    teleportationActivated = RayCastToPosition((Vector2)Camera.main.ScreenToWorldPoint(touch.position));
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    if(!teleportationActivated)turretMechanic.RotateTowards(touch.position);
                    break;
                case TouchPhase.Ended:
                    StopShooting();
                    pressedDown = false;
                    teleportationActivated = false;
                    break;
                
            }
        }
    }

    public void StartShooting()
    {
        StopShooting();
        shootCor = ShotingEnum();
        StartCoroutine(shootCor);
    }

    public void StopShooting()
    {
        if (shootCor != null)
        {
            StopCoroutine(shootCor);
            shootCor = null;
        }
    }

    IEnumerator ShotingEnum()
    {
        while (true)
        {
            turretMechanic.ShotTurret();
            yield return new WaitForSeconds(.2f);
        }
    }
    
    public bool RayCastToPosition(Vector3 castPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(castPosition, Vector2.zero,2f);

        // If it hits something...
        if (hit.collider != null)
        {
            var teleporter=hit.collider.GetComponent<Teleporter>();
            if (teleporter != null)
            {
                teleporter.TeleportToPosition(turretMechanic.transform.parent);
                return true;
            }
        }

        return false;
    }
    
    public void OnTurretDestroyed(TurretMechanic turretMechanic)
    {
        turretMechanic.transform.parent.localScale=Vector3.one;
        turretMechanic.gameObject.SetActive(false);
        GameOver.instance.ShowGameOver(true);
    }

    public void RespawnPlayer()
    {
        turretMechanic.gameObject.SetActive(true);
        turretMechanic.RestoreLifeBar();
        startingTeleporter.TeleportToPosition(turretMechanic.transform.parent);
    }
}
