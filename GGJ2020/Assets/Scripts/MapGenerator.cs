using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public RuleTile tile;
    [SerializeField] private Vector2Int GridMinSize;
    [SerializeField] private Vector2Int GridMaxSize;
    [SerializeField] private int mod;
    public int Level = 1;
    public Tilemap tilemap;
    public RandomItemGenerator ItemGen;
    public Timer timer;
    [Space(10)]
    [Header("For Debugging")]
    public Vector2Int GridSize;
    public Vector2Int ActualGridSize { get { return new Vector2Int(GridSize.x - 2, GridSize.y - 2); } }
    private Player player;
    public Vector3Int goal;
    private bool GoalOut = false;
    public Vector3Int LowerLeft { get { return new Vector3Int(Mathf.CeilToInt((float)-GridSize.x / 2f) + 1, Mathf.CeilToInt((float)-GridSize.y / 2) + 1, 0); } }
    public GameObject Orb;

    private List<Vector2Int> gridSizes = new List<Vector2Int>
    {
        new Vector2Int(5,5),
        new Vector2Int(6,6),
        new Vector2Int(7,7),
        new Vector2Int(6,11),
        new Vector2Int(9,7),
        new Vector2Int(9,9)
    };

    private void Start()
    {
        DialogueSystem.instance.OnDialogueEnd.RemoveAllListeners();

        player = FindObjectOfType<Player>();
        GenerateMap();

        if (!GameManager.instance.HasEneterdOnce)
        {
            timer.isStart = false;
            GameManager.instance.HasEneterdOnce = true;
            DialogueSystem.instance.StartDialogue(5);
            player.enabled = false;
            DialogueSystem.instance.OnDialogueEnd.AddListener(() => { player.enabled = true; timer.isStart = true; });
        }

        AudioSystem.instance.PlayMusic(0);
        timer.OnEnd.AddListener(Return);
    }

    public void GenerateMap()
    {
        int index = Level;
        if (index >= gridSizes.Count) index = gridSizes.Count - 1;
        GridSize = gridSizes[index];

        SpawnTiles();
    }

    private void PlayerGoesUp()
    {
        Level += 1;
        GenerateMap();
    }

    private void PlayerGoesDown()
    {
        Level -= 1;
        GenerateMap();
    }
    public void GenerateMap(Vector2Int size)
    {
        ClearMap();
        ItemGen.ClearTiles();
        Level += 1;
        GridSize = size;
        SpawnTiles();

        int index = Mathf.Clamp(Mathf.FloorToInt(Random.Range(0, 100f) / 33f), 0, 2);
        //ItemGen.GenerateItems(percentList[index]);
        ItemGen.GenerateItems(1, 1, 1);
        RepositionPlayer();
        GoalOut = false;
    }

    public void SpawnTiles()
    {
        int count = 0;
        while (count < GridSize.x * GridSize.y)
        {
            int x = Mathf.CeilToInt((float)-GridSize.x / 2f) + (count % GridSize.x);
            int y = Mathf.CeilToInt((float)-GridSize.y / 2) + Mathf.FloorToInt(count / GridSize.x);

            Vector3Int pos = new Vector3Int(x, y, 0);
            tilemap.SetTile(pos, tile);
            count++;
        }

        //Tile emptyTile = new Tile
        //{
        //    sprite = null,
        //    colliderType = Tile.ColliderType.Grid
        //};
        //for (int i = 0; i < GridSize.x; i++)
        //{
        //    tilemap.SetTile(LowerLeft + new Vector3Int(i, GridSize.y, 0), emptyTile);
        //    tilemap.SetTile(LowerLeft + new Vector3Int(i, -1, 0), emptyTile);
        //}

        //for(int i = 0; i < GridSize.y; i++)
        //{
        //    tilemap.SetTile(LowerLeft + new Vector3Int(-1, i, 0), emptyTile);
        //    tilemap.SetTile(LowerLeft + new Vector3Int(GridSize.x,i, 0), emptyTile);
        //}
    }
    List<float> percentList = new List<float>
    {
        .1f,.2f,.3f
    };

    public void ClearMap()
    {
        mod += 2;
        tilemap.ClearAllTiles();
        Orb.GetComponentInChildren<Animator>().Play("Return");
    }

    public void OpenOrb()
    {
        if (GoalOut) return;
        Orb.GetComponentInChildren<Animator>().Play("Orb");
        GoalOut = true;
    }

    public void CheckIfOnOrb(Vector3Int pos)
    {
        if (!GoalOut) return;
        if ((pos-goal).magnitude <= .1f)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(FadeSystem.instance.Willfade(true, Color.black));
            seq.AppendCallback(()=>
            {
                PlayerGoesUp();
                RepositionPlayer();
                //player.GetComponent<DiggingHandler>().ResetDugPoints();
            });
            seq.Append(FadeSystem.instance.Willfade(false));
            seq.PlayForward();
        }
    }

    public void CheckIfonTrap(Vector3Int pos)
    {

    }

    public void SetOrbPosition(Vector3Int pos)
    {
        Orb.SetActive(true);
        Orb.transform.position = tilemap.GetCellCenterWorld(pos) + new Vector3(0,0,-5);
        goal = pos;
    }
    private void RepositionPlayer()
    {
        player.changePos(LowerLeft + new Vector3Int(Random.Range(0, ActualGridSize.x - 1), Random.Range(0, ActualGridSize.y - 1), 0));
    }

    private void Return()
    {
        SceneSystem.instance.FadeToNextScene("ReturnToDungeon");
    }
}
