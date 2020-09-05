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
    public TileBase Trap;
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

    public void GenerateItems(int geodAmount, int timeAmount, int trapAmount)
    {
        ItemCoordinates.Clear();

        // Creates a list of all the points available
        List<Vector3Int> allPoints = new List<Vector3Int>();
        for(int i = 0; i < mapGen.ActualGridSize.x; i++)
        {
            for(int j = 0; j < mapGen.ActualGridSize.y; j++)
            {
                allPoints.Add(mapGen.LowerLeft + new Vector3Int(i, j, 0));
            }
        }
        int ran = Random.Range(0, allPoints.Count);
        // From the lower left of the grid, finds a random point
        Vector3Int rand = allPoints[ran];
        mapGen.SetOrbPosition(rand);
        allPoints.RemoveAt(ran);

        rand = allPoints[ran];
        itemLayer.SetTile(allPoints[ran], Button);
        allPoints.RemoveAt(ran);

        for (int i = 0; i < geodAmount; i++)
        {
            if(allPoints.Count == 0) { Debug.LogError("GeodAmount too big"); break; }
            ran = Random.Range(0, allPoints.Count);
            itemLayer.SetTile(allPoints[ran], Geod);
            allPoints.RemoveAt(ran);
        }

        for(int i = 0; i < timeAmount; i++)
        {
            if (allPoints.Count == 0) { Debug.LogError("Not enough space for Time items"); break; }
            ran = Random.Range(0, allPoints.Count);
            itemLayer.SetTile(allPoints[ran], Time);
            allPoints.RemoveAt(ran);
        }

        for(int i = 0; i < trapAmount; i++)
        {
            if (allPoints.Count == 0) { Debug.LogError("Not enough space for trap items"); break; }
            ran = Random.Range(0, allPoints.Count);
            itemLayer.SetTile(allPoints[ran], Trap);
            allPoints.RemoveAt(ran);
        }
    }
    /*
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
    */

    // Tile Checker
    public int CheckTile(Vector3Int pos)
    {
        if (itemLayer.GetTile(pos) == Geod) return 1;
        else if (itemLayer.GetTile(pos) == Button) return 2;
        else if (itemLayer.GetTile(pos) == Time) return 3;
        else if (itemLayer.GetTile(pos) == Trap) return 4; 
            return 0;
    }

    public void ClearTiles()
    {
        itemLayer.ClearAllTiles();
    }
}