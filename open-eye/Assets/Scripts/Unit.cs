using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public delegate void OnMoveDone();

    [HideInInspector]
    public Node position;

    [HideInInspector]
    public bool isAlly;
    
    public Queue<IEnumerator> moveQueue = new Queue<IEnumerator>();
    
    public UnitData unitData;
    public OnMoveDone onMoveDone;

    public int currentHealth
    {
        get;
        private set;
    }
    private int movement;
    private bool isMoving;

    public bool canMove
    {
        get
        {
            return movement > 0 && !position.isFighting;
        }
    }

    public void Refresh()
    {
        movement = unitData.movement;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
    
    public void SetUnit(UnitData unitData)
    {
        this.unitData = unitData;
        GetComponent<SpriteRenderer>().sprite = AssetManager.Instance.GetSprite(unitData.spriteName);
        currentHealth = unitData.health;
        movement = 0;
    }

    public void Move(Node from, Node to)
    {
        if (movement > 0)
        {
            movement--;
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

}
