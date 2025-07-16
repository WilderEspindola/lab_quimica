using UnityEngine;
using TMPro;

public class KeypadLock4 : MonoBehaviour
{
    [Header("Keypad 4 Config")]
    public TextMeshPro passCodeDisplay;
    public GameObject[] keyButtons;

    private string currentCode = "";
    private string savedCode = "?";

    // Sistema de eventos común para todos los keypads
    public static System.Action<char, int> OnKeypadValueChanged;
    public char associatedLetter = 'D'; // Asignar 'D' en el Inspector

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

        // Conversión segura y notificación
        int value = 0;
        if (savedCode != "?" && !int.TryParse(savedCode, out value))
        {
            savedCode = "?"; // Reset si no es numérico
        }

        OnKeypadValueChanged?.Invoke(associatedLetter, value);
        Debug.Log($"[Keypad4] {associatedLetter}={value}");
    }

    public string GetSavedCode() => savedCode;
}