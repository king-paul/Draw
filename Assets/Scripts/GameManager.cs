using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingLives = 5;
    [Range(1, 1000)]
    [SerializeField] int pointsPerLevel = 500;    
    [SerializeField] ObjectSpawner[] spawners;

    [Header("% Adjustments Per level")]
    [Range(0,100)]
    [SerializeField] float objectSpeedIncrease = 3.0f;
    [Range(0, 100)]
    [SerializeField] float fireRateIncrease = 3.0f;

    [Header("Heads Up Display")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;

    [Header("Pause Menu")]
    public GameObject menuGUI;

    [Header("Sound Effects")]
    public AudioClip progressLevelSound;
    public AudioClip gameOverSound;
    public AudioClip level3Sound;
    public AudioClip level6Sound;
    public AudioClip level9Sound;
    public AudioClip[] speeches;
    [Range(0, 1)]
    [SerializeField] float speechFrequency = 0.5f;

    [Header("Device Simulation")]
    public GameObject xrSimulator;

    private AudioSource audio;

    private int lives;
    private int score = 0;
    private int level = 1;
    private int levelPoints; // number of points since the last level

    public bool GameRunning { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        lives = startingLives;

        if (lives > 0)
        {
            gameOverText.gameObject.SetActive(false);
            GameRunning = true;
        }

        audio = GetComponent<AudioSource>();

#if UNITY_EDITOR
        xrSimulator.SetActive(true);
#else
        xrSimulator.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // Update GUI text
        levelText.text = level.ToString();
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
    }
    
    public void AddPoints(int amount)
    {
        score += amount;
        levelPoints += amount;

        if(levelPoints >= pointsPerLevel)
            IncreaseLevel();
    }
    public void IncreaseLevel()
    {
        level++;
        levelPoints = score % pointsPerLevel;

        foreach (ObjectSpawner spawner in spawners)
        {
            spawner.IncreaseTravelSpeed(objectSpeedIncrease);
            spawner.ReduceTimeDelay(fireRateIncrease);
        }

        // play level up sound
        if (audio != null)
        {
            if (level == 3 && level3Sound != null)
                audio.PlayOneShot(level3Sound);
            else if (level == 6 && level6Sound != null)
                audio.PlayOneShot(level6Sound);
            else if (level == 9 && level9Sound != null)
                audio.PlayOneShot(level9Sound);
            else if (progressLevelSound != null)
                audio.PlayOneShot(progressLevelSound);
        }
    }

    public void LoseLife()
    {
        lives--;

        if (lives == 0)
        {
            gameOverText.gameObject.SetActive(true);

            if (audio != null && gameOverSound != null)
                audio.PlayOneShot(gameOverSound);

            GameRunning = false;
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void TogglePause()
    {
        GameRunning = !GameRunning;

        if (GameRunning)
        {
            menuGUI.SetActive(false);
            Time.timeScale = 1;

            SetProjectileVisibility(true);
        }
        else
        {
            menuGUI.SetActive(true);
            Time.timeScale = 0;

            SetProjectileVisibility(false);
        }
    }

    public void PlayRandomSpeech()
    {
        if(speeches.Length > 0)
        {
            float chance = Random.value;
            Debug.Log("Random value: " + chance);

            if (chance <= speechFrequency)
            {
                int randomSpeech = Random.Range(0, speeches.Length);
                audio.PlayOneShot(speeches[randomSpeech]);
            }
        }        

    }

    private void SetProjectileVisibility(bool visible)
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject projectile in projectiles)
        {
            projectile.GetComponent<Renderer>().enabled = visible;
        }
    }

}
