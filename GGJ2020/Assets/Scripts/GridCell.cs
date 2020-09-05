using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CellType
{
    Normal,
    Exit,
    Trap,
    Geod,
    Chakra,
    Button
}

public class GridCell 
{
    public Vector3Int WorldGridLocation { get; private set; }
    public Vector2Int ActualGridLocation { get; private set; }
    public CellType Type;
    public bool IsCovered = true;
    public UnityEvent OnDug;
    public UnityEvent OnStepped;
    public Items rarity;

    public GridCell(Vector3Int pos, Vector2Int actualPos)
    {
        WorldGridLocation = pos;
        ActualGridLocation = actualPos;
    }

    public void SetType(CellType type)
    {
        Type = type;
    }

    public void Dig()
    {
        if (!IsCovered) return;

        IsCovered = false;

        if (OnDug != null)
            OnDug.Invoke();
    }
}
