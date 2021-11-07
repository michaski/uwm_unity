using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad6 : MonoBehaviour
{
    // Start is called before the first frame update
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Obstacle")
        {
            Debug.Log($"You've hit obstacle {hit.gameObject.name}");
        }
    }
}
