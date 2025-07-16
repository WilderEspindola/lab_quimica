using UnityEngine;
using TMPro;

public class KeypadLock2 : MonoBehaviour
{
    [Header("Referencias - Keypad 2")]
    public TextMeshPro passCodeDisplay;
    public GameObject[] keyButtons;

    private string currentCode = "";
    private string savedCode = "?";

    // Evento estático para notificar cambios
    public static System.Action<char, int> OnKeypadValueChanged;
    public char associatedLetter = 'B'; // Asignar en Inspector (B para KeypadLock2)

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
        savedCode = string.IsNullOrEmpty(currentCode) ? "?" : currentCode;
        SetKeypadVisible(false);

        // Notificar al FlowManager (Nuevo)
        int value = savedCode == "?" ? 0 : int.Parse(savedCode);
        OnKeypadValueChanged?.Invoke(associatedLetter, value);

        Debug.Log($"Keypad2 ({associatedLetter}) guardó: {savedCode}");
    }

    public string GetSavedCode() => savedCode;
}