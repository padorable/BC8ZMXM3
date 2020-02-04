using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Items
{
    Gold,
    Copper,
    Iron,
    Silver,
    Diamond
}
public struct itemAmount
{
    public Items Item;
    public int Amount;
}

public class GameManager : MonoBehaviour
{
    public itemAmount ItemsToGet;
    public itemAmount CurrentItems;
    public int Progress = 0;
    public UnityEvent OnFinishGoal;
    
    public static GameManager instance;
    public bool HasEneterdOnce = false;
    private void Awake()
    {
        instance = this;
        ItemsToGet = new itemAmount
        {
            Item = Items.Diamond,
            Amount = 30
        };
    }

    public void AddItem(int amount)
    {
        CurrentItems.Amount += amount;
        if(CurrentItems.Amount >= ItemsToGet.Amount)
        {
            if(OnFinishGoal != null)
                OnFinishGoal.Invoke();

            SceneSystem.instance.FadeToNextScene("Ending");
        }
    }

    public void setGoal(Items item, int amount)
    {
        ItemsToGet = new itemAmount { Item = item, Amount = amount };
        CurrentItems = new itemAmount { Item = item, Amount = 0 };
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)) SceneSystem.instance.FadeToNextScene("Ending");
    }
}
