using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public TMP_Text subtitles;
    private bool isDisplaying = false; // tracks if displaying
    private Coroutine displayCoroutine; // reference to coutrine

    public static SubtitleManager Instance;

    private void Awake()
    {
        // makes sure only one exists at a time
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplaySubtitles(string text, float letterDelay = 0.08f)
    {
        if (!isDisplaying) // if subtitles are currently not being displayed
        {
            displayCoroutine = StartCoroutine(DisplaySubtitlesCoroutine(text, letterDelay));
        }
    }

    private IEnumerator DisplaySubtitlesCoroutine(string text, float letterDelay)
    {
        isDisplaying = true; // set subtitles to being displayed 
        subtitles.text = "";
        int wordCount = 0; // tracks amount of words 
        for (int i = 0; i < text.Length; i++)
        {
            // for counting the words
            subtitles.text += text[i];
            if (text[i] == ' ' || text[i] == '\n')
            {
                wordCount++;
            }

            // if there are more than 20 words clears and starts a new sentence 
            if (wordCount >= 20)
            {
                wordCount = 0;
                subtitles.text = "";
                yield return new WaitForSeconds(letterDelay);
            }

            yield return new WaitForSeconds(letterDelay);
        }
        isDisplaying = false;
    }

    public void ClearSubtitles()
    {
        // stops the current subtitles anc clears it
        if (isDisplaying && displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            isDisplaying = false;
        }
        subtitles.text = "";
    }
}
