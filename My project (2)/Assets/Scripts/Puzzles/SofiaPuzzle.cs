using UnityEngine;

public class SofiaPuzzle : MonoBehaviour
{
    public LetterSlot[] slots;
    public string correctWord = "SOFIA";

    public GameObject objectToActivate;

    private bool solved = false;

    void Update()
    {
        if (solved) return;

        if (slots.Length != correctWord.Length) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentCube == null)
                return;

            if (slots[i].currentCube.letter.ToString() != correctWord[i].ToString())
                return;
        }

        solved = true;
        SolvePuzzle();
    }

    void SolvePuzzle()
    {
        Debug.Log("Puzzle SOFIA resuelto");

        if (objectToActivate != null)
            objectToActivate.SetActive(true);
    }
}