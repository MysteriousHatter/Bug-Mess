using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StylizedCountdown : MonoBehaviour
{
    public TMP_Text countdownText;  // Reference to the UI Text
    public int countdownTime = 3;  // Start from 3 seconds


    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        int currentTime = countdownTime;

        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("0");  // Update the text to display the current time
            yield return new WaitForSeconds(1.5f);  // Wait for 1 second before counting down
            currentTime--;
        }

        // Final state after countdown ends
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);  // Keep the "Go!" message for a second, then hide
        GameManager.instance.objecctsCanMove = true;
        countdownText.gameObject.SetActive(false);  // Optionally hide the countdown UI
    }

 
}
