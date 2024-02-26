using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField]
    private HealthControl healthControl;

    [SerializeField]
    private Vector3 spawnLocation;

    [SerializeField]
    private Renderer flagRender;

    [SerializeField]
    private Material flagOn;

    [SerializeField]
    private Material flagOff;


    // Start is called before the first frame update
    void Start()
    {
        healthControl = FindAnyObjectByType<HealthControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            spawnLocation = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            healthControl.setCheckpoint(spawnLocation);
            checkpointDetect();
            
        }
    }

    private void checkpointDetect()
    {
        CheckpointController[] checkpoints = FindObjectsOfType<CheckpointController>();
        foreach(CheckpointController StoredCheckpoint in checkpoints)
        {
            StoredCheckpoint.checkpointDisable();
        }

        for (int i = 0; i < flagRender.materials.Length; i++)
        {
            if (flagRender.materials[i].name.Contains("flagOff"))
            {
                flagRender.materials[i].color = flagOn.color;
                flagRender.materials[i].name = flagOn.name;
            }
        }
    }

    private void checkpointDisable()
    {
        for (int i = 0; i < flagRender.materials.Length; i++)
        {
            if (flagRender.materials[i].name.Contains("flagOn"))
            {
                flagRender.materials[i].color = flagOff.color;
                flagRender.materials[i].name = flagOff.name;
            }
        }
    }

}
