using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Was to be used on the scene selection, which we had no time to do

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
