using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialougeBehaviour : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialougeText;
    public GameObject dialougeBox; 

    private Queue<string> sentences;


    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialouge(Dialouge dialouge) {

        nameText.text = dialouge.name;

        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
            Debug.Log("Added sentence to queue: " + sentence);
        }

        DisplayNextSentence();

    }

   public void DisplayNextSentence() { 
        if (sentences.Count == 0) {
            EndDialouge();
            return;
        }

       string sentence = sentences.Dequeue();
       dialougeText.text = sentence;
    }
    
    void EndDialouge()
    {
        Debug.Log("end of convo");
    }

    public void ShowDialogue()
    {
        dialougeBox.SetActive(true);
    }

    public void HideDialogue()
    {
        dialougeBox.SetActive(false);
    }

}
