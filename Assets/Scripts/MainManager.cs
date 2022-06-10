using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_MaxScore;
    
    private bool m_GameOver = false;

    private string maxScorePath;


    // Start is called before the first frame update
    void Start()
    {
        maxScorePath =  Application.persistentDataPath + "/maxScore.json";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadMaxScore();
        AddPoint(0); //Update userName on scoretext
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{ApplicationManager.userName} Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveMaxScore();
    }

    private class MaxScoreData
    {
        public string userName;
        public int score;
    }

    void SetMaxScore(MaxScoreData maxScore)
    {
        m_MaxScore = maxScore.score;
        BestScoreText.text = $"Best Score : {maxScore.userName} : {maxScore.score}";
    }

    public void SaveMaxScore()
    {
        if (m_Points > m_MaxScore)
        {
            m_MaxScore = m_Points;            
            MaxScoreData maxScore = new MaxScoreData();
            maxScore.score = m_MaxScore;
            maxScore.userName = ApplicationManager.userName;
            string jsonString = JsonUtility.ToJson(maxScore);
            File.WriteAllText(maxScorePath, jsonString);
        }
    }

    public void LoadMaxScore()
    {        
        if (File.Exists(maxScorePath))
        {
            string jsonFile = File.ReadAllText(maxScorePath);
            MaxScoreData maxScore = JsonUtility.FromJson<MaxScoreData>(jsonFile);
            SetMaxScore(maxScore);
        }

    }

    public void ResetMaxScore()
    {
        try
        {
            File.Delete(maxScorePath);            
        }catch { }
        SetMaxScore(new MaxScoreData());
    }
}
