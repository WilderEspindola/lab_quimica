using UnityEngine;
using TMPro;

public class KeypadLock5 : MonoBehaviour
{
    [Header("Referencias Keypad 5")]
    public TextMeshPro passCodeDisplay; // TextMeshPro est�ndar
    public GameObject[] keyButtons; // Array de botones espec�ficos

    private string currentCode = "";
    private string savedCode = "?";

    void Start()
    {
        passCodeDisplay.text = savedCode;
        SetKeypadVisible(false);
    }

    public void ToggleKeypad()
    {
        bool newState = !keyButtons[0].activeSelf;
        SetKeypadVisible(newState);
        if (newState) currentCode = "";
    }

    private void SetKeypadVisible(bool visible)
    {
        foreach (var button in keyButtons)
            button.SetActive(visible);

        passCodeDisplay.text = visible ? currentCode : savedCode;
    }

    public void AddDigit(string digit)
    {
        if (currentCode.Length < 10) // L�mite de 10 d�gitos
            currentCode += digit;

        passCodeDisplay.text = currentCode;
    }

    public void ClearCode()
    {
        currentCode = "";
        passCodeDisplay.text = currentCode;
    }

    public void SaveCode()
    {
        savedCode = currentCode;
        if (string.IsNullOrEmpty(savedCode)) savedCode = "?";
        SetKeypadVisible(false);
        Debug.Log("Keypad5 - C�digo guardado: " + savedCode);
    }
}