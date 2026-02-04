using UnityEngine;

public class EnemyController : CellObject
{
   public int Health = 3;
   private BoardManager m_Board;
   public Sprite[] enemies;
   private SpriteRenderer spriteRenderer;
   private int m_CurrentHealth;

   private void Awake()
   {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
   }

   private void OnDestroy()
   {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
   }

   public override void Init(Vector2Int coord)
   {
      base.Init(coord);

     if (enemies.Length > 0)
     {
      Sprite newSprite = enemies[Random.Range(0, enemies.Length)];
      spriteRenderer.sprite = newSprite;
     }
      m_CurrentHealth = Health;
   }

   public override bool PlayerWantsToEnter()
   {
       m_CurrentHealth -= 1;

       if (m_CurrentHealth <= 0)
       {
            GameEvents.RaiseEnemyKill();
            Destroy(gameObject);
       }

       return false;
   }

   bool MoveTo(Vector2Int coord)
   {
       var board = GameManager.Instance.BoardManager;
       var targetCell =  board.GetCellData(coord);

      if (targetCell == null
          || !targetCell.Passable
          || targetCell.ContainedObject != null)
      {
          return false;
      }
    
      //remove enemy from current cell
      var currentCell = board.GetCellData(m_Cell);
      currentCell.ContainedObject = null;
    
      //add it to the next cell
      targetCell.ContainedObject = this;
      m_Cell = coord;
      transform.position = board.CellToWorld(coord);

      return true;
   }

   void TurnHappened()
   {
      //We added a public property that return the player current cell!
      var playerCell = GameManager.Instance.PlayerController.CellPosition();

      int xDist = playerCell.x - m_Cell.x;
      int yDist = playerCell.y - m_Cell.y;

      int absXDist = Mathf.Abs(xDist);
      int absYDist = Mathf.Abs(yDist);

      if ((xDist == 0 && absYDist == 1)
          || (yDist == 0 && absXDist == 1))
      {
          //we are adjacent to the player, attack!
          GameManager.Instance.ChangeFood(3);
      }
      else
      {
          if (absXDist > absYDist)
          {
              if (!TryMoveInX(xDist))
              {
                  //if our move was not successful (so no move and not attack)
                  //we try to move along Y
                  TryMoveInY(yDist);
              }
          }
          else
          {
              if (!TryMoveInY(yDist))
              {
                  TryMoveInX(xDist);
              }
          }
      }
   }

   bool TryMoveInX(int xDist)
   {
      //try to get closer in x
     
      //player to our right
      if (xDist > 0)
      {
          return MoveTo(m_Cell + Vector2Int.right);
      }
    
      //player to our left
      return MoveTo(m_Cell + Vector2Int.left);
   }

    bool TryMoveInY(int yDist)
    {
      //try to get closer in y
     
      //player on top
      if (yDist > 0)
      {
          return MoveTo(m_Cell + Vector2Int.up);
      }

      //player below
      return MoveTo(m_Cell + Vector2Int.down);
    }
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
       m_Board = boardManager;
       MoveTo(cell);
    }
}

