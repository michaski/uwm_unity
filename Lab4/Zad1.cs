using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zad1 : MonoBehaviour
{
    List<Vector3> positions = new List<Vector3>();
    public List<Material> Materials = new List<Material>();
    public float delay = 3.0f;
    public int count = 10;
    int objectCounter = 0;
    MeshRenderer renderer;
    // obiekt do generowania
    public GameObject block;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        // w momecie uruchomienia generuje 10 kostek w losowych miejscach
        List<int> pozycje_x = new List<int>(
            Enumerable
                //.Range((int)renderer.bounds.min.x, (int)renderer.bounds.max.x)
                .Range(0, (int)renderer.bounds.size.x)
                .OrderBy(x => Guid.NewGuid())
                .Take(count));
        List<int> pozycje_z = new List<int>(
            Enumerable
                //.Range((int)renderer.bounds.min.z, (int)renderer.bounds.max.z)
                .Range(0, (int)renderer.bounds.size.z)
                .OrderBy(x => Guid.NewGuid())
                .Take(count));

        for (int i = 0; i < count; i++)
        {
            this.positions.Add(new Vector3(
                pozycje_x[i] - (int)renderer.bounds.max.x, 
                0, 
                pozycje_z[i] - (int)renderer.bounds.max.z));
        }
        foreach (Vector3 elem in positions)
        {
            Debug.Log(elem);
        }
        // uruchamiamy coroutine
        StartCoroutine(GenerujObiekt());
    }

    void Update()
    {

    }

    IEnumerator GenerujObiekt()
    {
        Debug.Log("wywo³ano coroutine");
        foreach (Vector3 pos in positions)
        {
            var newObject = Instantiate(this.block, this.positions.ElementAt(this.objectCounter++), Quaternion.identity);
            newObject.GetComponent<MeshRenderer>().material = this.Materials[UnityEngine.Random.Range(0, Materials.Count)];
            yield return new WaitForSeconds(this.delay);
        }
        // zatrzymujemy coroutine
        StopCoroutine(GenerujObiekt());
    }
}

