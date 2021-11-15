using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad3 : MonoBehaviour
{
    public List<GameObject> Waypoints = new List<GameObject>();
    public float platformSpeed = 2f;
    //public float distance = 6.6f;
    private float acceptableDistanceFromWaypoint = 0.1f;
    private bool isRunning = false;
    private bool isRunningForwards = false;
    private bool isRunningBackwards = false;
    private bool isStartLocationCreated = false;
    private int currentWaypointIndex = 1;
    private Transform oldParent;

    void Start()
    {
        if (Waypoints.Count == 0)
        {
            Debug.LogError($"{transform.name}'s path should consist of at least one waypoint.");
            return;
        }
    }

    void FixedUpdate()
    {
        if (isRunningForwards && currentWaypointIndex == Waypoints.Count)
        {
            isRunningForwards = false;
            isRunningBackwards = true;
            currentWaypointIndex = Waypoints.Count - 2;
        }
        else if (isRunningBackwards && currentWaypointIndex < 0)
        {
            isRunningForwards = true;
            isRunningBackwards = false;
            currentWaypointIndex = 1;
            isRunning = false;
            Destroy(Waypoints[0]);
            isStartLocationCreated = false;
            Waypoints.RemoveAt(0);
        }

        if (isRunning)
        {
            Vector3 step = Vector3.zero;
            if (isRunningForwards)
            {
                step = Waypoints[currentWaypointIndex].transform.position - transform.position;
                if (step.magnitude <= acceptableDistanceFromWaypoint)
                {
                    Debug.Log($"Arrived at waypoint {Waypoints[currentWaypointIndex].transform.name}");
                    currentWaypointIndex++;
                }
            }
            else
            {
                step = Waypoints[currentWaypointIndex].transform.position - transform.position;
                if (step.magnitude <= acceptableDistanceFromWaypoint)
                {
                    Debug.Log($"Arrived at waypoint {Waypoints[currentWaypointIndex].transform.name}");
                    currentWaypointIndex--;
                }
            }    
            Vector3 move = step * Time.deltaTime;
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
            isRunning = true;
            isRunningForwards = true;
            if (!isStartLocationCreated)
            {
                var startLocation = new GameObject($"{transform.name} start loaction ({Guid.NewGuid()})");
                startLocation.transform.position = transform.position;
                Waypoints.Insert(0, startLocation);
                isStartLocationCreated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player zszed³ z windy.");
            other.gameObject.transform.parent = oldParent;
            oldParent = null;
            isRunningForwards = false;
            isRunningBackwards = true;
            currentWaypointIndex = 0;
        }
    }
}
