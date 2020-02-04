using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = GetPos(Player);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, GetPos(Player), 1.0f);
    }

    private Vector3 GetPos(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        pos.z = this.transform.position.z;
        return pos;
    }
}
