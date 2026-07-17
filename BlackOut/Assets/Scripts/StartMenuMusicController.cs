using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuMusicController : MonoBehaviour
{
    public AudioClip startScreenMusicClip;
    private AudioSource startMusicSource;

    private void Awake()
    {
        // Create a persistent GameObject to hold music AudioSource
        GameObject musicPlayer = new GameObject("StartMusicPlayer");
        startMusicSource = musicPlayer.AddComponent<AudioSource>();

        startMusicSource.clip = startScreenMusicClip;
        startMusicSource.loop = true;
        startMusicSource.playOnAwake = false;
        startMusicSource.volume = 0.3f;

        startMusicSource.Play();

        DontDestroyOnLoad(musicPlayer);
    }

    // Connect this method to your Start button's OnClick event in the Inspector
    public void OnStartButtonPressed(string sceneName)
    {
        StopMusic();
        SceneManager.LoadScene(sceneName);
    }

    private void StopMusic()
    {
        if (startMusicSource != null && startMusicSource.isPlaying)
        {
            startMusicSource.Stop();
            Debug.Log("Start screen music stopped.");
        }
    }
}
