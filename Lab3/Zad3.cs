using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad3 : MonoBehaviour
{
    [SerializeField] float Speed = 1.0f;
    int DirectionX = 1;
    int DirectionZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DirectionX == 1 && transform.position.x >= 10)
        {
            DirectionX = 0;
            DirectionZ = 1;
            transform.Rotate(0, 90, 0, Space.World);
        }
        else if (DirectionX == -1 && transform.position.x <= 0)
        {
            DirectionX = 0;
            DirectionZ = -1;
            transform.Rotate(0, 90, 0, Space.World);
        }
        else if (DirectionZ == 1 && transform.position.z >= 10)
        {
            DirectionX = -1;
            DirectionZ = 0;
            transform.Rotate(0, 90, 0, Space.World);
        }
        else if (DirectionZ == -1 && transform.position.z <= 0)
        {
            DirectionX = 1;
            DirectionZ = 0;
            transform.Rotate(0, 90, 0, Space.World);
        }
        transform.Translate(DirectionX * Speed * Time.deltaTime, 0, DirectionZ * Speed * Time.deltaTime, Space.World);
    }
}
