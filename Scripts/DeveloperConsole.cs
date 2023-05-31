using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeveloperConsole : MonoBehaviour
{
    [SerializeField] GameObject consoleContainer;
    [SerializeField]TextMeshProUGUI consoleText;
    [SerializeField] RectTransform textContainer;
    [SerializeField] Button clearButton; 
    [SerializeField] Button minimizeButton; 
    [SerializeField] Button maximizeButton; 
    [SerializeField] Button enterButton; 
    [Header("Commands")]
    [SerializeField] CommandsProcessor commandsProcessor;
    [SerializeField] TMP_InputField commandsInput;
    

    private void Awake()
    {
        try
        {
            InitializeConsoleStyle();
            Application.logMessageReceived += HandleLog;
            InitializeConsoleButtons();

            print("<color=green> <DevConsole> Console was successfully initialized!</color>");
        }
        catch {
            Debug.LogWarning("Dev Console couldn't be initialized");
        }

        try
        {
            commandsInput.onSubmit.AddListener(ProcessInput);
        }
        catch
        {
            Debug.LogWarning("Console commands couldn't be initialized");
        }
    }

    private void InitializeConsoleStyle()
    {
        consoleText.overflowMode = TextOverflowModes.Truncate;
        consoleText.alignment = TextAlignmentOptions.BottomLeft;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }


    void InitializeConsoleButtons()
    {
        minimizeButton.onClick.AddListener(() => { Minimize(); });
        maximizeButton.onClick.AddListener(() => { Maximize(); });
        clearButton.onClick.AddListener(() => { ClearConsole(); });
        enterButton.onClick.AddListener(()=> { ProcessInput(commandsInput.text); });
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        consoleText.text += $"[{DateTime.Now.ToString("HH:mm:ss")}]>{GetColorTag(type)}{logString}</color>\n";
        ResizeTextContainer();
    }

    private void ResizeTextContainer()
    {
        textContainer.sizeDelta = new Vector2(textContainer.sizeDelta.x, GetTextHeight());
        textContainer.anchoredPosition = new Vector2(textContainer.anchoredPosition.x, GetTextHeight());
    }

    string GetColorTag(LogType type)
    {
        string color;
        switch (type)
        {
            case LogType.Warning:
                color = "yellow";
                break;
            case LogType.Error:
            case LogType.Exception:
                color = "red";
                break;
            default:
                color = "white";
                break;
        }
        return $"<color={color}>";
    }

    float GetTextHeight()
    {
        return consoleText.preferredHeight;
    }

    public void ClearConsole() {
        consoleText.text = string.Empty;
        ResizeTextContainer();
    }
    public void Minimize() {
        consoleContainer.SetActive(false);
        maximizeButton.gameObject.SetActive(true);
    }

    public void Maximize() {
        consoleContainer.SetActive(true);
        maximizeButton.gameObject.SetActive(false);
    }

    public void ProcessInput(string input) {
        commandsProcessor?.ProcessCommand(input);
        commandsInput.text = string.Empty;
    }
}