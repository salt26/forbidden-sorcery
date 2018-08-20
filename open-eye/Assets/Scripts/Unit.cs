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

    [HideInInspector]
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

        if (!isMoving && moveQueue.Count > 0)               //맨 첫번째만을 움직이게 하기 위한 것
        {
            isMoving = true;
            StartCoroutine(moveQueue.Dequeue());
        }
    }

    IEnumerator MoveBetweenNodesAnimation(Node from, Node to)
    {
        GameManager.instance.movingUnits.Add(this);

        List<Unit> fromUnitList = from.units;
        fromUnitList.Remove(this);
        from.unitMovedThisTurn = true;
        from.DecideAndShowMainUnit();
        List<Unit> toUnitList = to.units;
        toUnitList.Add(this);
        to.unitMovedThisTurn = true;

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

        OnMoveAnimationFinished();                  //하나의 동작이 끝나는 순간 무엇을 할 것인가?
    }

    public void OnMoveAnimationFinished()
    {
        if (moveQueue.Count > 0)                                //큐에 쌓인 게 남았을 때
        {
            StartCoroutine(moveQueue.Dequeue());                //다음 큐를 실행해야지
        }
        else
        {                                               //큐가 비었을 때 - 이젠 끝내야지?
            isMoving = false;

            GameManager.instance.movingUnits.Remove(this);

            if(GameManager.instance.movingUnits.Count == 0)         //이동한 놈들 재배치해야지
            {
                Node.RefineUnitPositionInAllNodes();
            }
            
            if (onMoveDone != null)                             //이건 아군 유닛일 경우 비어 있으므로 아군 움직임 턴에 대해서는 신경쓰지 않아도 됨
            {
                onMoveDone();
            }
        }
    }

    public IEnumerator MoveInNode(Vector3 destination)
    {
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
