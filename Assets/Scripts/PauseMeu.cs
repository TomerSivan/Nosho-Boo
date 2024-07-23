using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMeu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameObject deathScreen;

    [SerializeField] Sprite mutedImage;
    [SerializeField] Sprite unmutedImage;

    [SerializeField] Image mutedIcon;


    [SerializeField] bool isMuted;

    private bool dead;


    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

    }
    public void Dead()
    {
        dead = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }
    public void MuteUnmute()
    {
        if (isMuted)
        {
            isMuted = false;
            mutedIcon.sprite = mutedImage;

            AudioListener.volume = 1;
        }
        else
        {
            isMuted = true;
            mutedIcon.sprite = unmutedImage;
            AudioListener.volume = 0;

        }

    }

    void Start()
    {
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        isMuted = false;
        dead = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            
            deathScreen.SetActive(true);
            Time.timeScale = 0f;
            
        }
    }
}
