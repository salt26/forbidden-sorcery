using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaAmount : MonoBehaviour {

    [SerializeField]
    private Text manaAmount;

    public void SetManaAmount(int currentMana, int manaDelta) {

        manaAmount.text = string.Format("{0} (+{1})", currentMana, manaDelta);
    }
}
