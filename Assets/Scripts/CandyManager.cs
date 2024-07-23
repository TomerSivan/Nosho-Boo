using UnityEngine;
using UnityEngine.UI;

public class CandyManager : MonoBehaviour
{
    public Text candyCounterText;
    public int candyCount = 0;

    private void Start()
    {
        UpdateCandyCounter();
    }

    private void UpdateCandyCounter()
    {
        candyCounterText.text = "Candies: " + candyCount.ToString();
    }

    public void CollectCandy()
    {
        candyCount++;
        UpdateCandyCounter();
    }
}