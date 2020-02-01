using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public RuleTile tile;
    [SerializeField] private Vector2Int GridMinSize;
    [SerializeField] private Vector2Int GridMaxSize;
    [Space(10)]
    [Header("For Debugging")]
    public Vector2Int GridSize;
    Tilemap tilemap;
    public Sprite testSprite;
    
    public Vector3Int LowerLeft { get { return new Vector3Int(-GridSize.x / 2, -GridSize.y / 2 + 1, 0); } }

    // Start is called before the first frame update
    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
    }

    public void GenerateMap()
    {
        GridSize = new Vector2Int(Random.Range(GridMinSize.x, GridMaxSize.x), Random.Range(GridMinSize.y, GridMaxSize.y));
        if (GridSize.x % 2 == 1) GridSize.x += 1;
        if (GridSize.y % 2 == 1) GridSize.y += 1;

        SpawnTiles();
    }

    public void GenerateMap(Vector2Int size)
    {
        GridSize = size;
        SpawnTiles();
    }

    public void SpawnTiles()
    {
        int count = 0;
        while (count < GridSize.x * GridSize.y)
        {
            int x = -GridSize.x / 2 + (count % GridSize.x);
            int y = -GridSize.y / 2 + Mathf.FloorToInt(count / GridSize.x);

            Vector3Int pos = new Vector3Int(x, y, 0);
            tilemap.SetTile(pos, tile);
            count++;
        }

        Tile emptyTile = new Tile
        {
            sprite = null,
            colliderType = Tile.ColliderType.Grid
        };
        for (int i = 0; i < GridSize.x; i++)
        {
            tilemap.SetTile(LowerLeft + new Vector3Int(i, GridSize.y-1, 0), emptyTile);
        }

        for(int i = 0; i < GridSize.y; i++)
        {
            tilemap.SetTile(LowerLeft + new Vector3Int(-1, i, 0), emptyTile);
            tilemap.SetTile(LowerLeft + new Vector3Int(GridSize.x,i, 0), emptyTile);
        }
    }

    public void ClearMap()
    {
        tilemap.ClearAllTiles();
    }
}
