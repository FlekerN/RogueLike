using System;

[Serializable]
public class PlayerStats
{
    public string nombre;
    public int fuerza;
    public int resistencia;
    public int supervivencia;
    public int inteligencia;
    public int puntosDisponibles;
    public int experiencia;
    public int vidaMaxima; 
    public int damage; 
    public int recoleccion; 
    public int CurrentLevel;

    public PlayerStats()
    {

        fuerza = 1;
        resistencia = 1;
        supervivencia = 1;
        inteligencia = 1;
        puntosDisponibles = 0;
        experiencia = 0;
        CurrentLevel = 1;
        vidaMaxima = 0;
        damage = 0;
        recoleccion = 0;
    }
    public void RecalcularStats()
    {
        vidaMaxima = 20 + resistencia * 10;
        damage = (10 + fuerza) / 10;
        recoleccion = 5 + supervivencia / 2;
    }   
    public void AplicarItem(Item item)
    {
        switch (item.Stat)
        {
            case StatType.Fuerza:
                SessionManager.Instance.PlayerData.fuerza++;
                break;
            case StatType.Resistencia:
                SessionManager.Instance.PlayerData.resistencia++;
                break;
            case StatType.Supervivencia:
                SessionManager.Instance.PlayerData.supervivencia++;
                break;
        }

        RecalcularStats();
    }
}
