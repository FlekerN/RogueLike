using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public string nombre { get; set; }
    public int fuerza { get; set; }
    public int resistencia { get; set; }
    public int supervivencia { get; set; }
    public int inteligencia { get; set; }
    public int puntosDisponibles { get; set; }
    public int experiencia { get; set; }
    public int vidaMaxima { get; set; }
    public int damage { get; set; }
    public int recoleccion { get; set; }


    public PlayerStats()
    {

        fuerza = 1;
        resistencia = 1;
        supervivencia = 1;
        inteligencia = 1;
        puntosDisponibles = 0;
        experiencia = 0;

        vidaMaxima = 0;
        damage = 0;
        recoleccion = 0;
    }

    public PlayerStats(string nombre):this()
    { 
        this.nombre = nombre;
        
    }
    public void RecalcularStats()
    {
        vidaMaxima = 20 + resistencia * 10;
        damage = (10 + fuerza) / 10;
        recoleccion = 5 + supervivencia / 2;
    }   
    public PlayerStats Clone()
    {
        return new PlayerStats
        {
            nombre = this.nombre,
            fuerza = this.fuerza,
            resistencia = this.resistencia,
            supervivencia = this.supervivencia,
            inteligencia = this.inteligencia,
            puntosDisponibles = this.puntosDisponibles,
            experiencia = this.experiencia,
            vidaMaxima = this.vidaMaxima,
            damage = this.damage,
            recoleccion =this.recoleccion
        };
    }

}
