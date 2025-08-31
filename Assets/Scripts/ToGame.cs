using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ToGame : MonoBehaviour
{
    private UIDocument uIDocument;
    private VisualElement fadeElement;

    private void Start()
    {
        uIDocument = GetComponent<UIDocument>();
        uIDocument.rootVisualElement.Q<VisualElement>("MainMenu").Q<UnityEngine.UIElements.Button>().clicked += ToGame_clicked;
        fadeElement = uIDocument.rootVisualElement.Q<VisualElement>("Fade");
        fadeElement.style.opacity = 0;
        fadeElement.style.display = DisplayStyle.None;
    }

    private void ToGame_clicked()
    {
        fadeElement.style.display = DisplayStyle.Flex;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for (int i = 0; i < 100; i++)
        {
            fadeElement.style.opacity = new StyleFloat((float)(i) / 100);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        SceneManager.LoadScene("Level1a");
    }
}
