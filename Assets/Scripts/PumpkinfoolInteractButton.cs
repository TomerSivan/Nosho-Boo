using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinfoolInteractButton : MonoBehaviour
{
    public ChatBubble cb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.E))
        {
            cb.CheckCandy();
        }
    }
}
