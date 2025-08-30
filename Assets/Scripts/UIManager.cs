using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement gameplayUI;
    private Label cookieText;
    private VisualElement sprintBarMoving;
    private VisualElement convoUI;
    private VisualElement convoBox;
    private Label convoLabel;
    private VisualElement convoIcon;
    [SerializeField] private Texture2D[] convoUIs;
    [SerializeField] private Texture2D[] headShotIcons;
    [SerializeField] private string[] convoTexts;
    private int convoIndex = 0;
    private float convoTimer;
    private VisualElement endingUI;
    private VisualElement endingUIContainer;
    [SerializeField]
    private VisualElement fadeUI;
    private float opacity = 2;

    void HideAll()
    {
        gameplayUI.style.display = DisplayStyle.None;
        convoUI.style.display = DisplayStyle.None;
        endingUI.style.display = DisplayStyle.None;
        fadeUI.style.display = DisplayStyle.None;
    }
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        gameplayUI = uiDocument.rootVisualElement.Q<VisualElement>("GameplayUI");
        cookieText = gameplayUI.Q<VisualElement>("CookieStatusContainer").Q<VisualElement>("CookieTextBox").Q<Label>("CookieText");
        sprintBarMoving = gameplayUI.Q<VisualElement>("SprintBarContainer").Q<VisualElement>("SprintBar").Q<VisualElement>("SprintBarMoving");
        convoUI = uiDocument.rootVisualElement.Q<VisualElement>("ConvoUI");
        convoBox = convoUI.Q<VisualElement>("ConvoBox");
        convoBox.RegisterCallback<ClickEvent>(NextTextClicked);
        convoBox.RegisterCallback<KeyDownEvent>(NextTextPressed, TrickleDown.TrickleDown);
        convoBox.focusable = true;
        convoBox.Focus();
        convoLabel = convoBox.Q<Label>("ConvoLabel");
        convoIcon = convoBox.Q<VisualElement>("ConvoIcon");
        endingUI = uiDocument.rootVisualElement.Q<VisualElement>("EndingUI");
        endingUIContainer = endingUI.Q<VisualElement>("EndingUIContainer");
        fadeUI = uiDocument.rootVisualElement.Q<VisualElement>("FadeUI");
        HideAll();
        fadeUI.style.display = DisplayStyle.Flex;
        convoUI.style.display = DisplayStyle.Flex;
    }

    private void OnDestroy()
    {
        convoBox.UnregisterCallback<ClickEvent>(NextTextClicked); 
        convoBox.UnregisterCallback<KeyDownEvent>(NextTextPressed);
    }


    void Update()
    {
        if (opacity >= 0)
        {
            opacity -= Time.unscaledDeltaTime / 6;
            fadeUI.style.opacity = new StyleFloat(opacity);
        }
        else
        {
            fadeUI.style.display = DisplayStyle.None;
            convoTimer += 15 * Time.unscaledDeltaTime;
        }
        
        cookieText.text = GameManager.Instance.biscuits.ToString() + "/" + Biscuit.totalBiscuits.ToString();
        sprintBarMoving.style.right = Length.Percent(100 - (GameManager.Instance.PlayerController.Stamina / 5) * 100);
        convoBox.style.backgroundImage = new StyleBackground(convoUIs[(int)(Time.unscaledTime * 3 % convoUIs.Length)]);
        convoIcon.style.backgroundImage = new StyleBackground(headShotIcons[(int)(Time.unscaledTime * 2 % headShotIcons.Length)]);
        convoLabel.text = convoTexts[convoIndex][..(int)Mathf.Clamp(convoTimer, 0, convoTexts[convoIndex].Length)];
    }

    void NextTextClicked(ClickEvent evt)
    {
        NextText();
    }

    void NextTextPressed(KeyDownEvent evt)
    {
        if (evt.keyCode != KeyCode.Space && evt.keyCode != KeyCode.Mouse0) return;
        NextText();
    }
    void NextText()
    {
        convoIndex++;
        convoTimer = 0;
        if (convoIndex >= convoTexts.Length)
        {
            convoIndex--;
            GameManager.Instance.StartGame();
            HideAll();
            gameplayUI.style.display = DisplayStyle.Flex;
            convoUI.pickingMode = PickingMode.Ignore;
            convoBox.focusable = false;
        }
    }

    public void ShowEnding(int index)
    {
        StartCoroutine(ShowEnding());
        gameplayUI.style.display = DisplayStyle.None;
        endingUI.style.display = DisplayStyle.Flex;
        endingUIContainer.style.display = DisplayStyle.None;
        endingUI.BringToFront();
        endingUIContainer.Q<UnityEngine.UIElements.Button>().clicked += TriggerRestart;
        switch (index)
        {
            case 0:
                endingUIContainer.Q<Label>().text = "Man, you need to be careful! Don't get caught by the chefs patrolling.";
                break;
            case 1:
                endingUIContainer.Q<Label>().text = "How'd you even manage that?";
                break;
            case 2:
                endingUIContainer.Q<Label>().text = "Was the golden biscuit... a lie?";
                break;
            case 3:
                endingUIContainer.Q<Label>().text = "Thanks for playing!";
                break;
        }
    }

    IEnumerator ShowEnding()
    {
        yield return new WaitForSeconds(3);
        endingUIContainer.style.display = DisplayStyle.Flex;
    }

    void TriggerRestart()
    {
        endingUI.Q<UnityEngine.UIElements.Button>().clicked -= TriggerRestart;
        GameManager.Instance.Restart();
    }
}
