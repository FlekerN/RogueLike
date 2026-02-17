using UnityEngine;

public class Achievment
{
    public string ID;
    public string Name;
    public string Description;
    public int AmountToAchieve;

    public int CurrentAchieved;
    public bool Completed;

    public Achievment(string iD, string name, string description, int amountToAchieve)
    {
        ID = iD;
        Name = name;
        Description = description;
        AmountToAchieve = amountToAchieve;
        CurrentAchieved = 0;
        Completed = false;
    }

}
