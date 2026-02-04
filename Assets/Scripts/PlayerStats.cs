using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public string nombre { get; set; }
    public int fuerza { get; set; }
    public int resistencia { get; set; }
    public int destreza { get; set; }
    public int inteligencia { get; set; }
    public int puntosDisponibles { get; set; }
    public int experiencia { get; set; }


    public PlayerStats()
    {

        fuerza = 1;
        resistencia = 1;
        destreza = 1;
        inteligencia = 1;
        puntosDisponibles = 0;
        experiencia = 0;
    }

    public PlayerStats(string nombre):this()
    { 
        this.nombre = nombre;
        
    }


}
