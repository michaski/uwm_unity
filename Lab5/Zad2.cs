using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zad2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float doorSpeed = 2f;
    public float distance = 0.9f;
    private float startPosition;
    private float endPosition;
    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        endPosition = transform.position.x - distance;
        startPosition = transform.position.x;
    }

    void FixedUpdate()
    {
        if (isOpening && transform.position.x > endPosition)
        {
            transform.Translate(doorSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        else if (isClosing && transform.position.x < startPosition)
        {
            transform.Translate(-doorSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            isOpening = false;
            isClosing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player zbli¿y³ siê do drzwi.");
            isOpening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player oddali³ siê od drzwi.");
            isClosing = true;
        }
    }
}
