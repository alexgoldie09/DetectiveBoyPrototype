using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame_Actions : Actions
{
    public override void Act()
    {
        GameManager.instance.QuitGame();
    }
}
