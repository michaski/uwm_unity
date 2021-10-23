using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zad5 : MonoBehaviour
{
    public GameObject Cube;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> cubePositions = new List<Vector3>();
        for (var i = 0; i < 10; i++)
        {
            var newCubePosition = new Vector3(GetRandomCoordinate(), 0.0f, GetRandomCoordinate());
            while (cubePositions.Exists(p => (p - newCubePosition).sqrMagnitude <= 1.5f ))
            {
                newCubePosition.x = Random.Range(1.0f, 9.0f);
                newCubePosition.z = Random.Range(1.0f, 9.0f);
            }
            cubePositions.Add(newCubePosition);
            Instantiate(
                Cube,
                newCubePosition,
                Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private float GetRandomCoordinate()
    {
        return Random.Range(1.0f, 9.0f);
    }
}
