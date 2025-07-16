using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FlowManager : MonoBehaviour
{
    // Valores aleatorios de las letras (A-F)
    public int A, B, C, D, E, F;

    // Diccionario para almacenar valores de keypads (?A-?F)
    private Dictionary<char, int> keypadValues = new Dictionary<char, int>();

    // Referencias existentes (no tocar)
    public List<GameObject> Sockets = new List<GameObject>();
    public GameObject HandAnimation;
    public bool IsPracticeMode = false;

    void Start()
    {
        InitializeKeypadValues();
        InitializeGame();
        SubscribeToKeypadEvents();
    }

    private void InitializeKeypadValues()
    {
        keypadValues.Add('A', 0); // ?A inicia en 0
        keypadValues.Add('B', 0); // ?B inicia en 0
        keypadValues.Add('C', 0);
        keypadValues.Add('D', 0);
        keypadValues.Add('E', 0);
        keypadValues.Add('F', 0);
    }

    private void SubscribeToKeypadEvents()
    {
        KeypadLock.OnKeypadValueChanged += UpdateKeypadValue;  // Para 'A'
        KeypadLock2.OnKeypadValueChanged += UpdateKeypadValue; // Para 'B'
        KeypadLock3.OnKeypadValueChanged += UpdateKeypadValue; // Para 'C'
        KeypadLock4.OnKeypadValueChanged += UpdateKeypadValue; // Para 'D'
        KeypadLock5.OnKeypadValueChanged += UpdateKeypadValue; // Para 'E'
        KeypadLock6.OnKeypadValueChanged += UpdateKeypadValue; // Para 'F'
    }

    private void UpdateKeypadValue(char letter, int value)
    {
        if (keypadValues.ContainsKey(letter))
        {
            keypadValues[letter] = value;
            Debug.Log($"[Keypad] {letter} = {value}");
        }
    }

    private void InitializeGame()
    {
        GameManager.Instance.UI_Messages.text = "Usa ✋ Thumbs Up para practicar o 🤙 Shaka para comenzar";
        GameManager.Instance.Timer.enabled = false;
        GameManager.Instance.MathematicsValues.gameObject.SetActive(false);
        GameManager.Instance.RightThumbsUp.gameObject.SetActive(true);
        GameManager.Instance.RightShaka.gameObject.SetActive(true);
        GameManager.Instance.LeftThumbsUp.gameObject.SetActive(false);
        DisableSockets();
        HandAnimation.SetActive(false);
        IsPracticeMode = false;
    }

    public void RightHandThumpsUpPerformed()
    {
        IsPracticeMode = true;
        GameManager.Instance.UI_Messages.text = "Modo Práctica: Usa los keypads para ajustar valores. ✋ Thumbs Up izquierdo para salir.";
        GameManager.Instance.RightThumbsUp.gameObject.SetActive(false);
        GameManager.Instance.RightShaka.gameObject.SetActive(false);
        GameManager.Instance.LeftThumbsUp.gameObject.SetActive(true);
        GameManager.Instance.MathematicsValues.gameObject.SetActive(true);
        EnableSockets();
        HandAnimation.SetActive(true);
    }

    public void RightShakaPerformed()
    {
        IsPracticeMode = false;
        GameManager.Instance.UI_Messages.text = "¡Comienza el juego! Ajusta los keypads para igualar ambos lados.";
        GameManager.Instance.RightThumbsUp.gameObject.SetActive(false);
        GameManager.Instance.RightShaka.gameObject.SetActive(false);
        GameManager.Instance.MathematicsValues.gameObject.SetActive(true);
        EnableSockets();
        GenerateValuesABCDEF();
        StartCountdown();
    }

    public void StartCountdown()
    {
        GameManager.Instance.Timer.enabled = true;
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        int countdownTime = 10;
        while (countdownTime >= 0)
        {
            GameManager.Instance.Timer.GetComponent<TextMeshProUGUI>().text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        OnCountdownFinished();
    }

    private void OnCountdownFinished()
    {
        GameManager.Instance.Timer.enabled = false;
        CalculateValue();
    }

    public void CalculateValue()
    {
        int leftSum = (keypadValues['A'] + A) + (keypadValues['B'] + B) + (keypadValues['C'] + C);
        int rightSum = (keypadValues['D'] + D) + (keypadValues['E'] + E) + (keypadValues['F'] + F);

        if (leftSum == rightSum)
        {
            GameManager.Instance.UI_Messages.text = $"✅ Correcto! {leftSum} = {rightSum}\nUsa ✋ Thumbs Up izquierdo para jugar de nuevo.";
            GameManager.Instance.LeftThumbsUp.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.UI_Messages.text = $"❌ Incorrecto! {leftSum} ≠ {rightSum}\nUsa ✋ Thumbs Up izquierdo para reintentar.";
            GameManager.Instance.LeftThumbsUp.gameObject.SetActive(true);
        }
    }

    public void LeftHandThumpsUpPerformed()
    {
        if (IsPracticeMode)
        {
            InitializeGame();
        }
        else
        {
            RestartScene();
        }
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void DisableSockets()
    {
        foreach (var socket in Sockets)
        {
            if (socket != null)
            {
                socket.SetActive(false);
                socket.GetComponent<XRSocketInteractor>().enabled = false;
            }
        }
    }

    private void EnableSockets()
    {
        foreach (var socket in Sockets)
        {
            if (socket != null)
            {
                socket.SetActive(true);
                socket.GetComponent<XRSocketInteractor>().enabled = true;
            }
        }
    }

    public void GenerateValuesABCDEF()
    {
        int[] validNumbers = { 1, 2, 3 };
        A = validNumbers[Random.Range(0, validNumbers.Length)];
        B = validNumbers[Random.Range(0, validNumbers.Length)];
        C = validNumbers[Random.Range(0, validNumbers.Length)];
        D = validNumbers[Random.Range(0, validNumbers.Length)];
        E = validNumbers[Random.Range(0, validNumbers.Length)];
        F = validNumbers[Random.Range(0, validNumbers.Length)];

        Transform values = GameManager.Instance.MathematicsValues.transform;
        try
        {
            values.GetChild(0).GetComponent<TextMeshPro>().text = A.ToString(); // A
            values.GetChild(2).GetComponent<TextMeshPro>().text = B.ToString(); // B
            values.GetChild(4).GetComponent<TextMeshPro>().text = C.ToString(); // C
            values.GetChild(6).GetComponent<TextMeshPro>().text = D.ToString(); // D
            values.GetChild(8).GetComponent<TextMeshPro>().text = E.ToString(); // E
            values.GetChild(10).GetComponent<TextMeshPro>().text = F.ToString(); // F
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error asignando valores: " + e.Message);
        }
    }

    void OnDestroy()
    {
        // Limpieza de eventos
        KeypadLock.OnKeypadValueChanged -= UpdateKeypadValue;
        KeypadLock2.OnKeypadValueChanged -= UpdateKeypadValue;
        KeypadLock3.OnKeypadValueChanged -= UpdateKeypadValue;
        KeypadLock4.OnKeypadValueChanged -= UpdateKeypadValue;
        KeypadLock5.OnKeypadValueChanged -= UpdateKeypadValue;
        KeypadLock6.OnKeypadValueChanged -= UpdateKeypadValue;
    }
}