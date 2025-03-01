using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; 

public class LogicScript : MonoBehaviour
{
   public int playerScore;
   public Text scoreText;
   public GameObject gameOverScreen;

   public GameObject gameElements; 

   public GameObject startScreen;

   public AudioSource audioSource;
   public AudioClip scoreSound; 
   public bool scoreFlag; //Whenever the bird scores, stops once game is over

   public Text highScoreText;

   public int highScore;

   public BirdController bird; 

   public GameObject pipeSObject;

   public PipeSpawnScript pSpawn; 

   public ParticleSystem scoreParticles;

     void Start()
    { 
        startScreen.SetActive(true);  // Show the start screen first
        gameElements.SetActive(false);  // Hide game elements until Play is clicked
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
        gameOverScreen.SetActive(false); // Hide Game Over UI at start
    }

[ContextMenu("Increase Score")]
   public void addScore(int scoreToAdd){
     if(scoreFlag){
        playerScore += scoreToAdd;
        audioSource.PlayOneShot(scoreSound);
        scoreParticles.transform.position = bird.transform.position; // Spawn at bird
        scoreParticles.Play();
        if (playerScore % 2 == 0) // Every 2 points the difficulty increases
       {
            pSpawn.IncreaseDifficulty();
            bird.IncreaseDifficulty();
        }
   
   }
    scoreText.text = playerScore.ToString();
   }
   
   public void restartGame(){
        gameElements.SetActive(true);
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   public void gameOver(){
      StartCoroutine(HandleGameOver()); 
      scoreFlag = false;  
   }

   private IEnumerator HandleGameOver()
{
    yield return new WaitForSeconds(0.2f); // Wait for sound to finish
    gameElements.SetActive(false);  // Hide game elements
    gameOverScreen.SetActive(true); // Show Game Over UI
    CheckHighScore();
}



       private void CheckHighScore()
    {
        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        UpdateHighScoreUI();
    }

        private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    public void startGame(){
        startScreen.SetActive(false);
        gameElements.SetActive(true);  // Show the game elements
         bird.StartGame();  // Allow the bird to move
         pipeSObject.SetActive(true);
         pipeSObject.GetComponent<PipeSpawnScript>().StartGame(); 
    }
}
