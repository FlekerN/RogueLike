using UnityEngine;

public class EXPObject : CellObject
{
    public int AmountGranted = 1;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.ChangeEXP(AmountGranted);

    }
}
