using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour
{
    // for the audio source component and audio clip
    public AudioSource audioSource;
    public AudioClip audioClip;

    public void StartGameAudio()
    {
        // if they assigned yknow do set the clip, play the clip then wait to load the scene
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            StartCoroutine(LoadScene());
        }
        // logs error if there is no audio source/clip
        else
        {
            UnityEngine.Debug.LogError("NO SOURCE!!!!!");
        }
    }

    IEnumerator LoadScene()
    {
        // loads the next scene once the audio finishes
        yield return new WaitForSeconds(audioClip.length);
        SceneManager.LoadScene("TwoCastles");
    }
}