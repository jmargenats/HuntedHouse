using UnityEngine;

public class SofiaPuzzle : MonoBehaviour
{
    public LetterSlot[] slots;
    public string correctWord = "SOFIA";

    public GameObject objectToActivate;

    private bool solved = false;
    public bool IsSolved => solved;

    void Update()
    {
        if (solved) return;

        if (slots.Length != correctWord.Length) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogWarning("SofiaPuzzle tiene un slot sin asignar.");
                return;
            }

            if (slots[i].currentCube == null)
                return;

            if (char.ToUpperInvariant(slots[i].currentCube.letter) != char.ToUpperInvariant(correctWord[i]))
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
