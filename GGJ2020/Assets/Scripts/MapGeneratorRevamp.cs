using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneratorRevamp : MonoBehaviour
{
    //[SerializeField] private Vector2Int GridMinSize;
    //[SerializeField] private Vector2Int GridMaxSize;

    public Vector2Int GridSize;
    public Tilemap tilemap;
    public Tilemap objectTileMap;
    public RuleTile tile;
    public Vector2Int Offset;
    public List<GridCell> GridList = new List<GridCell>();

    [Header("Tiles")]
    [SerializeField] private Tile chakraTile;
    [SerializeField] private Tile exitTile;
    [SerializeField] private Tile trapTile;
    [SerializeField] public Tile dugTile;
    public List<GeodTiling> Geods;

    public GridCell getCell(Vector2Int pos)
    {
        Vector2Int actualPos = pos - Offset;
        if (actualPos.x < 0 || actualPos.x >= GridSize.x || actualPos.y < 0 || actualPos.y >= GridSize.y)
            return null;

        int x = (actualPos.y * GridSize.x) + actualPos.x;

        return GridList[x];
    }

    public void SetInitialTile(Vector2Int pos)
    {
        GridCell cell = getCell(pos);

        if (cell == null) return;

        switch (cell.Type)
        {
            case CellType.Geod:
                {
                    int index = Geods.FindIndex((x) => x.item == cell.rarity);
                        
                    if(index >= 0)
                    {
                        objectTileMap.SetTile(cell.WorldGridLocation, Geods[index].tile);
                    }
                    else
                    {
                        Debug.Log("random");
                        objectTileMap.SetTile(cell.WorldGridLocation, Geods[Random.Range(0, Geods.Count)].tile);
                    }

                    break;
                }
            default:
                {
                    if (chakraTile != null)
                        objectTileMap.SetTile(cell.WorldGridLocation, chakraTile);
                    break;
                }
        }
    }

    public void UpdateTile(Vector2Int pos)
    {
        GridCell cell = getCell(pos);

        if (cell.IsCovered) return;

        switch (cell.Type)
        {
            case CellType.Exit:
                {
                    break;
                }

            case CellType.Trap:
                {
                    if (trapTile != null)
                        objectTileMap.SetTile(cell.WorldGridLocation, trapTile);
                    break;
                }

            default:
                {
                    if(dugTile != null)
                        objectTileMap.SetTile(cell.WorldGridLocation, dugTile);
                    break;
                }
        }
    }

    public void GenerateMap()
    {
        GridList = new List<GridCell>(GridSize.x * GridSize.y);
        int count = 0;
        while (count < (GridSize.x + 2) * (GridSize.y + 2))
        {
            int x = count % (GridSize.x + 2);
            int y = Mathf.FloorToInt(count / (GridSize.x + 2));

            Vector3Int pos = new Vector3Int(x - 1, y - 1, 0) + (Vector3Int)Offset;
            tilemap.SetTile(pos, tile);
            if (x != 0 && x != GridSize.x + 1 && y != 0 && y != GridSize.y + 1)
            {
                GridList.Add(new GridCell(pos, new Vector2Int(y - 1, x - 1)));
            }
            count++;
        }
    }

    public Vector2Int MidPoint
    {
        get
        {
            return new Vector2Int(Mathf.FloorToInt((float)GridSize.x / 2f), Mathf.FloorToInt((float)GridSize.y / 2f));
        }

    }

    public void ClearTiles()
    {
        tilemap.ClearAllTiles();
        objectTileMap.ClearAllTiles();
    }
}

[System.Serializable]
public struct GeodTiling
{
    public Items item;
    public Tile tile;
}