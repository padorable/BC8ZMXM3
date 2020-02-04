using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Was not used because there aren't obstacles at all lmao

public class RandomObstacleGenerator : MonoBehaviour
{
    Vector3Int size;
    Grid grid;

    public GameObject TestObstacle;
    public Transform Obstalces;
    [SerializeField, Range(0, 100)] int amountOfObstacles;

    List<GameObject> SpawnedObjects = new List<GameObject>();

    RandomItemGenerator itemGen;

    public List<Vector3Int> obstaclePoints = new List<Vector3Int>();
    private void Start()
    {
        grid = transform.parent.GetComponent<Grid>();
        itemGen = FindObjectOfType<RandomItemGenerator>();

    }

    public void RandomGenerate(MapGenerator gen)
    {
        obstaclePoints.Clear();

        for(int i = 0; i < amountOfObstacles; i++)
        {
            Vector3Int randomPoint = Vector3Int.zero;
            do
            {
                randomPoint = gen.LowerLeft + new Vector3Int(Random.Range(0, gen.ActualGridSize.x), Random.Range(0, gen.ActualGridSize.y - 1), 0);
            } while (obstaclePoints.Contains(randomPoint));

            obstaclePoints.Add(randomPoint);
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
        obstaclePoints.Clear();
    }
}
