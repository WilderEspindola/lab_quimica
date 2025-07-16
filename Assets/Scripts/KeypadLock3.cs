using UnityEngine;
using TMPro;

public class KeypadLock3 : MonoBehaviour
{
    [Header("Configuraci�n Keypad 3")]
    public TextMeshPro passCodeDisplay;
    public GameObject[] keyButtons; // Botones espec�ficos de ESTE keypad

    private string currentCode = "";
    private string savedCode = "?";

    void Start()
    {
        passCodeDisplay.text = savedCode;
        SetKeypadVisible(false);
    }

    // M�todos con sufijo "3" para evitar conflicto
    public void ToggleKeypad3()
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
        if (currentCode.Length < 10)
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
        Debug.Log("[Keypad3] C�digo guardado: " + savedCode);
    }
}