using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FPSControllerLPFP;

public class GameCtr : MonoBehaviour
{
    public Slider mSSlider, gSSlider;
    private GameObject player;
    private GameObject[] Enemy;
    public bool settingFlag;
    public GameObject settingPanel;
    public Text enemyHealthText;
    private int settingEnemyHealth;
    public InputField input;
    private bool useFlag;
    public bool healthInfiniteFlag;
    public bool bulletsInfiniteFlag;
    public Toggle healthInfiniteToggle;
    public Toggle bulletsInfiniteToggle;
    private float timeNub;
    private bool timeFlag;
    public Text timeText;
    public bool deathFlag;
    public GameObject gameOverUI;
    public bool caissonFlag;
    public bool winFalg;
    public GameObject winUI;

    private void Awake()
    {
        deathFlag = false;
        winFalg = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        int.TryParse(enemyHealthText.text, out settingEnemyHealth);
        timeNub = 1;
        timeText.text = timeNub.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            settingFlag = !settingFlag;
        if (settingFlag&&!deathFlag)
        {
            settingPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (timeFlag)
            {
                timeNub = Time.timeScale;
                timeFlag = false;
            }
            Time.timeScale = 0;
            timeText.text = gSSlider.value.ToString();
        }
        else if(!deathFlag)
        {
            settingPanel.SetActive(false);
            useFlag = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = timeNub;
            timeFlag = true;
        }
        if (useFlag)
        {
            player.GetComponent<FpsControllerLPFP>().mouseSensitivity = mSSlider.value;
            timeNub = gSSlider.value;
           
            for (int i = 0; i < Enemy.Length; i++)
            {
                if(Enemy[i]!=null)
                    Enemy[i].GetComponent<Enemy>().health = settingEnemyHealth;
            }
            enemyHealthText.text = settingEnemyHealth.ToString("f0");
            healthInfiniteFlag = healthInfiniteToggle.isOn;
            bulletsInfiniteFlag = bulletsInfiniteToggle.isOn;
        }
        if (deathFlag)
        {
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (winFalg)
        {
            Time.timeScale = 0;
            winUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Return()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Use()
    {
        useFlag = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EnemyHealthTextInput()
    {
        int.TryParse(input.text,out settingEnemyHealth);
    }
}
