using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{


    string greetingMessage = "I'm Tim the Pumpkinfool!\nI have diabities,\nGive me 3 candy...";
    string timerMessage = "I want candy...";
    string notEnoughMessage = "THIS IS NOT ENOUGH\nCANDY!";
    string winMessage = "Thank you!\nI love candy...";
    int characterIndex = 0;
    int queue = 0;
    bool isTyping = false;
    bool isSatisfied = false;


    public float messageDisplayTime = 10f;
    private float messageStartTime;
    private int textRotation = 0;

    public float typingSpeed;

    public CandyManager cm;
    public TMP_Text textMesh;
    public AudioSource src;
    public AudioClip sfx;

    private float remainingTime;

    void Start()
    {
        src.clip = sfx;
        src.loop = true;
        textMesh.text = "";

        StartCoroutine(TypeText(greetingMessage));
        textRotation++;
    }

    private IEnumerator TypeText(string msg)
    {
        isTyping = true;
        messageStartTime = Time.time;
        remainingTime = messageDisplayTime;
        characterIndex = 0;
        textMesh.text = "";

        src.Play();
        while (characterIndex < msg.Length)
        {
            textMesh.text += msg[characterIndex];
            characterIndex++;
            yield return new WaitForSeconds(typingSpeed);
        }
        src.Stop();
        isTyping = false;

        if (msg == winMessage)
            isSatisfied = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTyping)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0 && !isSatisfied)
            {
                StartCoroutine(TypeText(textRotation % 2 == 0 ? greetingMessage : timerMessage));
                textRotation++;
            }

        }
    }

    public void CheckCandy()
    {
        if (!isSatisfied)
        {
            if (isTyping)
            {

                StartCoroutine(WaitForTypingAndCheckCandy());
            }
            else
            {
                remainingTime = messageDisplayTime;
                StartCoroutine(TypeText(cm.candyCount >= 3 ? winMessage : notEnoughMessage));
            }
        }
    }
    private IEnumerator WaitForTypingAndCheckCandy()
    {
        if (!isSatisfied)
        {
            while (isTyping)
            {
                yield return null;
            }
            remainingTime = messageDisplayTime;
            StartCoroutine(TypeText(cm.candyCount >= 3 ? winMessage : notEnoughMessage));
        }
        
    }
}
