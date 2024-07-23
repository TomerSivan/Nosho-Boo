using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveManager : MonoBehaviour
{
    public Grave[] graves;
    private int selectedGraveIndex = 0;
    private BooMovement player;

    void Start()
    {
        player = FindObjectOfType<BooMovement>();
    }

    void Update()
    {
        
    }

    public void SwitchToNextGrave()
    {
        graves[selectedGraveIndex].Highlight(false);
        selectedGraveIndex = (selectedGraveIndex + 1) % graves.Length;
        HighlightSelectedGrave();
    }

    public void SwitchToPreviousGrave()
    {
        graves[selectedGraveIndex].Highlight(false);
        selectedGraveIndex = (selectedGraveIndex - 1 + graves.Length) % graves.Length;
        HighlightSelectedGrave();
    }

    public void TeleportPlayerToSelectedGrave()
    {
        graves[selectedGraveIndex].Highlight(false);
        player.transform.position = new Vector3(0f,0f,0f);

        Vector3 targetPosition = graves[selectedGraveIndex].transform.position;
        player.transform.position = targetPosition;
        player.GetComponent<Renderer>().enabled = true;
        player.isDisappeared = false;
        graves[selectedGraveIndex].Particles();

        Invoke("DelayedDissapear", 0.2f);

    }
    void DelayedDissapear()
    {
        player.SetGraveTeleported(graves[selectedGraveIndex]);
    }

    public void SetFirstSelectedGrave(Grave checkingGrave)
    {
        for (int i = 0; i < graves.Length; i++)
        {
            if (graves[i] == checkingGrave)
            {
                selectedGraveIndex = i;
            }

        }
        HighlightSelectedGrave();
    }

    private void HighlightSelectedGrave()
    {
        graves[selectedGraveIndex].Highlight(true);
    }
}
