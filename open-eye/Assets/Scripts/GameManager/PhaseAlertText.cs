using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseAlertText : MonoBehaviour
{
    public float phaseNoticeDuration;
    public float phaseMoveDuration;

    public Text phaseAlertText;

    public bool isPhaseNoticeDone = false;

    public Vector3 initialPosition = new Vector3();
    public Vector3 middlePosition = new Vector3();
    public Vector3 finalPosition = new Vector3();

    public IEnumerator AlertPhase()
    {
        Debug.Log("asd");
        phaseAlertText = GetComponent<Text>();
        isPhaseNoticeDone = false;
        this.GetComponent<Text>().enabled = true;

        switch (GameManager.instance.currentState)
        {
            case GameManager.RoundState.Standby:
                phaseAlertText.text = "Standby";
                break;
            case GameManager.RoundState.EnemyMove:
                phaseAlertText.text = "EnemyMove";
                break;
            case GameManager.RoundState.PlayerAction:
                phaseAlertText.text = "PlayerAction";
                break;
            case GameManager.RoundState.Fight:
                phaseAlertText.text = "Fight";
                break;
            case GameManager.RoundState.Captive:
                phaseAlertText.text = "Captive";
                break;
            case GameManager.RoundState.Upkeep:
                phaseAlertText.text = "Upkeep";
                break;
        }
        phaseAlertText.GetComponent<Transform>().position = initialPosition;
        float deltaTime = 0;
        float rate = deltaTime / phaseMoveDuration;

        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / phaseMoveDuration;
            phaseAlertText.transform.position = Vector3.Lerp(initialPosition, middlePosition, rate);
            yield return null;
        }
        yield return new WaitForSeconds(phaseNoticeDuration);
        deltaTime = 0;
        rate = deltaTime / phaseMoveDuration;
        while (rate < 1f)
        {
            deltaTime += Time.deltaTime;
            rate = deltaTime / phaseMoveDuration;
            phaseAlertText.transform.position = Vector3.Lerp(middlePosition, finalPosition, rate);
            yield return null;
        }

        this.GetComponent<Text>().enabled = false;
        isPhaseNoticeDone = true;
    }
}
