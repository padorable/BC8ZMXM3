using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomItemGenerator : MonoBehaviour
{
    [SerializeField] List<Vector3Int> ItemCoordinates = new List<Vector3Int>();

    MapGenerator mapGen;
    private void Start()
    {
        mapGen = FindObjectOfType<MapGenerator>();
    }

    private void Update()
    {

    }

    public void GenerateItems()
    {
        int amountOfItems = Mathf.RoundToInt((mapGen.GridSize.x * (mapGen.GridSize.y - 1)) * 0.3f);
        for (int i = 0; i < amountOfItems; i++)
        {
            Vector3Int randomPoint = Vector3Int.zero;
            do
            {
                randomPoint = mapGen.LowerLeft + new Vector3Int(Random.Range(0, mapGen.GridSize.x), Random.Range(0, mapGen.GridSize.y - 1), 0);
            } while (ItemCoordinates.Contains(randomPoint));

            ItemCoordinates.Add(randomPoint);
        }
    }
}