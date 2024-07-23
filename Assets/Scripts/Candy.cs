using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public Sprite[] candySprites;
    // Start is called before the first frame update
    void Start()
    {
        int randomIndex = Random.Range(0, candySprites.Length);
        GetComponent<SpriteRenderer>().sprite = candySprites[randomIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
