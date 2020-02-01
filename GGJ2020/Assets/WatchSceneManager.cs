using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WatchSceneManager : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneSystem.instance.FadeToNextScene(1,Color.white);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
