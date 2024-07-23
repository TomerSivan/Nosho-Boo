using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public Sprite[] graveSprites;
    public Sprite[] graveSpritesOutline;
    private int graveIndex;
    private bool isHighlighted = false;

    public ParticleSystem graveEffect;

    void Start()
    {
        graveIndex = Random.Range(0, graveSprites.Length);
        GetComponent<SpriteRenderer>().sprite = graveSprites[graveIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Particles()
    {
        graveEffect.Play();
    }

    public void Highlight(bool isHighlighted)
    {
        this.isHighlighted = isHighlighted;
        if (isHighlighted)
            GetComponent<SpriteRenderer>().sprite = graveSpritesOutline[graveIndex];
        else
            GetComponent<SpriteRenderer>().sprite = graveSprites[graveIndex];
    }
}
