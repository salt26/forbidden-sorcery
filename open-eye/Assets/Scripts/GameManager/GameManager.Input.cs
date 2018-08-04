using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    private bool isNodeInputActive
    {
        get
        {
            switch (currentState)
            {
                case RoundState.PlayerAction:
                    return true;
                default:
                    return false;
            }
        }
    }
}
