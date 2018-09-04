using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightAnimationUI : MonoBehaviour {

    [HideInInspector]
    public static bool[] isPastFightAnimationFinished = new bool[300];

    [HideInInspector]
    public static string[] nodeName = new string[300];

    [HideInInspector]
    public static bool[] isThereEnemyTanker = new bool[300];

    [HideInInspector]
    public static bool[] isThereEnemySoldier = new bool[300];

    [HideInInspector]
    public static bool[] isThereEnemyArcher = new bool[300];

    [HideInInspector]
    public static bool[] isThereEnemyAssassin = new bool[300];

    [HideInInspector]
    public static bool[] isThereEnemyMage = new bool[300];

    [HideInInspector]
    public static bool[] isThereTanker = new bool[300];

    [HideInInspector]
    public static bool[] isThereSoldier = new bool[300];

    [HideInInspector]
    public static bool[] isThereArcher = new bool[300];

    [HideInInspector]
    public static bool[] isThereAssassin = new bool[300];

    [HideInInspector]
    public static bool[] isThereMage = new bool[300];

    [HideInInspector]
    public static bool[] isThereZombie = new bool[300];

    [HideInInspector]
    public static bool[] isThereWolf = new bool[300];

    [HideInInspector]
    public static bool[] isThereSkeleton = new bool[300];

    public static void ShowFightAnimationUI(int i)
    {
        int AllyNumber = 0;
        int EnemyNumber = 0;

        if (isThereTanker[i] || isThereZombie[i]) AllyNumber++;
        if (isThereSoldier[i] || isThereWolf[i]) AllyNumber++;
        if (isThereArcher[i] || isThereSkeleton[i]) AllyNumber++;
        if (isThereAssassin[i]) AllyNumber++;
        if (isThereMage[i]) AllyNumber++;

        if (isThereEnemyTanker[i]) EnemyNumber++;
        if (isThereEnemySoldier[i]) EnemyNumber++;
        if (isThereEnemyArcher[i]) EnemyNumber++;
        if (isThereEnemyAssassin[i]) EnemyNumber++;
        if (isThereEnemyMage[i]) EnemyNumber++;

        GameObject.Find("FightAnimationUI").GetComponent<Image>().enabled = true;

        GameObject.Find("Ally1").GetComponent<Image>().enabled = true;

        for (int j = 1; j <= AllyNumber; j++)
        {
            GameObject.Find("Ally" + j).GetComponent<RectTransform>().localPosition = new Vector3(-250 + 50 * AllyNumber - 100 * j, -100, 0);
            GameObject.Find("Ally" + j).GetComponent<Image>().enabled = true;
        }

        for (int j = 1; j <= EnemyNumber; j++)
        {
            GameObject.Find("Enemy" + j).GetComponent<RectTransform>().localPosition = new Vector3(250 - 50 * AllyNumber + 100 * j, -100, 0);
            GameObject.Find("Enemy" + j).GetComponent<Image>().enabled = true;
        }
    }

    public static void HideFightAnimationUI()
    {
        GameObject.Find("FightAnimationUI").GetComponent<Image>().enabled = false;
        GameObject.Find("Ally5").GetComponent<Image>().enabled = false;
        GameObject.Find("Ally4").GetComponent<Image>().enabled = false;
        GameObject.Find("Ally3").GetComponent<Image>().enabled = false;
        GameObject.Find("Ally2").GetComponent<Image>().enabled = false;
        GameObject.Find("Ally1").GetComponent<Image>().enabled = false;
        GameObject.Find("Enemy1").GetComponent<Image>().enabled = false;
        GameObject.Find("Enemy2").GetComponent<Image>().enabled = false;
        GameObject.Find("Enemy3").GetComponent<Image>().enabled = false;
        GameObject.Find("Enemy4").GetComponent<Image>().enabled = false;
        GameObject.Find("Enemy5").GetComponent<Image>().enabled = false;
    }

    public static void HideFightAnimationText()
    {
        GameObject.Find("AssassinDamage(Enemy->Ally)").GetComponent<Text>().enabled = false;
        GameObject.Find("MageDamage(Enemy->Ally)").GetComponent<Text>().enabled = false;
        GameObject.Find("FightDamage(Enemy->Ally)").GetComponent<Text>().enabled = false;
        GameObject.Find("FightDamage(Ally->Enemy)").GetComponent<Text>().enabled = false;
        GameObject.Find("MageDamage(Ally->Enemy)").GetComponent<Text>().enabled = false;
        GameObject.Find("AssassinDamage(Ally->Enemy)").GetComponent<Text>().enabled = false;
        GameObject.Find("AssassinDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = false;
        GameObject.Find("MageDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = false;
        GameObject.Find("FightDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = false;
        GameObject.Find("FightDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = false;
        GameObject.Find("MageDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = false;
        GameObject.Find("AssassinDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = false;
    }

    public static void ChangeFightAnimationText(int i)
    {
        GameObject.Find("AssassinDamage(Enemy->Ally)").GetComponent<Text>().text = "AD: " + GameManager.instance.publicEnemyAssassinAttack[i];
        GameObject.Find("MageDamage(Enemy->Ally)").GetComponent<Text>().text = "MD: " + GameManager.instance.publicEnemyMageAttack[i];
        GameObject.Find("FightDamage(Enemy->Ally)").GetComponent<Text>().text = "FD: " + GameManager.instance.publicEnemyAttack[i];
        GameObject.Find("FightDamage(Ally->Enemy)").GetComponent<Text>().text = "FD: " + GameManager.instance.publicAllyAttack[i];
        GameObject.Find("MageDamage(Ally->Enemy)").GetComponent<Text>().text = "MD: " + GameManager.instance.publicAllyMageAttack[i];
        GameObject.Find("AssassinDamage(Ally->Enemy)").GetComponent<Text>().text = "AD: " + GameManager.instance.publicAllyAssassinAttack[i];
    }

    public static IEnumerator FightAnimation(int i)
    {
        yield return new WaitUntil(() => isPastFightAnimationFinished[i - 1]);

        ShowFightAnimationUI(i);

        GameObject.Find("Main Camera").GetComponent<CameraController>().SetDestination
        (GameObject.Find(nodeName[i]).GetComponent<Transform>().position + new Vector3(0, 0, -10));

        ChangeFightAnimationText(i);

        GameObject.Find("AssassinDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
        GameObject.Find("AssassinDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("AssassinDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
        GameObject.Find("AssassinDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("FightDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
        GameObject.Find("FightDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("FightDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
        GameObject.Find("FightDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("MageDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
        GameObject.Find("MageDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("MageDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
        GameObject.Find("MageDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        HideFightAnimationUI();

        isPastFightAnimationFinished[i] = true;
    }
}
