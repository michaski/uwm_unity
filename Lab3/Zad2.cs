using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad2 : MonoBehaviour
{
    [SerializeField] float Speed = 1.0f;
    private int Direction = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 10)
        {
            Direction = -1;
        }
        else if (transform.position.x <= 0)
        {
            Direction = 1;
        }
        transform.Translate(Direction * Speed * Time.deltaTime, 0, 0);
    }
}
