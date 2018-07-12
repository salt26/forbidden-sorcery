using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewContent : MonoBehaviour {
    public Button listButtonPrefab;
    public static ScrollViewContent manager;
    List<Button> list = new List<Button>();

    private void Awake()
    {
        manager = this;
    }

    public void AddComponent(Unit u, bool isForMove)
    {
        Button listCom = (Button) Instantiate(listButtonPrefab, this.transform);
        //listCom.GetComponent<RectTransform>().pivot = new Vector2((float)0.5, 1);
        //listCom.GetComponent<RectTransform>().anchorMax = new Vector2(1, (float) 0.5);
        //listCom.GetComponent<RectTransform>().anchorMin = new Vector2(0, (float) 0.5);
        listCom.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -this.GetComponent<RectTransform>().sizeDelta.y, 0);
        listCom.GetComponentInChildren<Text>().text = u.kind;
        listCom.GetComponent<UnitButton>().AddUnit(u, isForMove);
        list.Add(listCom);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y + listCom.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void Reset()
    {
        foreach(Button b in list)
        {
            Destroy(b.gameObject);
        }
        list.Clear();
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, 0);
    }
}
