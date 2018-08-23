﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IUnitInterface
{
    public delegate void OnMoveDone();

    [HideInInspector]
    public Node position;

    [HideInInspector]
    public bool isAlly { get; set; }

    [HideInInspector]
    public bool mainUnitInNode = false;

    public Queue<IEnumerator> moveQueue = new Queue<IEnumerator>();

    public static float movingTimeInNode = 0.2f;

    public UnitData unitData;
    public OnMoveDone onMoveDone;

    public UnitData UD
    {
        get
        {
            return unitData;
        }
        set
        {
            if (value is UnitData)
                unitData = value;
        }
    }
    public int ID { get; set; }
    public int CurrentHealth { get; set; }
    public int Movement { get; set; }
    private bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }
    private bool isMoved = false;
    //private bool isMovingInNode = false;
    public int level;

    public bool canMove
    {
        get
        {
            if (isAlly)
                return Movement > 0 && !position.isFighting;
            else
                return unitData.currentMoveType != UnitData.MoveType.stay && Movement > 0 && !position.isFighting;
        }
    }

    //private void FixedUpdate()
    //{
    //    if (moveQueue.Count > 0)
    //        isMoving = true;
    //    else
    //        isMoving = false;
    //}

    public void Refresh()
    {
        Movement = unitData.movement;
    }

    public void Damage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public void SetUnit(UnitData unitData)
    {
        this.unitData = unitData;
        GetComponent<SpriteRenderer>().sprite = AssetManager.Instance.GetSprite(unitData.spriteName);
        CurrentHealth = unitData.health;
        Movement = 0;
    }

    public void MoveBetweenNodes(Node from, Node to)
    {
        if (!GameManager.instance.movingUnits.Contains(this))
            GameManager.instance.movingUnits.Add(this);

        if (Movement > 0)
        {
            Movement--;
            position = to;

            //if (isAlly)
            //{
            //    from.RefineUnitPosition(from.allies.Count - 1, from.enemies.Count, this);
            //}
            //else
            //{
            //    from.RefineUnitPosition(from.allies.Count, from.enemies.Count - 1, this);
            //}
            List<Unit> fromUnitList = from.units;
            fromUnitList.Remove(this);
            List<Unit> toUnitList = to.units;
            toUnitList.Add(this);
            moveQueue.Enqueue(MoveBetweenNodesAnimation(from, to));

            //if (isAlly)
            //{
            //    to.RefineUnitPosition(to.allies.Count + 1, to.enemies.Count, this);
            //}
            //else
            //{
            //    to.RefineUnitPosition(to.allies.Count, to.enemies.Count + 1, this);
            //}
        }

        if (!isMoving && moveQueue.Count > 0)               //맨 첫번째만을 움직이게 하기 위한 것
        {
            StartCoroutine(moveQueue.Dequeue());
        }
    }

    IEnumerator MoveBetweenNodesAnimation(Node from, Node to)
    {
        isMoving = true;
        isMoved = true;

        from.DecideAndShowMainUnit();
        GetComponent<SpriteRenderer>().enabled = true;

        float duration = 0.5f;                                                  //여기서부터
        float deltaTime = 0;
        float rate = deltaTime / duration;
        var initialPosition = this.transform.position;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.position = Vector3.Lerp(initialPosition, to.transform.position, rate);
            yield return null;
        }
        transform.position = to.transform.position;                   //여기까지의 코드를 실행하는 데 0.5초(=이동시간)이 걸림

        to.DecideAndShowMainUnit();

        OnMoveBetweenNodesAnimationFinished();                  //하나의 동작이 끝나는 순간 무엇을 할 것인가?
    }

    void OnMoveBetweenNodesAnimationFinished()
    {
        if (moveQueue.Count > 0)                                //큐에 쌓인 게 남았을 때
        {
            StartCoroutine(moveQueue.Dequeue());                //다음 큐를 실행해야지
        }
        else
        {                                               //큐가 비었을 때 - 이젠 끝내야지?

            //GameManager.instance.movingUnits.Remove(this);


            //if(GameManager.instance.movingUnits.Count == 0)         //이동한 놈들 재배치해야지
            //{
            //    Node.RefineUnitPositionInAllNodes();
            //}
        }
    }

    public IEnumerator MoveInNodeAnimation(Vector3 destination)
    {
        isMoving = true;
        Vector3 initialPosition = this.transform.position;
        float duration = movingTimeInNode;
        float deltaTime = 0;
        float rate = deltaTime / duration;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.position = Vector3.Lerp(initialPosition, destination, rate);
            yield return null;
        }
        transform.position = destination;
        OnMoveInNodeAnimationFinished();

    }

    void OnMoveInNodeAnimationFinished()
    {
        //isMovingInNode = false;
        if (moveQueue.Count > 0)
        {
            StartCoroutine(moveQueue.Dequeue());
        }
        else
        {
            //if (GameManager.instance.currentState == GameManager.RoundState.PlayerAction)
            //GameManager.instance.EndTurnButton.interactable = true;
            GameManager.instance.movingUnits.Remove(this);
            if (onMoveDone != null && isMoved)// && GameManager.instance.currentState == GameManager.RoundState.EnemyMove)
            {
                onMoveDone();
            }
            isMoved = false;
            isMoving = false;
        }
    }

    public int CompareTo(object obj)
    {
        IUnitInterface imaginaryUnit;
        if (!(obj is IUnitInterface))
            return 1;
        else
        {
            imaginaryUnit = obj as IUnitInterface;
            return this.UD.aggro - imaginaryUnit.UD.aggro;
        }
    }
    public Stack<Node> NextNodeCandidate = new Stack<Node>();
    public Queue<Node> Nodes = new Queue<Node>();

    public Node NextNode()
    {
        Node nextNode = position.edges[0];
        if ((int)unitData.currentMoveType == 2)
        {
            return position;
        }
        {
            if (unitData.currentMoveType == 0)
            {
                foreach (Node node in position.edges)
                {
                    if (node.distance < nextNode.distance)
                    {
                        nextNode = node;
                    }
                    else if (nextNode.distance == node.distance)
                    {
                        int r = Random.Range(0, 1);
                        if (r == 0)
                            nextNode = node;
                    }
                }
            }
            else
            {
                if (position.isPlayerTerritory)
                {
                    return position;
                }
                var distanceMin = int.MaxValue - 1;
                var distance = int.MaxValue;
                Node nextNodeCandidate = null;
                NextNodeCandidate.Clear();
                foreach (Node node in position.edges)
                {
                    NextNodeCandidate.Push(node);
                }
                while (NextNodeCandidate.Count != 0)
                {
                    nextNodeCandidate = NextNodeCandidate.Pop();
                    if (nextNodeCandidate.isPlayerTerritory)
                    {
                        distance = 0;
                        nextNode = nextNodeCandidate;
                        distanceMin = distance;
                        if (distance == distanceMin)
                        {
                            int r = Random.Range(0, 1);
                            if (r == 0)
                                nextNode = nextNodeCandidate;
                        }
                        break;

                    }
                    Nodes.Clear();
                    Nodes.Enqueue(nextNodeCandidate);
                    distance = 1;
                    var previousCount = 1;
                    var currentCount = 0;
                    bool escape = true;
                    foreach (Node node in GameManager.instance.allNodes)
                    {
                        node.isChecked = false;
                    }
                    position.isChecked = true;
                    while (escape)
                    {
                        if (previousCount == 0)
                        {
                            previousCount = currentCount;
                            currentCount = 0;
                            distance++;
                        }
                        previousCount--;
                        if (Nodes.Count == 0 || distance > distanceMin)
                        {
                            distance = int.MaxValue;
                            break;
                        }
                        Node check = null;
                        check = Nodes.Dequeue();
                        foreach (Node node in check.edges)
                        {
                            if (node.isPlayerTerritory)
                            {
                                escape = false;
                                break;
                            }
                            else if (!node.isChecked)
                            {
                                Nodes.Enqueue(node);
                                node.isChecked = true;
                                currentCount++;
                            }
                        }
                    }
                    if (distance < distanceMin)
                    {
                        nextNode = nextNodeCandidate;
                        distanceMin = distance;
                    }

                    if (distance == distanceMin)
                    {
                        int r = Random.Range(0, 1);
                        if (r == 0)
                            nextNode = nextNodeCandidate;
                    }
                }
            }
        }
        return nextNode;
    }
}
