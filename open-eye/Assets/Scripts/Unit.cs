using System.Collections;
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
    private bool isMoving;

    public bool canMove
    {
        get
        {
            return Movement > 0 && !position.isFighting;
        }
    }

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
        GetComponent<SpriteRenderer>().enabled = true;

        if (Movement > 0)
        {
            Movement--;
            position = to;
            moveQueue.Enqueue(MoveBetweenNodesAnimation(from, to));
        }

        if (!isMoving && moveQueue.Count > 0)
        {
            isMoving = true;
            StartCoroutine(moveQueue.Dequeue());
        }
    }

    IEnumerator MoveBetweenNodesAnimation(Node from, Node to)
    {
        GameManager.instance.movingUnits.Add(this);

        float duration = 0.5f;
        float deltaTime = 0;
        float rate = deltaTime / duration;

        List<Unit> fromUnitList = from.units;
        fromUnitList.Remove(this);
        from.DecideAndShowMainUnit();
        from.SetUnitPosition();
        var initialPosition = this.transform.localPosition;

        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.localPosition = Vector3.Lerp(initialPosition, to.transform.localPosition, rate);
            yield return null;
        }

        transform.localPosition = to.transform.localPosition;
        
        List<Unit> toUnitList = to.units;
        toUnitList.Add(this);


        to.SetUnitPosition();
        to.DecideAndShowMainUnit();

        OnMoveAnimationFinished();
    }

    public void OnMoveAnimationFinished()
    {
        if (moveQueue.Count > 0)
        {
            StartCoroutine(moveQueue.Dequeue());
        }
        else
        {
            isMoving = false;
            
            GameManager.instance.movingUnits.Remove(this);

            if (onMoveDone != null)
            {
                onMoveDone();
            }
        }
    }

    public IEnumerator MoveInNode(Vector3 destination)
    {
        Vector3 initialPosition = this.transform.position;

        float duration = 0.5f;
        float deltaTime = 0;
        float rate = deltaTime / duration;

        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.position = Vector3.Lerp(initialPosition, destination, rate);
            yield return null;
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
    public Stack<Node> NextNodeCandidate= new Stack<Node>();
    public Queue<Node> Nodes = new Queue<Node>();

    public Node NextNode()
    {
        Node nextNode = null;
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
