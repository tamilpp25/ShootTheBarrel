using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int points = 0;
    public int barrelsBroken = 0;
    public int barrlsToBreak = 0;
    public GameState state = GameState.RUNNING;
    public static GameManager instance;
    private bool gameEnded = false;
    public Difficulty difficulty = Difficulty.HARD;
    private Vector3[] positions = new Vector3[20]
    {
        new Vector3(-8,0,-1),
        new Vector3(-11.2f,0,4.5f),
        new Vector3(-11.2f,0,-0.6f),
        new Vector3(-11,0,0.5f),
        new Vector3(-9.5f,0,27.1f),
        new Vector3(-3.9f,0,27.5f),
        new Vector3(9.2f,0,24.7f),
        new Vector3(13.6f,0,25.5f),
        new Vector3(15.7f,0,10),
        new Vector3(9.95f,0,7.6f),
        new Vector3(10,0.3f,14),
        new Vector3(-12.6f, -0.5f, 5.7f),
        new Vector3(4.5f,0.5f, 9.5f),
        new Vector3(-0.8f,6.2f,3),
        new Vector3(3.5f,6.2f,0.5f),
        new Vector3(9.1f, 7.79f, -3.17f),
        new Vector3(9,0,-17),
        new Vector3(4.5f, 0, 9.5f),
        new Vector3(-11,0,1),
        new Vector3(-3, 0, 20),
    };
    private DateTime timer;
    public GameObject redBarrelPrefab;
    public GameObject yellowBarrelPrefab;

    [SerializeField]
    private TMP_Text scoreDisplay;
    [SerializeField]
    private TMP_Text timeDisplay;
    [SerializeField]
    private TMP_Text progressDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //Development build
        Debug.developerConsoleVisible = true;

        instance = this;
        points = 0;
        Debug.Log("Starting game with difficulty: " + difficulty);
        switch (difficulty)
        {
            case Difficulty.EASY:
                barrlsToBreak = 5;
                break;
            case Difficulty.MEDIUM:
                barrlsToBreak = 10;
                break;
            case Difficulty.HARD:
                barrlsToBreak = 20;
                break;
        }
        timer = generateTimeLeft(difficulty);
        SpawnRandomBarrels(difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.RUNNING)
        {
            timeDisplay.text = String.Format("Time left: {0}s", (timer - DateTime.Now).Seconds);
            progressDisplay.text = String.Format("{0}/{1}", barrelsBroken, barrlsToBreak);

            Debug.Log("Time left: " + (timer - DateTime.Now).Seconds + "s");
            if (!gameEnded)
            {
                if ((timer - DateTime.Now).Seconds <= 0)
                {
                    gameEnded = true;
                    Debug.Log("Time over! Game ended! Points: " + points);
                    scoreDisplay.text = String.Format("Game Over!\nScore: {0}", points);
                    scoreDisplay.gameObject.SetActive(true);
                    state = GameState.ENDED;
                    GameObject.Find("Crosshair").SetActive(false);
                }
                else
                {
                    if (barrlsToBreak == barrelsBroken)
                    {
                        gameEnded = true;
                        points += CalculateTimeRemainingPoints();
                        scoreDisplay.text = String.Format("Game Over!\nScore: {0}", points);
                        scoreDisplay.gameObject.SetActive(true);
                        Debug.Log("You won! Points: " + points);
                        state = GameState.ENDED;
                        GameObject.Find("Crosshair").SetActive(false);
                    }
                }

                // In progress
            }
            else
            {
                //Game ended display score
                Debug.Log("Score: " + points);
            }
        }
    }

    public void CalculatePoints()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                points += 5;
                break;
            case Difficulty.MEDIUM:
                points += 10;
                break;
            case Difficulty.HARD:
                points += 20;
                break;
        }
    }

    int CalculateTimeRemainingPoints()
    {
        int time = (timer - DateTime.Now).Seconds;
        switch (difficulty)
        {
            case Difficulty.EASY:
                return time;
            case Difficulty.MEDIUM:
                return time * 2;
            case Difficulty.HARD:
                return time * 3;
            default:
                return time;
        }
    }

    void SpawnRandomBarrels(Difficulty diff)
    {
        int max = 0, counter = 0;
        List<Vector3> vec3 = new List<Vector3>();
        switch (diff)
        {
            case Difficulty.EASY:
                max = 5;
                break;
            case Difficulty.MEDIUM:
                max = 10;
                break;
            case Difficulty.HARD:
                max = 20;
                break;
        }

        while (counter < max)
        {
            Vector3 pos = positions[Random.Range(0, 20)];
            if (!vec3.Contains(pos))
            {
                Debug.Log(pos);
                vec3.Add(pos);
                SpawnRandomColorBarrel(pos);
                counter++;
            }
        }
    }

    private void SpawnRandomColorBarrel(Vector3 pos)
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                Instantiate(redBarrelPrefab, pos, Quaternion.identity);
                break;
            case 1:
                Instantiate(yellowBarrelPrefab, pos, Quaternion.identity);
                break;
        }
    }

    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public enum GameState
    {
        RUNNING,
        ENDED
    }

    private DateTime generateTimeLeft(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.EASY:
                return DateTime.Now.AddMinutes(2);
            case Difficulty.MEDIUM:
                return DateTime.Now.AddMinutes(1);
            case Difficulty.HARD:
                return DateTime.Now.AddSeconds(30);
            default:
                return DateTime.Now;
        }
    }
}
