using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class AchievementManger : MonoBehaviour
{
    public List<Achievment> Logros;

    private void Start()
    {
        Logros = new List<Achievment>();

        Achievment logro = new Achievment("KILL","Exterminador", "La sangre empapa tus ojos", 100);
        Logros.Add(logro);
        logro = new Achievment("KILL", "Genocida", "Memorias de un pintor Austriaco", 1000);
        Logros.Add(logro);
        logro = new Achievment("FOOD", "Cementerio de Hamburguesas", "Deberios de pensar en una dieta", 1000);
        Logros.Add(logro);
    }
    public void checkAchievement(string IDAchievement, int cantidad = 1) 
    {
        foreach (Achievment l in Logros) 
        {
            if (l.ID == IDAchievement && l.Completed == false) 
            {
                l.CurrentAchieved += cantidad;
                if (l.CurrentAchieved >= l.AmountToAchieve) 
                {
                    l.Completed=true;
                    Debug.Log(l.Name + ": "+l.Description);
                }
            }
        }
    }
    private void OnEnable()
    {
        GameEvents.OnEmemyKilled += HandleEnemyKill;
        GameEvents.OnWallDestroyed += HandleWallDestroyed;
        GameEvents.OnPlayerStep += HandlePlayerStep;
        GameEvents.OnFoodGathered += HandleFoodGathered;
        GameEvents.OnChestOpened+= HandleChestOpened;
    }
    private void OnDisable()
    {
        GameEvents.OnEmemyKilled -= HandleEnemyKill;
        GameEvents.OnWallDestroyed -= HandleWallDestroyed;
        GameEvents.OnPlayerStep -= HandlePlayerStep;
        GameEvents.OnFoodGathered -= HandleFoodGathered;
        GameEvents.OnChestOpened -= HandleChestOpened;
    }
    private void HandleEnemyKill() 
    {
        checkAchievement("KILL");
    }
    private void HandleWallDestroyed()
    {
        checkAchievement("WALL");
    }
    private void HandlePlayerStep()
    {
        checkAchievement("STEP");
    }
    private void HandleFoodGathered(int amount)
    {
        checkAchievement("FOOD", amount);
    }
    private void HandleChestOpened()
    {
        checkAchievement("CHEST");
    }

}
