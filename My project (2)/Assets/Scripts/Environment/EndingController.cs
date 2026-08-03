using UnityEngine;

public class EndingController : MonoBehaviour
{
    public GameObject helpedTomasText;
    public GameObject didNotHelpTomasText;

    void Start()
    {
        bool helpedTomas =
            GameManager.Instance != null &&
            GameManager.Instance.helpedTomasEscape;

        helpedTomasText.SetActive(helpedTomas);
        didNotHelpTomasText.SetActive(!helpedTomas);
    }
}