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

    public void Move(Node from, Node to)
    {
        GetComponent<SpriteRenderer>().enabled = true;

        if (Movement > 0)
        {
            Movement--;
            position = to;
            moveQueue.Enqueue(MoveAnimation(from, to));
        }

        if (!isMoving && moveQueue.Count > 0)
        {
            isMoving = true;
            StartCoroutine(moveQueue.Dequeue());
        }
    }

    IEnumerator MoveAnimation(Node from, Node to)
    {
        GameManager.instance.movingUnits.Add(this);

        float duration = 0.5f;
        float deltaTime = 0;
        float rate = deltaTime / duration;
        List<Unit> fromUnitList = from.units;
        List<Unit> toUnitList = to.units;
        toUnitList.Add(this);
        fromUnitList.Remove(this);
        from.DecideAndShowMainUnit();
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / duration;
            transform.localPosition = Vector3.Lerp(from.transform.localPosition, to.transform.localPosition, rate);
            yield return null;
        }

        transform.localPosition = to.transform.localPosition;

        OnMoveAnimationFinished();
        to.DecideAndShowMainUnit();
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
}
