using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Light2D globalLight;
    public Light2D playerLight;
    public float lightDrainRate = 1f;
    public float batteryDrainrate = 0.5f;
    public float minLightRadius = 0.5f;
    public float blackoutDuration = 120f;

    public AudioClip crowdCheerClip;
    public AudioClip gameOverSound; // ✅ New: Game over sound
    public AudioSource audioSource;

    public Slider batteryBar;
    public Image batteryFillImage;
    public Color normalColor = Color.green;
    public Color lowBatteryColor = Color.red;

    public TextMeshProUGUI countdownText;
    public GameObject gameOverUI;
    public GameObject winScreenUI;

    private float timeRemaining;
    private bool powerRestored = false;
    private float maxLightRadius = 10f;
    private bool gameEnded = false;

    [Header("Heartbeat")]
    public AudioClip heartbeatClip;
    private AudioSource heartbeatSource;
    private bool isHeartbeatPlaying = false;

    [Header("Werewolf Howl Sound")]
    public AudioClip werewolfHowlClip;  // Assign this in Inspector
    public float minHowlDelay = 10f;
    public float maxHowlDelay = 30f;
    private AudioSource werewolfAudioSource;
    private float howlTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeRemaining = blackoutDuration;

        if (batteryBar != null)
        {
            batteryBar.minValue = 0f;
            batteryBar.maxValue = 1f;
            batteryBar.value = 1f;
        }

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (winScreenUI != null)
            winScreenUI.SetActive(false);

        // Setup heartbeat audio source
        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.clip = heartbeatClip;
        heartbeatSource.loop = true;
        heartbeatSource.playOnAwake = false;
        heartbeatSource.volume = 1f;

        // Setup werewolf howl audio source
        werewolfAudioSource = gameObject.AddComponent<AudioSource>();
        werewolfAudioSource.playOnAwake = false;
        werewolfAudioSource.loop = false;
        werewolfAudioSource.volume = 1f;

        howlTimer = Random.Range(minHowlDelay, maxHowlDelay);

        StartCoroutine(BlackoutCountdown());
    }

    private void Update()
    {
        if (gameEnded) return;

        playerLight.pointLightOuterRadius -= lightDrainRate * Time.deltaTime;

        if (playerLight.pointLightOuterRadius <= minLightRadius)
        {
            GameOver();
        }

        float currentPercent = playerLight.pointLightOuterRadius / maxLightRadius;

        // Battery bar update
        if (batteryBar != null)
        {
            batteryBar.value = currentPercent;

            if (batteryFillImage != null)
            {
                batteryFillImage.color = currentPercent < 0.25f ? lowBatteryColor : normalColor;
            }
        }

        // Countdown update
        if (!powerRestored && countdownText != null)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0f, timeRemaining);
            countdownText.text = "Power returns in: " + Mathf.CeilToInt(timeRemaining) + "s";
        }

        // Heartbeat logic
        if (!gameEnded)
        {
            if (currentPercent < 0.4f && !isHeartbeatPlaying)
            {
                heartbeatSource.Play();
                isHeartbeatPlaying = true;
            }
            else if (currentPercent >= 0.4f && isHeartbeatPlaying)
            {
                heartbeatSource.Stop();
                isHeartbeatPlaying = false;
            }
        }
        else
        {
            if (isHeartbeatPlaying)
            {
                heartbeatSource.Stop();
                isHeartbeatPlaying = false;
            }
        }

        // Werewolf howl timer (using unscaled delta time to keep working when game is paused)
        if (!gameEnded)
        {
            howlTimer -= Time.unscaledDeltaTime;
            if (howlTimer <= 0f)
            {
                PlayHowlSound();
                howlTimer = Random.Range(minHowlDelay, maxHowlDelay);
            }
        }
    }

    public void AddLight(float amount)
    {
        playerLight.pointLightOuterRadius = Mathf.Min(
            playerLight.pointLightOuterRadius + amount,
            maxLightRadius
        );
    }

    void GameOver()
    {
        if (gameEnded) return;

        Debug.Log("Game Over – Light ran out");
        gameEnded = true;

        // Stop heartbeat
        if (heartbeatSource != null && isHeartbeatPlaying)
        {
            heartbeatSource.Stop();
            isHeartbeatPlaying = false;
        }

        // Play game over sound
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        Time.timeScale = 0f;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator BlackoutCountdown()
    {
        yield return new WaitForSeconds(blackoutDuration);

        if (gameEnded) yield break;

        globalLight.intensity = 1f;

        if (audioSource != null && crowdCheerClip != null)
        {
            audioSource.PlayOneShot(crowdCheerClip);
        }

        powerRestored = true;

        if (countdownText != null)
            countdownText.text = "Power Restored!";

        if (winScreenUI != null)
            winScreenUI.SetActive(true);

        Time.timeScale = 0f;
    }

    private void PlayHowlSound()
    {
        if (werewolfHowlClip != null && !werewolfAudioSource.isPlaying)
        {
            werewolfAudioSource.pitch = Random.Range(0.95f, 1.05f);
            werewolfAudioSource.PlayOneShot(werewolfHowlClip);
            Debug.Log("🧟 Werewolf howled!");
        }
    }
}
