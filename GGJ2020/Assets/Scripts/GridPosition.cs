using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridPosition : MonoBehaviour
{
    private Grid grid;
    private bool IsPressing = false;
    Vector3Int currentPos;
    Vector3Int LookingAtPos;
    bool isLooking = true;
    // Update is called once per frame

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        currentPos = grid.WorldToCell(this.transform.position);
        this.transform.position = grid.GetCellCenterWorld(currentPos);
        StartCoroutine(ControlMovement());

    }
    IEnumerator ControlMovement()
    {
        float elapsedTime = 0;
        while(true)
        {
            float y = Input.GetAxisRaw("Vertical");
            float x = Input.GetAxisRaw("Horizontal");

            if ((x != 0 || y != 0))
            {
                if (x != 0 && y != 0) y = 0;

                if (isLooking)
                {
                    elapsedTime += Time.deltaTime;
                    LookingAtPos = currentPos + new Vector3Int(Mathf.CeilToInt(x), Mathf.CeilToInt(y), 0);
                    if (elapsedTime >= .05f)
                        isLooking = false;
                    yield return null;
                }
                else
                {
                    Vector3Int direction = new Vector3Int(Mathf.CeilToInt(x), Mathf.CeilToInt(y), 0);
                    Vector3Int pos = currentPos + direction;
                    LookingAtPos = currentPos + direction;

                    Vector2 nextPos = grid.GetCellCenterWorld(pos);

                    Collider2D[] col = Physics2D.OverlapBoxAll(nextPos, new Vector2(.1f, .1f), 0);

                    if (col.Length == 0)
                    {
                        currentPos = pos;
                    }

                    yield return new WaitForSeconds(.25f);
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
        Vector3 to = grid.GetCellCenterWorld(currentPos);
        to.z = -2;
        this.transform.position = Vector3.Lerp(this.transform.position, to, Time.deltaTime * 10);
    }
}
