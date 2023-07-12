using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterObjectsSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 regionSize = Vector2.one;
    [SerializeField] private float minRadius = 1;
    [SerializeField] private float maxRadius = 10;
    [SerializeField] private int rejectionSamples = 30;
    [SerializeField] private Vector2 regionOffset = Vector2.zero;

    private List<PoissonDiscSamplingWithVariableDensity.Point> points;

    void Start()
    {
        points = PoissonDiscSamplingWithVariableDensity.GeneratePoints(minRadius, maxRadius, regionSize, rejectionSamples);
        foreach(PoissonDiscSamplingWithVariableDensity.Point point in points) {
            int idx = (int) Random.Range(0, transform.childCount);
            GameObject obj = transform.GetChild(idx).gameObject;
            GameObject spawned = Instantiate(obj, new Vector3(point.x + regionOffset.x, obj.transform.position.y, point.y + regionOffset.y), obj.transform.rotation, transform);
            float randomScale = point.radius * 0.5f;
            spawned.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            spawned.SetActive(true);
        }
    }
}
