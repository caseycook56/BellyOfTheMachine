using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<Rigidbody> objectsRigidbody = new List<Rigidbody>();
    private GameObject[] spawnerObjects;
    public float maxY = 10.0f;
    public float z = -1;
    public float minX = 2;
    public float maxX = 9;

    void Start()
    {
       spawnerObjects = GameObject.FindGameObjectsWithTag("Spawnable");
        for (int i = 0; i < spawnerObjects.Length; i++)
        {
            Rigidbody rb;
            spawnerObjects[i].TryGetComponent<Rigidbody>(out rb);

            objectsRigidbody.Add(rb == null ? spawnerObjects[i].GetComponentInChildren<Rigidbody>() : rb);
            objectsRigidbody[i].useGravity = false;
            objectsRigidbody[i].constraints = RigidbodyConstraints.FreezeAll;
            spawnerObjects[i].SetActive(false);

        }
    }
    private void FixedUpdate()
    { 
        float randomTime = Random.Range(1, 1000);

        if (randomTime < 50)
        {
            int index = Random.Range(0, objectsRigidbody.Count);
            Debug.Log("Index: " + index);
            if (spawnerObjects[index] != null) {
                spawnerObjects[index].SetActive(true);
                objectsRigidbody[index].constraints = RigidbodyConstraints.None;
                objectsRigidbody[index].useGravity = true;
                objectsRigidbody[index].position = new Vector3(Random.Range(minX, maxX), maxY, z);
            }
        }
    }
}
