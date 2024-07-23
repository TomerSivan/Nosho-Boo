using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private bool paused;

    public GameObject pauseButton;

    private PauseMeu pauseMenu;

    public BooMovement boo;
    public NoshoMovement nosho;

    void Start()
    {

        pauseMenu = pauseButton.GetComponent<PauseMeu>();
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (boo.isDead && nosho.isDead)
        {
            Debug.Log("hAPPEND");

            Invoke("Kill", 0.5f);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (paused == false)
            {
                paused = true;
                pauseMenu.Pause();
            }
            else
            {
                paused = false;
                pauseMenu.Resume();
            }
        }

    }
    void Kill()
    {
        pauseMenu.Dead();
    }
}
