using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject startScreenUI;     // Assign your StartScreenUI
    public GameObject gameplayObjects;   // Assign parent of all gameplay objects

    public AudioClip startScreenMusicClip;  // Assign your start screen music clip in Inspector

    private AudioSource musicSource;

    private void Awake()
    {
        // Create and configure AudioSource for music
        GameObject musicPlayer = new GameObject("StartMusicPlayer");
        musicSource = musicPlayer.AddComponent<AudioSource>();
        musicSource.clip = startScreenMusicClip;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.3f;

        musicSource.Play();

        DontDestroyOnLoad(musicPlayer);
    }

    private void Start()
    {
        gameplayObjects.SetActive(false);   // Hide gameplay at start
        startScreenUI.SetActive(true);      // Show menu
    }

    public void StartGame()
    {
        startScreenUI.SetActive(false);     // Hide menu
        gameplayObjects.SetActive(true);    // Enable gameplay
        
        // Stop the music
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
