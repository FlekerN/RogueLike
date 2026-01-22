using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
   public Tile[] ObstacleTile;
   public Tile ObstacleDamaged;
   public int MaxHealth = 3;

   private int m_HealthPoint;
   private Tile m_OriginalTile;
  
   public override void Init(Vector2Int cell)
   {
       base.Init(cell);

       m_HealthPoint = MaxHealth;
       
       m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
       Tile newtile = ObstacleTile[Random.Range(0, ObstacleTile.Length)];
       GameManager.Instance.BoardManager.SetCellTile(cell, newtile);
   }

   public override bool PlayerWantsToEnter()
   {
       m_HealthPoint -= 1;

       if (m_HealthPoint == 1)
       {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, ObstacleDamaged);
       }

       if (m_HealthPoint > 0)
       {
           return false;
       }
 
       GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
       Destroy(gameObject);
       return true;
   }
}

