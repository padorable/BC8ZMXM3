using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiggingHandler : MonoBehaviour
{
    MapGeneratorRevamp mapGen;

    private void Start()
    {
        mapGen = FindObjectOfType<MapGeneratorRevamp>();
    }

    public void Dig(Vector3Int pos)
    {
        GridCell cell = mapGen.getCell((Vector2Int)pos);
        if (cell != null)
        {
            cell.Dig();
            mapGen.UpdateTile((Vector2Int)pos);
        }
    }

    //Tilemap dugLayer;
    //public TileBase DugTile;
    //MapGenerator mapGen;

    //private int digAmount = 0;
    //private List<int> modifiedWeights;
    //private List<Vector3Int> dugPoints = new List<Vector3Int>();
    //// Start is called before the first frame update
    //void Start()
    //{
    //    dugLayer = GameObject.FindGameObjectWithTag("DugLayer").GetComponent<Tilemap>();
    //    mapGen = FindObjectOfType<MapGenerator>();
    //}

    //public void Dig(Vector3Int pos)
    //{
    //    // Out of bounds
    //    if (pos.x < mapGen.LowerLeft.x || pos.y < mapGen.LowerLeft.y || pos.x > mapGen.LowerLeft.x + mapGen.ActualGridSize.x - 1 || pos.y > mapGen.LowerLeft.y + mapGen.ActualGridSize.y-1)
    //    {
    //        Debug.Log("Out");
    //        return;
    //    }
    //    AudioSystem.instance.PlaySFX(0);
    //    if (dugPoints.Contains(pos)) return;
    //    dugPoints.Add(pos);
    //    Debug.Log("Dug");

    //    if (mapGen.goal == pos) return;

    //    switch(mapGen.ItemGen.CheckTile(pos))
    //    {
    //        case 0: //None
    //            break;
    //        case 1:
    //            {
    //                int amount = Rarirty();
    //                GameManager.instance.AddItem(amount);
    //                MessagingSystem.instance.Message("Collected " + GameManager.instance.CurrentItems.Amount + "/" + GameManager.instance.ItemsToGet.Amount);
    //                break;
    //            }
    //        case 2: //Button
    //            AudioSystem.instance.PlaySFX(1,.5f);
    //            AudioSystem.instance.PlaySFX(4, .9f);
    //            MessagingSystem.instance.Message("You activated the switch!");
    //            mapGen.OpenOrb();
    //            break;
    //        case 3:
    //            mapGen.timer.AddTime(8);
    //            MessagingSystem.instance.Message("You gained more chakra!");
    //            break;
    //        case 4:
    //            MessagingSystem.instance.Message("Oh no a fucking trap!");
    //            break;
    //    }

    //    dugLayer.SetTile(pos, DugTile);
    //}

    //public int Rarirty()
    //{
    //    List<float> chance = new List<float>
    //    {
    //        10f + mapGen.Level,
    //        4 + ((float)mapGen.Level * 1.7f),
    //        2 + ((float)mapGen.Level * 1.5f)
    //    };
    //    float total = 0;
    //    foreach (float f in chance) total += f;
    //    float currentTotal = chance[0];
    //    float roll = Random.Range(0, 100f) / 100f;
    //    for (int i = 1; i < 3; i++)
    //    {
    //        if (roll <= currentTotal / total)
    //        {
    //            return i;
    //        }
    //        else if (chance.Count < i)
    //        {
    //            currentTotal += chance[i];
    //        }
    //    }
    //    return 1;
    //}
    //public void ResetDugPoints()
    //{
    //    dugPoints.Clear();
    //    dugLayer.ClearAllTiles();
    //}
}
