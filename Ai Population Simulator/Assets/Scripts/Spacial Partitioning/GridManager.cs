using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager {
    private GridPartition[,] partitions;
    private int _width;
    private int _height;

    //Create our paritions
    public GridManager(int width, int height) {
        partitions = new GridPartition[height, width];
        _width = height;
        _height = width;
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                partitions[x, y] = new GridPartition(new Vector2Int(x, y));
            }
        }
    }
    //Get a parition in a location
    public GridPartition GetPartition(Vector2Int location) {
        if (withinBounds(location)) {
            return partitions[location.x, location.y];
        }
        return null;
    }
    //Get neighbours around this point
    public Vector2Int[] GetNeighbours(Vector2Int point) {
        Vector2Int[] points = new Vector2Int[] {
            new Vector2Int(point.x - 1, point.y),
            new Vector2Int(point.x + 1, point.y),
            new Vector2Int(point.x, point.y - 1),
            new Vector2Int(point.x, point.y + 1),
            new Vector2Int(point.x - 1, point.y - 1),
            new Vector2Int(point.x - 1, point.y + 1),
            new Vector2Int(point.x + 1, point.y - 1),
            new Vector2Int(point.x + 1, point.y + 1)
        };
        return points;
    }
    //Get neighbour paritions as a grid partition
    public GridPartition[] GetNeighboursPartitions(Vector2Int point) {
        GridPartition[] points = new GridPartition[] {
            GetPartition(new Vector2Int(point.x - 1, point.y)),
            GetPartition(new Vector2Int(point.x + 1, point.y)),
            GetPartition(new Vector2Int(point.x, point.y - 1)),
            GetPartition(new Vector2Int(point.x, point.y + 1)),
            GetPartition(new Vector2Int(point.x - 1, point.y - 1)),
            GetPartition(new Vector2Int(point.x - 1, point.y + 1)),
            GetPartition(new Vector2Int(point.x + 1, point.y - 1)),
            GetPartition(new Vector2Int(point.x + 1, point.y + 1))
        };
        return points;
    }
    //Draw a grid parition at a location
    public void DrawGizmos(Vector2Int location, Color? col = null) { 
        if (withinBounds(location)) {
            partitions[location.x, location.y].DrawGizmo(col);
        }
    }

    //Is our sample within bounds?
    private bool withinBounds(Vector2Int location) {
        if (location.x >= 0 & location.x < _width) {
            if (location.y >= 0 & location.y < _height) {
                return true;
            }
        }

        return false;
    }
    //Remove a agent to location
    public void RemoveAgent(Vector2Int positition, AgentData data) {
        if (withinBounds(positition)) {
            partitions[positition.x, positition.y].agents.Remove(data);
        }
    }
    //Add a agent to location
    public void AddAgent(Vector2Int positition, AgentData data) {
        if (withinBounds(positition)) {
            partitions[positition.x, positition.y].agents.Add(data);
        }
    }
    //Draw every parition gizmos
    public void DrawGizmos() {
        foreach (var item in partitions) {
            item.DrawGizmo();
        }
    }
    //Set water
    public void SetWater(Vector2Int point) {
        //Set mid as unwalkable
        if (withinBounds(point)) {
            partitions[point.x, point.y].walkable = false;
            partitions[point.x, point.y].drinkable = false;
        }
        //Get neighbours as drinkable
        Vector2Int[] points = GetNeighbours(point);
        foreach (var item in points) {
            if (withinBounds(item)) {
                if (partitions[item.x, item.y].walkable == true) {
                    partitions[item.x, item.y].drinkable = true;
                }

            }
        }
    }
    //Get paritions in a radius as a array
    public Vector2Int[] GetPartitionsInRadius(Vector2 point, int radius) {
        List<Vector2Int> OutputIndex = new List<Vector2Int>();

        int threshold = radius * radius;
        for (int i = -radius; i < radius; i++) {
            for (int j = -radius; j < radius; j++) {
                if (i * i + j * j < threshold)
                    OutputIndex.Add(new Vector2Int((int)point.x + i, (int)point.y + j));
            }
        }

        return OutputIndex.ToArray();
    }
    //Increase the camo at the parition
    public void SetCamoflauge(Vector2Int point, int radius) {

        int threshold = radius * radius;
        for (int i = -radius; i < radius; i++) {
            for (int j = -radius; j < radius; j++) {
                Vector2Int pos = new Vector2Int((int)point.x + i, (int)point.y + j);
                if (withinBounds(pos) & pos != point) {
                    if (i * i + j * j < threshold)
                        partitions[pos.x, pos.y].CamoflaugeLevel += radius - Vector2.Distance(new Vector2(i, j), Vector2.zero);
                }

            }
        }

    }
    //Convert our camo to a textures
    public Texture2D CamoflaugeToTexture() {
        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                float col = partitions[x, y].CamoflaugeLevel / 2;
                texture.SetPixel(x, y, new Color(col, col, col));
            }
        }
        texture.Apply();

        return texture;
    }
    //Convert our water to a texture
    public Texture2D WaterToTexture() {
        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                float col = partitions[x, y].drinkable == true ? 1 : 0;
                texture.SetPixel(x, y, new Color(col, col, col));
            }
        }
        texture.Apply();

        return texture;
    }
    //Convert our walkable areas to a texture
    public Texture2D WalkableToTexture() {
        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                float col = partitions[x, y].walkable == true ? 1 : 0;
                texture.SetPixel(x, y, new Color(col, col, col));
            }
        }
        texture.Apply();

        return texture;
    }
    //Convert our food to a texture
    public Texture2D FoodToTexture() {
        Texture2D texture = new Texture2D(_width, _height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                float col = partitions[x, y].containsFood == true ? 1 : 0;
                texture.SetPixel(x, y, new Color(col, col, col));
            }
        }
        texture.Apply();

        return texture;
    }
    //Get paritions in a cone
    public Vector2Int[] GetPartitionsInCone(Vector2Int point, int radius, Vector3 dir, float size = 45) {
        List<Vector2Int> OutputIndex = new List<Vector2Int>();

        Vector2Int[] i = GetPartitionsInRadius(point, radius);

        foreach (var sampledPoint in i) {
            Vector3 to = new Vector3(point.x, 0, point.y);
            Vector3 from = new Vector3(sampledPoint.x, 0, sampledPoint.y);
            Vector3 lookDirection = from - to;
            float dot = Vector3.Dot(lookDirection, dir);
            if (Vector3.Angle(lookDirection, dir) <= size) {
                if (withinBounds(sampledPoint)) {
                    OutputIndex.Add(sampledPoint);
                }

            }
        }

        return OutputIndex.ToArray();
    }
    //Set the food in a location
    public bool SetFood(Vector2Int location, Food type, FoodSource foodSource) {
        if (withinBounds(location)) {
            if (!partitions[location.x, location.y].containsFood) {
                partitions[location.x, location.y].food = type;
                partitions[location.x, location.y].foodSource = foodSource;
                return true;
            }
        }
        return false;
    } 
}
