using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterObjectsSpawner : MonoBehaviour
{
    [SerializeField] private float minRadius = 1;
    [SerializeField] private float maxRadius = 10;
    [SerializeField] private int rejectionSamples = 30;
    [SerializeField] private Vector2 regionSize = Vector2.one;
    [SerializeField] private Vector2 regionOffset = Vector2.zero;
    [SerializeField] private Vector2 noiseScale = Vector2.one;
    private Transform spawnedParent;
    private List<GameObject> prefabs;
    private List<PoissonDiscSamplingWithVariableDensity.Point> points;

    void Start()
    {
        spawnedParent = transform.Find("SpawnedParent");
        prefabs = new List<GameObject>();
        Transform prefabParent = transform.Find("PrefabParent");
        if(prefabParent != null) {
            for(int i = 0; i < prefabParent.childCount; i++) {
                prefabs.Add(prefabParent.GetChild(i).gameObject);
            }
        }
        SpawnObjects();
    }

    void OnValidate()
    {
        if(prefabs != null && prefabs.Count > 0) {
            DestroySpawnedObjects();
            SpawnObjects();
        }
    }

    void DestroySpawnedObjects()
    {
        for(int i = spawnedParent.childCount - 1; i >= 0; i--) {
            GameObject obj = spawnedParent.GetChild(i).gameObject;
            Destroy(obj);
        }
    }

    void SpawnObjects()
    {
        points = PoissonDiscSamplingWithVariableDensity.GeneratePoints(minRadius, maxRadius, noiseScale, regionSize, rejectionSamples);
        foreach(PoissonDiscSamplingWithVariableDensity.Point point in points) {
            int idx = (int)Random.Range(0, prefabs.Count);
            GameObject obj = prefabs[idx];
            GameObject spawned = Instantiate(obj, new Vector3(point.x + regionOffset.x, obj.transform.position.y, point.y + regionOffset.y), obj.transform.rotation, spawnedParent);
            float randomScale = point.radius * 0.5f;
            spawned.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            spawned.SetActive(true);
        }
    }
}
