using UnityEngine;

public class ShopObject : CellObject
{ 
    public override bool PlayerWantsToEnter()
    {
       GameManager.Instance.OpenShop();
        return false;
    }
}
