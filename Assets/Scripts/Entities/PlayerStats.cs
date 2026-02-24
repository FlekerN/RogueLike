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
