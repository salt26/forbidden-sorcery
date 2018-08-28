using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightAnimationUI : MonoBehaviour {

    [HideInInspector]
    public static bool[] isPastFightAnimationFinished = new bool[100];

    public static void ShowFightAnimationUI()
    {
        GameObject.Find("FightAnimationUI").GetComponent<Image>().enabled = true;
        GameObject.Find("Ally5").GetComponent<Image>().enabled = true;
        GameObject.Find("Ally4").GetComponent<Image>().enabled = true;
        GameObject.Find("Ally3").GetComponent<Image>().enabled = true;
        GameObject.Find("Ally2").GetComponent<Image>().enabled = true;
        GameObject.Find("Ally1").GetComponent<Image>().enabled = true;
        GameObject.Find("Enemy1").GetComponent<Image>().enabled = true;
        GameObject.Find("Enemy2").GetComponent<Image>().enabled = true;
        GameObject.Find("Enemy3").GetComponent<Image>().enabled = true;
        GameObject.Find("Enemy4").GetComponent<Image>().enabled = true;
        GameObject.Find("Enemy5").GetComponent<Image>().enabled = true;
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

        ChangeFightAnimationText(i);

        GameObject.Find("AssassinDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("AssassinDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("FightDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("FightDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("MageDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        GameObject.Find("MageDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
        HideFightAnimationText();

        isPastFightAnimationFinished[i] = true;
    }
}
