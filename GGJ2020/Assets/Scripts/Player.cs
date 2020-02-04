using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class StepOnTile : UnityEvent<Vector3Int> { }

public class Player : MonoBehaviour
{
    private Grid grid;
    private Vector3Int currentPos;
    private Vector3Int lookingAtPos;
    private Vector3Int direction;
    private bool isLooking = true;
    private bool stopMoving = false;
    [HideInInspector] public StepOnTile OnStepOnTile = new StepOnTile();
    //public bool debug = false;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        currentPos = grid.WorldToCell(this.transform.position);
        this.transform.position = grid.GetCellCenterWorld(currentPos);
        StartCoroutine(ControlMovement());
        isLooking = false;
        stopMoving = false;
    }

    IEnumerator ControlMovement()
    {
        float elapsedTime = 0;
        while(true)
        {
            float y = Input.GetAxisRaw("Vertical");
            float x = Input.GetAxisRaw("Horizontal");

            if ((x != 0 || y != 0) && !stopMoving)
            {
                if (x != 0 && y != 0) y = 0;

                direction = new Vector3Int(Mathf.CeilToInt(x), Mathf.CeilToInt(y),0);
                
                // Player has a delay before actually moving
                if (isLooking)
                {
                    elapsedTime += Time.deltaTime;
                    lookingAtPos = currentPos + new Vector3Int(Mathf.CeilToInt(x), Mathf.CeilToInt(y), 0);
                    // After .13f seconds of holding the movement button, player will start moving
                    if (elapsedTime >= .13f)
                        isLooking = false;
                    yield return null;
                }
                else
                {
                    Vector3Int pos = currentPos + direction;
                    // Used to check on the next block
                    lookingAtPos = pos + direction;

                    Vector2 nextPos = grid.GetCellCenterWorld(pos);

                    // Used to check if there are any colliders on the next block
                    Collider2D[] col = Physics2D.OverlapBoxAll(nextPos, new Vector2(.1f, .1f), 0);
                    if (col.Length == 0)
                    {
                        currentPos = pos;
                    }

                    //debug = false;
                    yield return new WaitUntil(()=>(((Vector2)(this.transform.position - grid.GetCellCenterWorld(currentPos))).magnitude <= .1f));
                    //debug = true;
                    Vector3 to = grid.GetCellCenterWorld(currentPos);
                    to.z = -1;
                    this.transform.position = to;

                    OnStepOnTile.Invoke(currentPos);
                }
            }
            else
            {
                elapsedTime = 0;
                isLooking = true;
                yield return null;
            }
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.GetComponent<DiggingHandler>().Dig(lookingAtPos);
        }

        GetComponent<Animator>().SetFloat("X", direction.x);
        GetComponent<Animator>().SetFloat("Y", direction.y);

        Vector3 to = grid.GetCellCenterWorld(currentPos);
        to.z = -1;
        this.transform.position += (to - this.transform.position).normalized * Time.deltaTime*5.5f;
    }

    public void changePos(Vector3Int pos)
    {
        lookingAtPos = pos + new Vector3Int(0,1,0);
        currentPos = pos;

        Vector3 to = grid.GetCellCenterWorld(currentPos);
        to.z = -1;
        this.transform.position = to;
    }
}
