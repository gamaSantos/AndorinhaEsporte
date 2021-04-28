using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    Button startButton;
    UIDocument document;

    void Start()
    {
        document = GetComponent<UIDocument>();
        startButton = document.rootVisualElement.Q<Button>(name: "Play");

        startButton.clicked += PlayButtonClicked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Action PlayButtonClicked = () =>
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    };
}
