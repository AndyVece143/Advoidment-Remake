using UnityEngine;
using UnityEngine.SceneManagement;
public class MainGameScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
