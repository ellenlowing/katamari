using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterObjectsSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 regionSize = Vector2.one;
    [SerializeField] private float radius = 1;
    [SerializeField] private int rejectionSamples = 30;
    [SerializeField] private Vector2 regionOffset = Vector2.zero;

    private List<Vector2> points;

    void Start()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        foreach(Vector2 point in points) {
            int idx = (int) Random.Range(0, transform.childCount);
            GameObject obj = transform.GetChild(idx).gameObject;
            GameObject spawned = Instantiate(obj, new Vector3(point.x + regionOffset.x, obj.transform.position.y, point.y + regionOffset.y), obj.transform.rotation, transform);
            float randomScale = Random.Range(0.1f, 0.5f);
            spawned.transform.localScale *= randomScale;
        }
    }
}
