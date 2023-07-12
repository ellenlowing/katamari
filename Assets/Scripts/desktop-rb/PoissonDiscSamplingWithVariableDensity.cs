using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSamplingWithVariableDensity
{

    public class Point {
        public float x, y, radius;
        public Point(float x, float y, float radius) {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }
    }

    public class Cell {
        public int x, y;
        public List<Point> points;
        public Cell(int x, int y) {
            this.x = x;
            this.y = y;
            points = new List<Point>();
        }
    }

    public static List<Point> GeneratePoints(float rmin, float rmax, Vector2 noiseScale, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30) {
        List<Cell> cells = new List<Cell>(); 
        float cellSize = rmax / Mathf.Sqrt(2);
        int gridSizeX = Mathf.CeilToInt(sampleRegionSize.x / cellSize);
        int gridSizeY = Mathf.CeilToInt(sampleRegionSize.y / cellSize);

        Cell tempCell = null;
        Point tempPoint = null;
        float px, py, dist, rsum;
        bool invalid = true;

        for(int y = 0; y < gridSizeY; y++) {
            for(int x = 0; x < gridSizeX; x++) {
                tempCell = new Cell(x, y);
                cells.Add(tempCell);
                px = x * rmax;
                py = y * rmax;
                for(int k = 0; k < numSamplesBeforeRejection; k++) {

                    if(invalid) {
                        float angle = Random.value * Mathf.PI * 2;
                        Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                        Vector2 pos = new Vector2(px + dir.x * Random.Range(0, rmax), py + dir.y * Random.Range(0, rmax));
                        float radius = Mathf.Lerp(rmin, rmax, Mathf.PerlinNoise(pos.x * noiseScale.x, pos.y * noiseScale.y));
                        tempPoint = new Point(pos.x, pos.y, radius);
                        invalid = false;
                    }
                    
                    // check if temp point is within sample region size
                    if(tempPoint.x < 0 || tempPoint.x > sampleRegionSize.x || tempPoint.y < 0 || tempPoint.y > sampleRegionSize.y) {
                        invalid = true;
                        continue; // move on with next random candidate
                    }

                    // check if temp point intersects with neighboring cell's existing points
                    int searchStartX = Mathf.Max(0, x - 2);
                    int searchStartY = Mathf.Max(0, y - 2);
                    int searchEndX = Mathf.Min(x + 2, gridSizeX);
                    int searchEndY = Mathf.Min(y + 2, gridSizeY);

                    for(int l = 0; l < cells.Count; l++) {
                        if(
                            cells[l].x >= searchStartX && 
                            cells[l].x <= searchEndX &&
                            cells[l].y >= searchStartY &&
                            cells[l].y <= searchEndY
                        ) {
                            for(int i = 0; i < cells[l].points.Count; i++) {
                                Point p = cells[l].points[i];
                                dist = Mathf.Sqrt(Mathf.Pow(p.x - tempPoint.x, 2) + Mathf.Pow(p.y - tempPoint.y, 2));
                                rsum = p.radius + tempPoint.radius;
                                if(dist < rsum) {
                                    invalid = true;
                                }
                            }
                            if(invalid) break;
                        }
                    }

                    if(!invalid) tempCell.points.Add(tempPoint);
                    invalid = true;
                    
                }
            }
        }
        return cells.SelectMany(R => R.points).ToList();
    }
    
}
