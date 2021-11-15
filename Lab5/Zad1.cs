using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad1 : MonoBehaviour
{
    public float elevatorSpeed = 2f;
    private bool isRunning = false;
    public float distance = 6.6f;
    private bool isRunningUp = true;
    private bool isRunningDown = false;
    private float downPosition;
    private float upPosition;
    private Transform oldParent;

    void Start()
    {
        upPosition = transform.position.x + distance;
        downPosition = transform.position.x;
    }

    void FixedUpdate()
    {
        if (isRunningUp && transform.position.x >= upPosition)
        {
            isRunning = false;
        }
        else if (isRunningDown && transform.position.x <= downPosition)
        {
            isRunning = false;
        }

        if (isRunning)
        {
            var step = elevatorSpeed * Time.deltaTime;
            Vector3 move = transform.right * step;
            transform.Translate(move);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player wszed³ na windê.");
            if (oldParent == null)
            {
                // zapamiêtujemy "starego rodzica"
                oldParent = other.gameObject.transform.parent;
                // skrypt przypisany do windy, ale other mo¿e byæ innym obiektem
                other.gameObject.transform.parent = transform;
            }
            if (transform.position.x >= upPosition)
            {
                isRunningDown = true;
                isRunningUp = false;
                elevatorSpeed = -elevatorSpeed;
            }
            else if (transform.position.x <= downPosition)
            {
                isRunningUp = true;
                isRunningDown = false;
                elevatorSpeed = Mathf.Abs(elevatorSpeed);
            }
            isRunning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player zszed³ z windy.");
            other.gameObject.transform.parent = oldParent;
            oldParent = null;
        }
    }
}
