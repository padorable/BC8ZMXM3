using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum Items
{
    Copper,
    Iron,
    Silver,
    Gold,
    Diamond,
    None
}

[System.Serializable]
public struct itemAmount
{
    public Items Item;
    public int Amount;
}

public class GameManager : MonoBehaviour
{
    public List<itemAmount> ItemsToGet;
    public List<itemAmount> CurrentItems;
    public int Progress = 0;
    public UnityEvent OnFinishGoal;
    
    public static GameManager instance;
    public bool HasEneterdOnce = false;
    public DungeonSetting CurrentSetting;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        setGoal(new List<itemAmount>
        {
            new itemAmount
            {
                Item = Items.Diamond,
                Amount = 10
            }
        });
    }

    public bool AddItem(Items item, int amount)
    {
        int x = CurrentItems.FindIndex((a) => a.Item == item);

        if(x < 0)
        {
            CurrentItems.Add(new itemAmount { Item = item, Amount = 1 });
        }
        else
        {
            int newAmount = CurrentItems[x].Amount + amount;
            CurrentItems[x] = new itemAmount { Amount = newAmount, Item = CurrentItems[x].Item };

            // Check if done
            bool isDone = true;
            foreach (itemAmount it in ItemsToGet)
            {
                int b = CurrentItems.FindIndex((a) => a.Item == it.Item);

                if (b < 0)
                {
                    isDone = false;
                    break;
                }

                if (CurrentItems[b].Amount < it.Amount)
                {
                    isDone = false;
                    break;
                }
            }

            if (isDone)
            {
                if (OnFinishGoal != null)
                    OnFinishGoal.Invoke();

                if(SceneSystem.instance != null)
                    SceneSystem.instance.FadeToNextScene("Ending");
            }
        }

        return ItemsToGet.FindIndex((a) => a.Item == item) >= 0;
    }

    public void setGoal(Items item, int amount)
    {
        ItemsToGet = new List<itemAmount> { new itemAmount { Item = item, Amount = amount } };
        CurrentItems = new List<itemAmount>();
    }
    
    public void setGoal(List<itemAmount> items)
    {
        ItemsToGet = items;
        CurrentItems = new List<itemAmount>();
    }

    public itemAmount GetItemAmount(Items item, bool fromCurrentItems)
    {
        if (fromCurrentItems)
            return CurrentItems.Find((x) => x.Item == item);
        else
            return ItemsToGet.Find((x) => x.Item == item);
        
    }

    public void SetDungeonSetting(DungeonSetting setting)
    {
        CurrentSetting = setting;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)) SceneSystem.instance.FadeToNextScene("Ending");
    }
}
