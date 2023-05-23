using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    private List<GameObject> objects;
    
    [SerializeField]
    public int objectCount = 200;

    [SerializeField]
    public float planeRange = 10.0f;

    void Start()
    {
        objects = new List<GameObject>();
        for(int i = 0; i < objectCount; i++) {
            int idx = (int) Random.Range(0, transform.childCount);
            GameObject obj = transform.GetChild(idx).gameObject;
            Vector3 randomPos = new Vector3(Random.Range(-planeRange, planeRange), obj.transform.position.y, Random.Range(-planeRange, planeRange));
            GameObject spawned = Instantiate(obj, randomPos, obj.transform.rotation, transform);
            spawned.transform.localScale *= Random.Range(0.3f, 1.8f);
        }
    }
}
