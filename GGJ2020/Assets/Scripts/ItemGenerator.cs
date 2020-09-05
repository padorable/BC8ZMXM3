using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class ItemGenerator : MonoBehaviour
{
    [Header("Item Weights")]
    [Range(0, 100)] public float OccupyPercent;
    [Range(0, 100)] public int Geod;
    [Range(0, 100)] public int Trap;
    [Range(0, 100)] public int Chakra;
    public List<ItemWeight> CurrentItemWeights;

    public GameObject Orb;
    public MapGeneratorRevamp mapGen;
    [HideInInspector] public UnityEvent NextLevel;
    [HideInInspector] public UnityEvent PreviousLevel;
    [HideInInspector] public UnityEvent PickUpChakra;

    public void SetPercentages(FloorSetting setting)
    {
        OccupyPercent = setting.OccupyPercent;
        Geod = setting.Geod;
        Trap = setting.Trap;
        Chakra = setting.Chakra;
        CurrentItemWeights = setting.ItemWeights;
    }

    public void SetItems()
    {
        Orb.GetComponentInChildren<Animator>().Play("Return");

        int total = Geod + Trap + Chakra;
        List<GridCell> cells = new List<GridCell>(mapGen.GridList);

        // Setting Button
        int rand = Random.Range(0, cells.Count);
        GridCell button = cells[rand];
        button.SetType(CellType.Button);
        button.OnDug = new UnityEvent();
        button.OnDug.AddListener(() => 
        {
            Orb.GetComponentInChildren<Animator>().Play("Orb");
            if(MessagingSystem.instance != null)
                MessagingSystem.instance.Message("You activated the switch!");

        });
        Debug.Log("Button: " + button.ActualGridLocation);
        mapGen.SetInitialTile((Vector2Int)cells[rand].WorldGridLocation);
        cells.RemoveAt(rand);

        // Setting Exit
        rand = Random.Range(0, cells.Count);
        GridCell exit = cells[rand];
        exit.SetType(CellType.Exit);
        exit.OnStepped = new UnityEvent();
        exit.OnStepped.AddListener(() =>
        {
            if (!button.IsCovered)
            {
                Debug.Log("Go To Next Level");
                NextLevel.Invoke();
            }
        });
        Orb.transform.position = mapGen.tilemap.GetCellCenterWorld(cells[rand].WorldGridLocation) + new Vector3(0,0,-5);
        cells.RemoveAt(rand);

        //GEOD
        int geodAmount = Mathf.FloorToInt(mapGen.GridList.Count * ((float)Geod / (float)total) * (OccupyPercent / 100f));
        setGeodCell(cells, geodAmount);
        
        //TRAPS
        int trapAmount = Mathf.FloorToInt(mapGen.GridList.Count * ((float)Trap / (float)total) * (OccupyPercent / 100f));
        setTrapCell(cells, trapAmount);

        //CHAKRA
        int chakraAmount = Mathf.FloorToInt(mapGen.GridList.Count * ((float)Chakra / (float)total) * (OccupyPercent / 100f));
        SetCells(cells, chakraAmount, CellType.Chakra, () =>
        {
            if (PickUpChakra != null)
                PickUpChakra.Invoke();

            if (MessagingSystem.instance != null)
                MessagingSystem.instance.Message("You got Chakra!");

        }, null);

        Debug.Log("Exit: 1\nButton: 1\nGeod: " + geodAmount + "\nTrap: " + trapAmount + "\nChakra: " + chakraAmount);
    }

    private void setTrapCell(List<GridCell> cells, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(0, cells.Count);
            GridCell cell = cells[rand];

            cells[rand].SetType(CellType.Trap);
            cells[rand].OnStepped = new UnityEvent();
            cells[rand].OnStepped.AddListener(()=> 
            {
                if(!cell.IsCovered)
                {
                    Debug.Log("Activated Trap");
                    if (PreviousLevel != null)
                        PreviousLevel.Invoke();
                }
            });

            cells[rand].OnDug = new UnityEvent();
            cells[rand].OnDug.AddListener(() =>
            {
                if(MessagingSystem.instance != null)
                {
                    MessagingSystem.instance.Message("You dug a trap! Be careful!");
                }
            });

            cells.RemoveAt(rand);
        }
    }

    private void setGeodCell(List<GridCell> cells, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(0, cells.Count);

            cells[rand].SetType(CellType.Geod);
            cells[rand].OnDug = new UnityEvent();
            Items item = getRandomItem();
            cells[rand].rarity = item;
            mapGen.SetInitialTile((Vector2Int)cells[rand].WorldGridLocation);

            cells[rand].OnDug.AddListener(() =>
            {
                if(item == Items.None)
                {
                    if(MessagingSystem.instance != null)
                        MessagingSystem.instance.Message("You got jack shit bruh");
                }
                else
                {
                    if(GameManager.instance != null)
                    {
                        bool a = GameManager.instance.AddItem(item, 1);
                        if (a)
                            MessagingSystem.instance.Message("Collected " + GameManager.instance.GetItemAmount(item, true).Amount + "/" + GameManager.instance.GetItemAmount(item, false).Amount);
                        else
                            Debug.Log("Bitch you aren't even suppose to pick that up");
                    }
                }
            });

            cells.RemoveAt(rand);
        }
    }

    private void SetCells(List<GridCell> cells, int amount, CellType type, UnityAction onDug, UnityAction onStep)
    {
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(0, cells.Count);

            cells[rand].SetType(type);
            mapGen.SetInitialTile((Vector2Int)cells[rand].WorldGridLocation);
            if(onDug != null)
            {
                cells[rand].OnDug = new UnityEvent();
                cells[rand].OnDug.AddListener(onDug);
            }

            if(onStep != null)
            {
                cells[rand].OnStepped = new UnityEvent();
                cells[rand].OnStepped.AddListener(onStep);
            }

            cells.RemoveAt(rand);
        }
    }

    private Items getRandomItem()
    {
        float total = 0;
        foreach (ItemWeight item in CurrentItemWeights) total += item.weight;

        float rand = Random.Range(0f, 100f) / 100f;

        float acc = 0;
        foreach (ItemWeight item in CurrentItemWeights)
        {
            if (rand <= ((item.weight + acc) / total))
                return item.item;
        }

        return global::Items.None;
    }
}
