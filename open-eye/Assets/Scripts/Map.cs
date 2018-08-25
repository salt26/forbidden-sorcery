using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    [SerializeField]
    public Sprite allySprite;
    public Sprite enemySprite;
    public Sprite neutralSprite;
    public Sprite devilKingCastleSprite;

    [SerializeField]
    public Transform centralPositionIndicator;
    public Transform allyPositionIndicator;
    public Transform enemyPositionIndicator;
    public Font font;
    
}
