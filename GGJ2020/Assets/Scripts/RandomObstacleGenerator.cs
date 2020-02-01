using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomObstacleGenerator : MonoBehaviour
{
    Vector3Int size;
    Grid grid;

    public GameObject TestObstacle;
    public Transform Obstalces;
    [SerializeField, Range(0, 100)] int amountOfObstacles;

    List<GameObject> SpawnedObjects = new List<GameObject>();

    RandomItemGenerator itemGen;
    MapGenerator mapGen;
    private void Start()
    {
        grid = transform.parent.GetComponent<Grid>();
        mapGen = FindObjectOfType<MapGenerator>();
        itemGen = FindObjectOfType<RandomItemGenerator>();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mapGen.ClearMap();
            mapGen.GenerateMap();
            ClearObstacles();
            RandomGenerate();
            itemGen.GenerateItems();
        }
    }
    public void RandomGenerate()
    {
        List<Vector3Int> points = new List<Vector3Int>();

        for(int i = 0; i < amountOfObstacles; i++)
        {
            Vector3Int randomPoint = Vector3Int.zero;
            do
            {
                randomPoint = mapGen.LowerLeft + new Vector3Int(Random.Range(0, mapGen.GridSize.x), Random.Range(0, mapGen.GridSize.y - 1), 0);
            } while (points.Contains(randomPoint));

            points.Add(randomPoint);
            GameObject obj = Instantiate(TestObstacle, grid.GetCellCenterWorld(randomPoint) + new Vector3(0,0,-1), Quaternion.identity, Obstalces);
            SpawnedObjects.Add(obj);
        }
    }

    public void ClearObstacles()
    {
        for(int i = SpawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(SpawnedObjects[i]);
            SpawnedObjects.RemoveAt(i);
        }
        SpawnedObjects.Clear();
    }
}
