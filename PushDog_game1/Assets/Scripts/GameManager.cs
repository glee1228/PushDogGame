using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    AudioSource audioSource_background;

    public AudioClip mainbackgroundsound;
    public AudioClip nextstagesound;
    public AudioClip gameoversound;

    public LevelBuilder m_LevelBuilder;

    private bool m_ReadyForInput;
    private Player m_Player;

    public float levelStartDelay = 2f;
    public Text mainlevelText;
    private GameObject levelImage;
    private bool doingSetup = true;

    public Text levelText;
    int Level;

    public float timeAmt = 100;
    float time;
    public Text timeText;

    public Text scoreText;
    public int score = 500;
    int totalscore = 0;

    public Text scoreminusText;
    public Text timeplusText;
    private GameObject timeplusimage;
    private GameObject scoreminusimage;


    public void Awake()
    {
        audioSource_background = GetComponent<AudioSource>();
    }

    void Start()
    {
        levelImage = GameObject.Find("LevelImage");
        mainlevelText = GameObject.Find("MainLevelText").GetComponent<Text>();
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        GetComponent<AudioSource>().clip = mainbackgroundsound;
        audioSource_background.Play();

        timeplusimage = GameObject.Find("TimePlusImage");
        scoreminusimage = GameObject.Find("ScoreMinusImage");

        scoreminusimage.SetActive(false);
        timeplusimage.SetActive(false);

        time = timeAmt;
        TimeCount();

        ResetScene();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveInput.Normalize();

        if (Input.GetKeyDown("f"))
        {
            ResetScene();

            time += 5;
            score -= 100;

            scoreminusimage.SetActive(true);
            timeplusimage.SetActive(true);

            scoreminusText.text = "Score\n-100";
            timeplusText.text = "Time\n+5s";
            Invoke("CallScoreMinusText", 1f);
        }

        if (score <= 0)
        {
            score = 0;
            GameOver();
        }

        if (moveInput.sqrMagnitude > 0.5)
        {
            if (m_ReadyForInput)
            {
                m_ReadyForInput = false;
                m_Player.Move(moveInput);

                if (IsLevelComplete())
                {
                    if (score != 1) score += 1000;
                    else score += 0;

                    if (Level == 5)
                    {
                        if (score > 1)
                        {
                            totalscore = score;
                            score = 1;
                        }

                        ShowClearScore();

                        Invoke("DelayEndingScene", 3f);
                    }
                    else
                    {
                        ShowClearLevel();

                        NextLevel();
                    }
                }
            }
        }
        else
        {
            m_ReadyForInput = true;
        }

        scoreText.text = "Score " + score;
        TimeCount();
    }

    public void changeToScene(int changeTheScene)
    {
        SceneManager.LoadScene(changeTheScene);
    }

    public void DelayEndingScene()
    {
        changeToScene(4);
    }

    public void ShowClearLevel()
    {
        doingSetup = true;

        Level = m_LevelBuilder.m_CurrentLevel + 2;
        mainlevelText.text = "Stage " + Level + "\n\nScore +1000";

        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
    }

    public void ShowClearScore()
    {
        doingSetup = true;

        mainlevelText.text = "All Clear\n\nTotal Score " + totalscore;

        levelImage.SetActive(true);
    }

    public void NextLevel()
    {
        m_LevelBuilder.NextLevel();
        m_LevelBuilder.Build();

        GetComponent<AudioSource>().clip = mainbackgroundsound;
        audioSource_background.Play();

        StartCoroutine(ResetSceneASync());

        Level = m_LevelBuilder.m_CurrentLevel + 1;
        levelText.text = "Stage" + Level;

        time = timeAmt;
    }

    public void TimeCount()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = "Time " + time.ToString("F");
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        //GetComponent<AudioSource>().clip = gameoversound;
        //audioSource_background.Play();
        GetComponent<AudioSource>().mute = true;

        if (Input.GetKeyDown("space"))
        {
            changeToScene(0);
        }

        mainlevelText.text = "Game Over\n\n" + "Score: " + score;
        levelImage.SetActive(true);   
    }

    public void CallScoreMinusText()
    {
        scoreminusimage.SetActive(false);
        timeplusimage.SetActive(false);
        scoreminusText.text = "";
        timeplusText.text = "";
    }

    public void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneASync());
    }

    bool IsLevelComplete()
    {
        Bone[] bones = FindObjectsOfType<Bone>();
        foreach (var bone in bones)
        {
            if (!bone.m_OnHome) return false;
        }
        return true;
    }

    IEnumerator ResetSceneASync()
    {
        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncUnload.isDone)
            {
                yield return null;
                Debug.Log("Unloading...");
            }
            Debug.Log("Unload Done");
            Resources.UnloadUnusedAssets();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading...");
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));

        m_LevelBuilder.Build();
        m_Player = FindObjectOfType<Player>();
        Debug.Log("Level loaded");
    }
}
