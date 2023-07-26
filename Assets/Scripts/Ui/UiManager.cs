using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    void Start()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If pause screen already active unpause and viceversa
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }       
    }

    #region Game Over
    public void GameOver() // Activate over screen
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.Playsound(gameOverSound);
    }

    public void Restart() // Game over funcitons
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exits play mode ( will only be executed in the editor )
        #endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        // If status = true pause || If status = false unpause
        pauseScreen.SetActive(status);

        // When pause status is true change timescale to 0 ( time stops )
        // When it's false change it back to 1 ( time goes by normally )
        if (status)      
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}
