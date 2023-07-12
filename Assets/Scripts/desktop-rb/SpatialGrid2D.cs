using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpatialGrid2D<T>
{
    public float Width {get; private set; }
    public float Height {get; private set; }
    public float CellLength {get; private set; }
    public int CellsPerX {get; private set; }
    public int CellsPerY {get; private set; }
    
    private List<SpatialCell> GridCells;

    private List<T> GridItems;

    public SpatialGrid2D(float width, float height, float minRadius, float maxRadius)
    {
        Width = width;
        Height = height;
        CellLength = ((minRadius + maxRadius) * 0.5f) / Mathf.Sqrt(2);
        CellsPerX = Mathf.CeilToInt(Width / CellLength);
        CellsPerY = Mathf.CeilToInt(Height / CellLength);

        int totalCells = CellsPerX * CellsPerY;

        GridCells = new List<SpatialCell>(totalCells);
        GridItems = new List<T>(totalCells);

        for(int y = 0; y < CellsPerY; y++) 
        {
            for(int x = 0; x < CellsPerX; x++)
            {
                GridCells.Add(new SpatialCell(x, y, CellLength));
            }
        }
    }

    private sealed class SpatialCell 
    {
        public int OffsetX;
        public int OffsetY;
        public float OriginX;
        public float OriginY;
        public float Dimension;

        public List<SpatialGridItem> Contents;

        public SpatialCell(int x, int y, float dimension) 
        {
            OffsetX = x;
            OffsetY = y;
            OriginX = ((float)OffsetX * dimension);
            OriginY = ((float)OffsetY * dimension);
            Dimension = dimension;
            Contents = new List<SpatialGridItem>();
        }
    }

    private sealed class SpatialGridItem
    {
        public int Index;
        public float X;
        public float Y;
        public float Radius;
    }
}