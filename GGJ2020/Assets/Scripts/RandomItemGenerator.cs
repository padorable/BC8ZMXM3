using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Handles the spawning of Chakra, Ores, and position of the Orb

public class RandomItemGenerator : MonoBehaviour
{
    public List<Vector3Int> ItemCoordinates = new List<Vector3Int>();
    public Tilemap itemLayer;
    public TileBase Geod;
    public TileBase Button;
    public TileBase Time;
    MapGenerator mapGen;

    private Vector3Int RandPointFromGrid(MapGenerator map)
    {
        if (map == null) return Vector3Int.zero;

        return mapGen.LowerLeft + new Vector3Int(Random.Range(0, mapGen.ActualGridSize.x), Random.Range(0, mapGen.ActualGridSize.y), 0);
    }

    private void Start()
    {
        mapGen = FindObjectOfType<MapGenerator>();
    }

    public void GenerateItems(float percent)
    {
        float clampPercent = Mathf.Clamp(percent, 0.001f, 0.49f);

        ItemCoordinates.Clear();

        // From the lower left of the grid, finds a random point
        Vector3Int rand = RandPointFromGrid(mapGen);
        ItemCoordinates.Add(rand);
        mapGen.SetOrbPosition(rand);

        //
        int amountOfItems = Mathf.CeilToInt((mapGen.ActualGridSize.x * (mapGen.ActualGridSize.y)) * clampPercent);
        Debug.Log(amountOfItems);
        
        // spawns button as well because lazy
        for (int i = 0; i <= amountOfItems; i++)
        {
            // Loops until there is a valid point that isn't chosen yet
            Vector3Int randomPoint = Vector3Int.zero;
            do
            {
                randomPoint = RandPointFromGrid(mapGen);
            } while (ItemCoordinates.Contains(randomPoint));

            ItemCoordinates.Add(randomPoint);
            if(amountOfItems == i)
            {
                itemLayer.SetTile(randomPoint, Button);
            }
            else
                itemLayer.SetTile(randomPoint, Geod);
        }

        amountOfItems = Mathf.RoundToInt((mapGen.ActualGridSize.x * (mapGen.ActualGridSize.y)) * clampPercent);
        {
            for (int i = 0; i < amountOfItems; i++)
            {
                Vector3Int randomPoint = Vector3Int.zero;
                do
                {
                    randomPoint = RandPointFromGrid(mapGen);
                } while (ItemCoordinates.Contains(randomPoint));

                ItemCoordinates.Add(randomPoint);
                itemLayer.SetTile(randomPoint, Time);
            }
        }
    }

    // Tile Checker
    public int CheckTile(Vector3Int pos)
    {
        if (itemLayer.GetTile(pos) == Geod) return 1;
        else if (itemLayer.GetTile(pos) == Button) return 2;
        else if (itemLayer.GetTile(pos) == Time) return 3;
            return 0;
    }

    public void ClearTiles()
    {
        itemLayer.ClearAllTiles();
    }
}