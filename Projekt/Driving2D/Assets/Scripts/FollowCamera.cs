using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject ObjectToFollow;

    void LateUpdate()
    {
        transform.position = new Vector3(
                ObjectToFollow.transform.position.x,
                ObjectToFollow.transform.position.y,
                -10.0f
            );
    }
}
