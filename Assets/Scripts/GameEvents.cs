using UnityEngine;
using System;
using System.Security.Cryptography;
public static class GameEvents
{
    public static event Action OnEmemyKilled;
    public static event Action OnWallDestroyed;
    public static event Action<int> OnFoodGathered;
    public static event Action OnPlayerStep;
    public static event Action OnChestOpened;

    public static void RaiseEnemyKill() 
    {
        OnEmemyKilled?.Invoke();
    }
    public static void RaiseWallDestroyed()
    {
        OnWallDestroyed?.Invoke();
    }
    public static void RaisePlayerStep() 
    {
        OnPlayerStep?.Invoke();
    }
    public static void RaiseFoodPicked(int amount) 
    {
        OnFoodGathered?.Invoke(amount); 
    }
    public static void RaiseChestOpened() 
    { 
        OnChestOpened?.Invoke();
    }
}
