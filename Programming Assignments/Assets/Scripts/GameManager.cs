using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public TextMeshProUGUI timerText;
    private  int redTilesCount = 0;
    private  int blackTilesCount = 0;
    private int effectiveTiles = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI youText;
    public float gameTime = 60f; // 5 minutes
    private float timer;
    private bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        timer = gameTime;
        youText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameWon) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        scoreText.text = (redTilesCount/2).ToString();

        if (timer <= 0)
        {
            GameOver(false);
        }
        else
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        redTilesCount = 0;
        blackTilesCount = 0;
        effectiveTiles = 0;
        int whiteTiles = 0;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector3 tilePosition = new Vector3(x, 0, y);
                Collider[] colliders = Physics.OverlapSphere(tilePosition, 0.1f);
                foreach (Collider collider in colliders)
                {
                    Renderer renderer = collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if(renderer.material.color == Color.white)
                        {
                            whiteTiles++;
                        }
                        else if(renderer.material.color == Color.red)
                        {
                            redTilesCount++;
                        }
                        else if(renderer.material.color == Color.black)
                        {
                            blackTilesCount++;
                        }
                    }
                }
            }
        }
        // If all tiles except one (where the enemy stands) are red, player wins
        if (redTilesCount == 198)
        {
            GameOver(true);
        }
    }

    void GameOver(bool won)
    {
        gameWon = true;
        if (won)
        {
            youText.text = "YOU WIN";
            youText.color = Color.green;
        }
        else
        {
            youText.text = "YOU LOSE";
            youText.color = Color.red;
        }

        // Restart the game after a short delay
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
       
        yield return new WaitForSeconds(3f);
       

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
