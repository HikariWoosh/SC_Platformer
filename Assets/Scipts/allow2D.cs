using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class allow2D : MonoBehaviour
{
    [SerializeField]
    private CameraControl control;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private CameraControlSecondDimension control2;

    [SerializeField]
    private HealthControl healthControl;

    [SerializeField]
    private Vector3 zoneOffset;

    [SerializeField]
    private Quaternion zoneRotation;

    [SerializeField]
    private bool allow2DMovement;



    private void Start()
    {
        findCamera();
    }

    private void Update()
    {
        if (healthControl.Health < 1)
        {
            control.canSwitch = false;
            control.triggerLeave();
        }
    }

    private void findCamera()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        control = FindAnyObjectByType<CameraControl>();
        control2 = FindAnyObjectByType<CameraControlSecondDimension>();
        healthControl = GameObject.Find("gameManager").GetComponent<HealthControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            control2.changeOffset(zoneOffset, zoneRotation);
            control.canSwitch = true;
            player.Movement2D = allow2DMovement;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            control.canSwitch = false;
            control.triggerLeave();
        }
    }
}
