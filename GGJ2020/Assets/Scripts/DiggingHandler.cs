using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Items
{
    Silver,
    Gold,
    Copper,
    Glass
}

public class DiggingHandler : MonoBehaviour
{
    Tilemap dugLayer;
    public TileBase DugTile;
    MapGenerator gen;
    public List<Items> ToDig = new List<Items>();

    // Start is called before the first frame update
    void Start()
    {
        dugLayer = GameObject.FindGameObjectWithTag("DugLayer").GetComponent<Tilemap>();
        gen = FindObjectOfType<MapGenerator>();
    }
    
    public void Dig(Vector3Int pos)
    {
        //if (gen.List.Contains(pos))
        //{
        //    if (Random.Range(0, 100f) / 100f < .5)
        //    {

        //    }
            dugLayer.SetTile(pos, DugTile);
        //}
    }
}
