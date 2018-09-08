using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseAlertText : MonoBehaviour
{
    public float phaseNoticeDuration;
    public float phaseMoveDuration;

    public Image phaseAlertImage;

    public bool isPhaseNoticeDone = false;

    public Vector3 initialPosition = new Vector3();
    public Vector3 middlePosition = new Vector3();
    public Vector3 finalPosition = new Vector3();
    [SerializeField]
    public Sprite enemyTurn;
    public Sprite battleStart;
    public Sprite playerTurn;
    public GameObject phaseAlertTextBackGround;

    public IEnumerator AlertPhase()
    {
        phaseAlertImage = GetComponent<Image>();
        isPhaseNoticeDone = false;
        phaseAlertImage.enabled = true;
        bool checkTurn = false;
        switch (GameManager.instance.currentState)
        {
            case GameManager.RoundState.Standby:
                phaseAlertTextBackGround.GetComponent<Image>().enabled = false;
                phaseAlertImage.enabled = false;
                checkTurn = false;
                break;
            case GameManager.RoundState.EnemyMove:
                phaseAlertTextBackGround.GetComponent<Image>().enabled = true;
                phaseAlertImage.sprite = enemyTurn;
                checkTurn = true;
                break;
            case GameManager.RoundState.PlayerAction:
                phaseAlertTextBackGround.GetComponent<Image>().enabled = true;
                phaseAlertImage.sprite = playerTurn;
                checkTurn = true;
                break;
            case GameManager.RoundState.AutoMove:
                phaseAlertTextBackGround.GetComponent<Image>().enabled = false;
                phaseAlertImage.enabled = false;
                checkTurn = false;
                break;
            case GameManager.RoundState.Captive:
                phaseAlertTextBackGround.GetComponent<Image>().enabled = false;
                phaseAlertImage.enabled = false;
                checkTurn = false;
                break;
            case GameManager.RoundState.Fight:
                phaseAlertTextBackGround.GetComponent<Image>().enabled = true;
                phaseAlertImage.sprite = battleStart;
                checkTurn = true;
                break;
        }
        if (checkTurn)
        {
            phaseAlertImage.GetComponent<Transform>().position = initialPosition;
            float deltaTime = 0;
            float rate = deltaTime / phaseMoveDuration;

            while (rate < 1f)
            {
                if (GameManager.instance.currentState == GameManager.RoundState.PlayerAction)
                {
                    phaseAlertImage.enabled = true;
                }
                deltaTime += Time.deltaTime;
                rate = deltaTime / phaseMoveDuration;
                phaseAlertImage.transform.position = Vector3.Lerp(initialPosition, middlePosition, rate);
                yield return null;
            }
            yield return new WaitForSeconds(phaseNoticeDuration);
            deltaTime = 0;
            rate = deltaTime / phaseMoveDuration;
            while (rate < 1f)
            {
                deltaTime += Time.deltaTime;
                rate = deltaTime / phaseMoveDuration;
                phaseAlertImage.transform.position = Vector3.Lerp(middlePosition, finalPosition, rate);
                yield return null;
            }
        }
        phaseAlertImage.enabled = false;
        phaseAlertTextBackGround.GetComponent<Image>().enabled = false;
        isPhaseNoticeDone = true;
        if(GameManager.RoundState.PlayerAction == GameManager.instance.currentState)
        {
            GameManager.instance.EndTurnButton.interactable = true;
            GameManager.instance.ProduceButton.interactable = true;
        }
    }
}
