
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public float gameSpeed = 5f;
    public float maxSpeed = 12f;
    public float speedIncrease = 0.1f;

    public bool isGameOver = false;

    private float score = 0f;
    private TimmyControls timmy;
    public Sprite crashedTimmy;
    public SpriteRenderer timmySprite;

    void Awake()
    {
        // Other scripts use instance to access the game manager
        instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        timmy = FindAnyObjectByType<TimmyControls>();
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Score increases as time goes on
            score += Time.deltaTime * 10f;

            // Increase the challenge gradually, up the maxSpeed
            gameSpeed += speedIncrease * Time.deltaTime;
            gameSpeed = Mathf.Min(gameSpeed, maxSpeed);
        }

        // Restart Control: when game is over and 'r' key is clicked, restart game
        if (isGameOver &&
            Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    //Timmy's Speed: jumping and dashing
    public float GetWorldSpeed()
    {
        float speed = gameSpeed;

        // When dashing the ground and obstacles scroll twice as fast to create a fast movement effect
        if (timmy != null && timmy.IsDashing())
        {
            speed *= 2f;
        }

        return speed;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        // Mark the game as over before freezing movement
        isGameOver = true;

        // Change to Crashed Timmy prefab
        timmySprite.sprite = crashedTimmy;

        //Stop music and play game over music
        if (AudioControls.instance != null)
        {
            AudioControls.instance.PlayGameOver();
        }

        // Teminate all movement
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Reload the starting scene to reset the game and the score
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnGUI()
    {
        // Score text
        GUIStyle scoreStyle = new GUIStyle(GUI.skin.label);
        scoreStyle.fontSize = 45;
        scoreStyle.normal.textColor = Color.white;

        GUI.Label(
            new Rect(20, 20, 300, 60),
            "Score: " + Mathf.FloorToInt(score),
            scoreStyle
        );

        if (isGameOver)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            // Game Over text
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 80;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = Color.red;

            GUI.Label(
                new Rect(0, Screen.height / 2 - 130, Screen.width, 130),
                "GAME OVER!",
                titleStyle
            );

            // Restart text
            GUIStyle restartStyle = new GUIStyle(GUI.skin.label);
            restartStyle.fontSize = 45;
            restartStyle.alignment = TextAnchor.MiddleCenter;
            restartStyle.normal.textColor = Color.white;

            GUI.Label(
                new Rect(0, Screen.height / 2, Screen.width, 100),
                "Press R to Restart",
                restartStyle
            );
        }
    }
}

//Used AI to change Timmy's prefab from the orignal Timmy to Crashed Timmy without creating a whole new sript.
//It saved me time and helped me keep things more consice.
//Used AI to help with the sizing the text/colors and the starting variable parameters. Then, adjusted to my liking.
//Used AI to figure out how to restart the game to its orignal starting screen.

