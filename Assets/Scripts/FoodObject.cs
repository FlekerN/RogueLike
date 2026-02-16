using UnityEngine;

public class FoodObject : CellObject
{
   public int AmountGranted;
  
   public override void PlayerEntered()
   {
       AmountGranted = SessionManager.Instance.PlayerData.recoleccion;

       Destroy(gameObject);
       GameManager.Instance.ChangeFood(AmountGranted);
       GameEvents.RaiseFoodPicked(AmountGranted);
   }
}
