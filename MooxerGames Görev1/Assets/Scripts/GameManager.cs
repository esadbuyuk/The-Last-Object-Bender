using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI enemyScoreText;
    private int score;
    private int enemyScore;
    
    ObjectPooler objectPooler;
    private bool routinCompleted = false;
   
   
    void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;

        UpdateScore(0);
        UpdateEnemyScore(0);
        SpawnObjects(20);        
    }

    void Update()
    {
        if (routinCompleted)
        {
            SpawnWithTime(10);
        }
    }
    
    public void ResetGame()
    {
        Time.timeScale = 1;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "You: " + score;
    }

    public void UpdateEnemyScore(int scoreToAdd)
    {
        enemyScore += scoreToAdd;
        enemyScoreText.text = "Enemy: " + enemyScore;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    
    IEnumerator SpawnWithTime(float seconds)
    {
        routinCompleted = false;
        yield return new WaitForSeconds(seconds);
        SpawnObjects(10);
        routinCompleted = true;
    }

    private IEnumerator RemoveAfterSeconds(float seconds, GameObject obj)
    {
        if (obj.activeInHierarchy)
        {
            yield return new WaitForSeconds(seconds);
            obj.SetActive(false);
        }
    }

    private Vector3 RandomPosCalculator()
    {
        Vector3 randomPos = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));

        return randomPos;
    }

    public void SpawnObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            objectPooler.SpawnFromPool("Pink Bucket Pool").transform.position = RandomPosCalculator();
            objectPooler.SpawnFromPool("Green Basket Pool").transform.position = RandomPosCalculator();
        }
    }
}