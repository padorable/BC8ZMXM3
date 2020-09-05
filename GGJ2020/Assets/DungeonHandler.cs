using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonHandler : MonoBehaviour
{
    private MapGeneratorRevamp mapGen;
    private ItemGenerator itemGen;
    private Player player;
    private Timer timer;

    public static DungeonHandler instance;
    [HideInInspector] public DungeonSetting CurrentSetting;

    public int currentFloor { get; private set; } = 0;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            mapGen = this.GetComponent<MapGeneratorRevamp>();
            itemGen = this.GetComponent<ItemGenerator>();
            player = FindObjectOfType<Player>();
            timer = this.GetComponent<Timer>();
            itemGen.NextLevel = new UnityEngine.Events.UnityEvent();
            itemGen.NextLevel.AddListener(Nextlevel);
            itemGen.PreviousLevel = new UnityEngine.Events.UnityEvent();
            itemGen.PreviousLevel.AddListener(PreviousLevel);
            itemGen.PickUpChakra = new UnityEngine.Events.UnityEvent();

            if(AudioSystem.instance != null)
                AudioSystem.instance.PlayMusic(0);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        GameManager manager = GameManager.instance;
        if (manager == null)
            Debug.LogError("Missing GameManager");
        else
            CurrentSetting = manager.CurrentSetting;

        // Incase CurrentSetting is null, create a default setting
        if (CurrentSetting == null)
        {
            CurrentSetting = new DungeonSetting();
            CurrentSetting.Goal = new List<itemAmount>
            {
                new itemAmount
                {
                    Item = Items.Copper,
                    Amount = 30
                }
            };
            CurrentSetting.Floors = new List<FloorSetting>
                {
                    new FloorSetting
                    {
                        LevelSize = new Vector2Int(5,5),
                        OccupyPercent = 50,
                        Chakra = 50,
                        Trap = 0,
                        Geod = 50
                    }
                };
            CurrentSetting.MaxTime = 80;
            CurrentSetting.ChakraValue = 10;
        }

        if (manager != null) manager.setGoal(CurrentSetting.Goal);

        // Setting tile settings
        if (CurrentSetting.RuleTileSetting != null)
            mapGen.tile = CurrentSetting.RuleTileSetting;
        if (CurrentSetting.DigTile != null)
            mapGen.dugTile = CurrentSetting.DigTile;

        // Setting timer
        itemGen.PickUpChakra.AddListener(() => timer.AddTime(CurrentSetting.ChakraValue));
        timer.timeMax = CurrentSetting.MaxTime;

        currentFloor = 0;
        SetLevel();
        timer.StartTimer();
    }

    public void SetLevel()
    {
        mapGen.ClearTiles();
        mapGen.GridSize = CurrentSetting.Floors[currentFloor].LevelSize;
        itemGen.SetPercentages(CurrentSetting.Floors[currentFloor]);
        mapGen.GenerateMap();
        itemGen.SetItems();
        Debug.Log(mapGen.MidPoint + " | " + mapGen.getCell(mapGen.MidPoint).WorldGridLocation);
        player.changePos(mapGen.getCell(mapGen.MidPoint).WorldGridLocation);
        //player.changePos(new Vector3Int(0, 0, 0));
    }

    private void SetFloor(int lvl)
    {
        currentFloor = lvl;
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => player.enableMoving(false));
        if (FadeSystem.instance != null)
            seq.Append(FadeSystem.instance.Willfade(true));
        seq.AppendCallback(() => SetLevel());
        if (FadeSystem.instance != null)
            seq.Append(FadeSystem.instance.Willfade(false));
        seq.AppendCallback(() => player.enableMoving(true));
        seq.Play();
    }

    public void Nextlevel()
    {
        SetFloor(Mathf.Min(currentFloor + 1, CurrentSetting.Floors.Count - 1));
    }

    public void PreviousLevel()
    {
        SetFloor(Mathf.Max(currentFloor - 1, 0));
    }
}
