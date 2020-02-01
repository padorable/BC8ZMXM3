using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public RuleTile tile;
    [SerializeField] private Vector2Int GridMinSize;
    [SerializeField] private Vector2Int GridMaxSize;
    [SerializeField] private int mod;
    [Space(10)]
    [Header("For Debugging")]
    public Vector2Int GridSize;
    public Tilemap tilemap;
    public Tilemap itemMap;
    public Sprite testSprite;
    public Tile itemTile;
    public Tile timeTile;
    
    public Vector3Int LowerLeft { get { return new Vector3Int(-GridSize.x / 2, -GridSize.y / 2 + 1, 0); } }

    // Start is called before the first frame update
    void Start()
    {
        //tilemap = FindObjectOfType<Tilemap>();
    }

    public void GenerateMap()
    {
        GridSize = new Vector2Int(Random.Range(GridMinSize.x, GridMaxSize.x) + mod, Random.Range(GridMinSize.y, GridMaxSize.y) + mod);
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

        for (int i = 0; i < 2 + Random.Range(0, 2); i++)
        {
            itemMap.SetTile(LowerLeft + new Vector3Int(Random.Range(0, GridSize.x), Random.Range(0, GridSize.y - 1), 0), itemTile);
        }
        for (int i = 0; i < 1 + Random.Range(0, 2); i++)
        {
            itemMap.SetTile(LowerLeft + new Vector3Int(Random.Range(0, GridSize.x), Random.Range(0, GridSize.y - 1), 0), timeTile);
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
        mod += 2;
        tilemap.ClearAllTiles();
        itemMap.ClearAllTiles();
    }
}
