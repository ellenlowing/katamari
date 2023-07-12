using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public float radius = 1f;
    public float minRadius = 1f;
    public float maxRadius = 10f;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;
    // List<Vector2> points;
    List<PoissonDiscSamplingWithVariableDensity.Point> points;

    void OnValidate() {
        // points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        points = PoissonDiscSamplingWithVariableDensity.GeneratePoints(minRadius, maxRadius, Vector2.one, regionSize, rejectionSamples);
    }
    
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(regionSize/2, regionSize);
        if(points != null) {
            // foreach (Vector2 point in points) {
            //     Gizmos.DrawSphere(new Vector3(point.x, 0, point.y), displayRadius);
            // }
            foreach (PoissonDiscSamplingWithVariableDensity.Point point in points) {
                Gizmos.DrawSphere(new Vector3(point.x, point.y, 0f), point.radius * displayRadius);
            }
        }
    }
}
