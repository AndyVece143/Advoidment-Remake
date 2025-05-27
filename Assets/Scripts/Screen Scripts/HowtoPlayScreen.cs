using UnityEngine;
using UnityEngine.SceneManagement;

public class HowtoPlayScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
