using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    Node position;
    [SerializeField]
    bool isAlly;
    bool isMove;
    bool isMoved = true;
    bool isInitialized = false;
    public bool isOldOne = false;
    [HideInInspector]
    public int movableLength;//changeable
    public int staticMovableLength;
    public Queue<IEnumerator> moveQueue = new Queue<IEnumerator>();
    public string kind;//kind
    public int cost = 1;
    public int attck;
    public int health;
    [HideInInspector]
    public UnitData unitData;
    
    private void FixedUpdate()
    {
        if (moveQueue.Count != 0 && isMoved)
            StartCoroutine(moveQueue.Dequeue());
    }
    

    public string Kind
    {
        get
        {
            return kind;
        }
        set
        {
            kind = value;
        }
    }
    public Node Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

    public bool IsMoved
    {
        set
        {
            isMoved = value;
        }
        get
        {
            return isMoved;
        }
    }

    public bool IsMove
    {
        get
        {
            return isMove;
        }
        set
        {
            isMove = value;
        }
    }

    public bool IsAlly
    {
        get
        {
            return isAlly;
        }
        set
        {
            isAlly = value;
        }
    }

    public void Initialize()
    {
        /*
        if (!isAlly)
        {
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
        */
        isInitialized = true;
        movableLength = staticMovableLength;
    }

    public void Move(Node from, Node to)
    {
        //if (movableLength <= 0)
        //    isMove = false;
        //else 
        if (isInitialized && movableLength > 0)
        {
            isMove = true;
            movableLength--;
            position = to;
            moveQueue.Enqueue(MoveAnimation(from, to));
        }
        else
            isMove = false;
    }

    IEnumerator MoveAnimation(Node from, Node to)
    {
        isMoved = false;
        float duration = 0.5f;
        float deltaTime = 0;
        float rate = deltaTime / duration;
        List<Unit> fromUnitList = isAlly ? from.allies : from.enemies;
        List<Unit> toUnitList = isAlly ? to.allies : to.enemies;
        toUnitList.Add(this);
        fromUnitList.Remove(this);
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.localPosition = Vector3.Lerp(from.transform.localPosition, to.transform.localPosition, rate);
            yield return null;
        }

        transform.localPosition = to.transform.localPosition;

        isMoved = true;
        //Manager.manager.isAllMoved += 1;
        //if (Manager.manager.isAllMoved == Manager.manager.haveToMove)
        //{
        //    Manager.manager.isAllMoved = 0;
        //    Manager.manager.haveToMove = 0;
        //    Manager.manager.PlayerAction();
        //}
    }
}
