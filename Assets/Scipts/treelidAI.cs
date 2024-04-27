using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Audio;

public class treelidAI : MonoBehaviour
{

    [SerializeField]
    private Transform treelid;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 startPos;

    [SerializeField]
    private Vector3 movePosition;

    [SerializeField]
    private bool agroed;

    [SerializeField]
    private bool chasing;


    [Header("Enemy Controls")]

    [SerializeField]
    private float aggroRange;

    [SerializeField]
    private float returnTime;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 1)
        {
            transform.Translate(Vector3.down * gravity * Time.deltaTime);
        }

        distance = Vector3.Distance(startPos, target.position);

        if (distance <= aggroRange)
        {
            chase();
        }

        if (distance > aggroRange)
        {
            deAggro();
        }
    }

    void chase()
    {
        movePosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(movePosition);
        transform.position = Vector3.MoveTowards(treelid.position, movePosition, speed * Time.deltaTime);
    }

    void deAggro()
    {
        transform.LookAt(startPos);
        transform.position = Vector3.Lerp(transform.position, startPos, returnTime * Time.deltaTime);
    }

}
