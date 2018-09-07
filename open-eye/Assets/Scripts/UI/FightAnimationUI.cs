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
    
    [SerializeField]
    public static Sprite tankerImage;
    public static Sprite soldierImage;
    public static Sprite archerImage;
    public static Sprite assassinImage;
    public static Sprite mageImage;
    public static Sprite zombieImage;
    public static Sprite wolfImage;
    public static Sprite skeletonImage;

    public static Sprite enemyTankerImage;
    public static Sprite enemySoldierImage;
    public static Sprite enemyArcherImage;
    public static Sprite enemyAssassinImage;
    public static Sprite enemyMageImage;

    public Sprite t1;
    public Sprite t2;
    public Sprite t3;
    public Sprite t4;
    public Sprite t5;
    public Sprite t6;
    public Sprite t7;
    public Sprite t8;
    public Sprite t9;
    public Sprite t10;
    public Sprite t11;
    public Sprite t12;
    public Sprite t13;

    void Start()
    {
        tankerImage = t1;
        soldierImage = t2;
        archerImage = t3;
        assassinImage = t4;
        mageImage = t5;
        zombieImage = t6;
        wolfImage = t7;
        skeletonImage = t8;
        enemyTankerImage = t9;
        enemySoldierImage = t10;
        enemyArcherImage = t11;
        enemyAssassinImage = t12;
        enemyMageImage = t13;
    }

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

        BattleCryManager.instance.PlaySound();

        GameObject.Find("FightAnimationUI").GetComponent<Image>().enabled = true;

        for (int j = 1; j <= AllyNumber; j++)
        {
            if (isThereTanker[i] || isThereZombie[i])
            {
                if (isThereTanker[i])
                {
                    isThereTanker[i] = false;
                    isThereZombie[i] = false;
                    GameObject.Find("Ally" + j).GetComponent<Image>().sprite = tankerImage;
                }
                if (isThereZombie[i])
                {
                    isThereTanker[i] = false;
                    isThereZombie[i] = false;
                    GameObject.Find("Ally" + j).GetComponent<Image>().sprite = zombieImage;
                }
            }
            else if (isThereSoldier[i] || isThereWolf[i])
            {
                if (isThereSoldier[i])
                {
                    isThereSoldier[i] = false;
                    isThereWolf[i] = false;
                    GameObject.Find("Ally" + j).GetComponent<Image>().sprite = soldierImage;
                }
                if (isThereWolf[i])
                {
                    isThereSoldier[i] = false;
                    isThereWolf[i] = false;
                    GameObject.Find("Ally" + j).GetComponent<Image>().sprite = wolfImage;
                }
            }
            else if (isThereArcher[i] || isThereSkeleton[i])
            {
                if (isThereArcher[i])
                {
                    isThereArcher[i] = false;
                    isThereSkeleton[i] = false;
                    GameObject.Find("Ally" + j).GetComponent<Image>().sprite = archerImage;
                }
                if (isThereSkeleton[i])
                {
                    isThereArcher[i] = false;
                    isThereSkeleton[i] = false;
                    GameObject.Find("Ally" + j).GetComponent<Image>().sprite = skeletonImage;
                }
            }
            else if (isThereAssassin[i])
            {
                isThereAssassin[i] = false;
                GameObject.Find("Ally" + j).GetComponent<Image>().sprite = assassinImage;
            }
            else if (isThereMage[i])
            {
                isThereMage[i] = false;
                GameObject.Find("Ally" + j).GetComponent<Image>().sprite = mageImage;
            }

            GameObject.Find("Ally" + j).GetComponent<RectTransform>().localPosition = new Vector3(-250 + 50 * AllyNumber - 100 * j, -100, 0);
            GameObject.Find("Ally" + j).GetComponent<Image>().enabled = true;
        }
        

        for (int j = 1; j <= EnemyNumber; j++)
        {
            if (isThereEnemyTanker[i])
            {
                isThereEnemyTanker[i] = false;
                GameObject.Find("Enemy" + j).GetComponent<Image>().sprite = enemyTankerImage;
            }
            else if (isThereEnemySoldier[i])
            {
                isThereEnemySoldier[i] = false;
                GameObject.Find("Enemy" + j).GetComponent<Image>().sprite = enemySoldierImage;
            }
            else if (isThereEnemyArcher[i])
            {
                isThereEnemyArcher[i] = false;
                GameObject.Find("Enemy" + j).GetComponent<Image>().sprite = enemyArcherImage;
            }
            else if (isThereEnemyAssassin[i])
            {
                isThereEnemyAssassin[i] = false;
                GameObject.Find("Enemy" + j).GetComponent<Image>().sprite = enemyAssassinImage;
            }
            else if (isThereEnemyMage[i])
            {
                isThereEnemyMage[i] = false;
                GameObject.Find("Enemy" + j).GetComponent<Image>().sprite = enemyMageImage;
            }

            GameObject.Find("Enemy" + j).GetComponent<RectTransform>().localPosition = new Vector3(250 - 50 * EnemyNumber + 100 * j, -100, 0);
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
        GameObject.Find("AssassinDamage(Enemy->Ally)").GetComponent<Text>().text = "" + GameManager.instance.publicEnemyAssassinAttack[i];
        GameObject.Find("MageDamage(Enemy->Ally)").GetComponent<Text>().text = "" + GameManager.instance.publicEnemyMageAttack[i];
        GameObject.Find("FightDamage(Enemy->Ally)").GetComponent<Text>().text = "" + GameManager.instance.publicEnemyAttack[i];
        GameObject.Find("FightDamage(Ally->Enemy)").GetComponent<Text>().text = "" + GameManager.instance.publicAllyAttack[i];
        GameObject.Find("MageDamage(Ally->Enemy)").GetComponent<Text>().text = "" + GameManager.instance.publicAllyMageAttack[i];
        GameObject.Find("AssassinDamage(Ally->Enemy)").GetComponent<Text>().text = "" + GameManager.instance.publicAllyAssassinAttack[i];
    }

    public static IEnumerator FightAnimation(int i)
    {
        yield return new WaitUntil(() => isPastFightAnimationFinished[i - 1]);

        ShowFightAnimationUI(i);

        GameObject.Find("Main Camera").GetComponent<CameraController>().SetDestination
        (GameObject.Find(nodeName[i]).GetComponent<Transform>().position + new Vector3(0, 0, -10));

        ChangeFightAnimationText(i);

        if(GameManager.instance.publicAllyAssassinAttack[i] > 0)
        {
            GameObject.Find("AssassinDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
            GameObject.Find("AssassinDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
            HideFightAnimationText();
        }

        if (GameManager.instance.publicEnemyAssassinAttack[i] > 0)
        {
            GameObject.Find("AssassinDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
            GameObject.Find("AssassinDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
            HideFightAnimationText();
        }

        if (GameManager.instance.publicAllyAttack[i] > 0)
        {
            GameObject.Find("FightDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
            GameObject.Find("FightDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
            HideFightAnimationText();
        }

        if (GameManager.instance.publicEnemyAttack[i] > 0)
        {
            GameObject.Find("FightDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
            GameObject.Find("FightDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
            HideFightAnimationText();
        }

        if (GameManager.instance.publicAllyMageAttack[i] > 0)
        {
            GameObject.Find("MageDamage(Ally->Enemy)").GetComponent<Text>().enabled = true;
            GameObject.Find("MageDamageEffect(Ally->Enemy)").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
            HideFightAnimationText();
        }

        if (GameManager.instance.publicEnemyMageAttack[i] > 0)
        {
            GameObject.Find("MageDamage(Enemy->Ally)").GetComponent<Text>().enabled = true;
            GameObject.Find("MageDamageEffect(Enemy->Ally)").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(GameManager.instance.fightAnimationDuration);
            HideFightAnimationText();
        }

        HideFightAnimationUI();

        isPastFightAnimationFinished[i] = true;
    }
}
