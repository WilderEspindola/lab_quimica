using UnityEngine;
using TMPro;

public class KeypadLock3 : MonoBehaviour
{
    [Header("Configuración Keypad 3")]
    public TextMeshPro passCodeDisplay;
    public GameObject[] keyButtons;

    private string currentCode = "";
    private string savedCode = "?";

    // Evento estático para notificar cambios
    public static System.Action<char, int> OnKeypadValueChanged;
    public char associatedLetter = 'C'; // Asignar en Inspector (C para KeypadLock3)

    void Start()
    {
        passCodeDisplay.text = savedCode;
        SetKeypadVisible(false);
    }

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
        savedCode = string.IsNullOrEmpty(currentCode) ? "?" : currentCode;
        SetKeypadVisible(false);

        // Notificar al FlowManager (Nuevo)
        if (!int.TryParse(savedCode, out int value))
        {
            value = savedCode == "?" ? 0 : 0; // Fuerza 0 si no es número
            savedCode = "?"; // Opcional: Resetear si es inválido
        }
        OnKeypadValueChanged?.Invoke(associatedLetter, value);

        Debug.Log($"[Keypad3] {associatedLetter}={savedCode}");
    }

    public string GetSavedCode() => savedCode;
}