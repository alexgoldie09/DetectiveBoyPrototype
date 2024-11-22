using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneTransition sceneTransition; // Reference to SceneTransition
    public string gameSceneTitle; // Reference to the name of the game scene

    public void StartGame()
    {
        sceneTransition.StartGame(gameSceneTitle); // Replace "GameScene" with your actual scene name
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            // Stop play mode in the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}