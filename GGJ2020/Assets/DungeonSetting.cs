using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "DungeonSetting")]
public class DungeonSetting : ScriptableObject
{
    public RuleTile RuleTileSetting;
    public Tile DigTile;
    public float MaxTime = 80;
    [Tooltip("Amount of seconds to add when to the timer when dug")]
    public float ChakraValue = 10;

    [Tooltip("Required amount of items needed to finish the dungeon")]
    public List<itemAmount> Goal;
    [Space(height: 5)]
    [SerializeField]public List<FloorSetting> Floors;
}

[System.Serializable]
public struct FloorSetting
{
    public Vector2Int LevelSize;
    [Range(0,100)] public float OccupyPercent;
    [Range(0, 100)] public int Geod;
    [Range(0, 100)] public int Trap;
    [Range(0, 100)] public int Chakra;
    public List<ItemWeight> ItemWeights;
}

[System.Serializable]
public struct ItemWeight
{
    public Items item;
    public float weight;
}