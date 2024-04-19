using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TwoCastlesLevel : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip monologAudioClip;
    public AudioClip angryResponseAudioClip;
    public AudioClip myLeftAudioClip;
    public AudioClip leftNotRightAudioClip;
    public AudioClip goodJobAudioClip;
    public AudioClip wrong1AudioClip;
    public AudioClip wrong2AudioClip;
    public AudioClip wrong3AudioClip;
    public Button castle1;
    public Button castle2;
    public Canvas canvas;

    private bool canClick = false;
    private bool monologFinished = false;
    private int clickCount = 0;
    private bool angryResponsePlayed = false;
    private bool isCastle1Clicked = false;
    private bool isCastle2Clicked = false;
    private bool castle1IsCorrect = false;
    private bool castle2IsCorrect = false;
    private int wrongClicks = 0;

    void Start()
    {
        // gotta make sure they cant be clicked on right at the start
        castle1.interactable = false;
        castle2.interactable = false;

        // runs the level set up
        StartCoroutine(SetupLevel());
    }

    IEnumerator SetupLevel()
    {
        // plays monolog
        audioSource.clip = monologAudioClip;
        audioSource.Play();
        SubtitleManager.Instance.ClearSubtitles();
        SubtitleManager.Instance.DisplaySubtitles("Once upon a time there was a knight with a very dapper pig... sadly a masked unknown figure took his dapper pig and ran off to hide at a top of a castle. The knight had to go through a tretorus journey needing to fight all kind of monsters such as dragons and zombies... and yes even spiders. However the knight was determined to get his dapper pig back and now has made it to the 2 castles all he has to do is go to the castle on the LEFT");

        yield return new WaitForSeconds(audioSource.clip.length);

        // make it so angry response cant play after the narrator does monolog
        if (!angryResponsePlayed)
        {
            monologFinished = true;
            canClick = true;
            EnableButtons();
        }
        else
        {
            canClick = true; // allow you to be able to click on the screen
        }
    }

    void Update()
    {
        // check if there are clicks(more than or equal 3 to be exact
        if (!monologFinished && Input.GetMouseButtonDown(0))
        {
            if (!angryResponsePlayed)
            {
                clickCount++;

                if (clickCount >= 2)
                {
                    PlayAngryResponseAudio();
                    angryResponsePlayed = true;
                    clickCount = 0;
                }
            }
        }
    }

    void PlayAngryResponseAudio()
    {
        audioSource.Stop(); // stop monolog
        audioSource.clip = angryResponseAudioClip;
        audioSource.Play();
        SubtitleManager.Instance.ClearSubtitles();
        SubtitleManager.Instance.DisplaySubtitles("WHY DO YOU KEEP CLICKING! Do you know what if you don't want to hear my story that bad i won't tell you it just go to the castle on the LEFT!");

        // wait for the angry response audio to finish
        StartCoroutine(WaitForAngryResponse());
    }

    IEnumerator WaitForAngryResponse()
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        // enable the buttons/click after the anger
        EnableButtons();
        canClick = true;
    }

    void EnableButtons()
    {
        //makes the button interactable
        castle1.interactable = true;
        castle2.interactable = true;
    }

    public void OnCastle1Click()
    {
        // if the button can be clicked and hasnt been clicked before
        if (canClick && !isCastle1Clicked)
        {
            // make sure to sure to note it down as being clicked for incorrect logic to work
            isCastle1Clicked = true;
            // if its the correct castle run correct
            if (castle1IsCorrect)
            {
                Correct();
            }
            // if it isnt then make the other castle correct
            else
            {
                // plays my left audio
                MyLeft();
                castle2IsCorrect = true;
            }
        }
        // if the castles been clicked before aka the incorrect castle then run incorrect
        else if (canClick && isCastle1Clicked) 
        { 
           Incorrect();
        }
    }

    public void OnCastle2Click()
    {
        // if the button can be clicked and hasnt been clicked before
        if (canClick && !isCastle2Clicked)
        {
            // make sure to sure to note it down as being clicked for incorrect logic to work
            isCastle2Clicked = true;
            // if its the correct castle run correct
            if (castle2IsCorrect)
            {
                Correct();
            }
            // if it isnt then make the other castle correct
            else
            {
                // plays i said left not right audio clip
                LeftNotRight();
                castle1IsCorrect = true;
            }
        }
        // if the castles been clicked before aka the incorrect castle then run incorrect
        else if (canClick && isCastle2Clicked)
        {
            Incorrect();
        }
    }

    // plays audio
    void MyLeft()
    {
        audioSource.clip = myLeftAudioClip;
        audioSource.Play();
        SubtitleManager.Instance.ClearSubtitles();
        SubtitleManager.Instance.DisplaySubtitles("Ughhhh noooooooo. Not your left MY left");
    }

    // plays audio
    void LeftNotRight()
    {
        audioSource.clip = leftNotRightAudioClip;
        audioSource.Play();
        SubtitleManager.Instance.ClearSubtitles();
        SubtitleManager.Instance.DisplaySubtitles("Can you follow instructions? I said the castle on the left not the castle on the right");
    }

    // plays audio and moves scene
    void Correct()
    {
        audioSource.clip = goodJobAudioClip;
        audioSource.Play();
        SubtitleManager.Instance.ClearSubtitles();
        SubtitleManager.Instance.DisplaySubtitles("Well done, you were finally able to follow basic instructions and go to the right castle. Good Job");
        StartCoroutine(WaitAndMove("Reception"));
    }

    // used for checking if incorrect
    void Incorrect()
    {
        // increments wrong clicks and for every time you click wrongs plays a different warning
        wrongClicks++;
        switch (wrongClicks)
        {
            case 1:
                audioSource.clip = wrong1AudioClip;
                SubtitleManager.Instance.ClearSubtitles();
                SubtitleManager.Instance.DisplaySubtitles("I bet you want to go to the wrong castle just to spite me... you know what go ahead be my guest");
                break;
            case 2:
                audioSource.clip = wrong2AudioClip;
                SubtitleManager.Instance.ClearSubtitles();
                SubtitleManager.Instance.DisplaySubtitles("Do you know what since i'm sooooo nice ill give you one more chance to go the the correct castle.");
                break;
            case 3:
                audioSource.clip = wrong3AudioClip;
                SubtitleManager.Instance.ClearSubtitles();
                SubtitleManager.Instance.DisplaySubtitles("Ok dont tell me i didn't warn you enjoy what you find there");
                break;
        }

        // if you click 3 or more times it will move you to the bad castle
        if (wrongClicks >= 3)
        {
            StartCoroutine(WaitAndMove("BadCastle"));
        }

        audioSource.Play();
    }

    // makes sure to play the audio first then move
    IEnumerator WaitAndMove(string sceneName)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene(sceneName);
    }
}