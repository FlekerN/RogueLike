using System;
public enum StatType
{
    Fuerza,
    Resistencia,
    Supervivencia
}
[Serializable]
public class Item
{
    public string ID;
    public string Name;
    public string Description;
    public StatType Stat;
    public int Price;
    public bool bought;

    
    public Item(string iD, string name, string description,int price, StatType stat)
    {
        ID = iD;
        Name = name;
        Description = description;
        Stat = stat;
        Price = price;
        bought = false;
    }
}
