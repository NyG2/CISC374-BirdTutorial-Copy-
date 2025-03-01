using UnityEngine;



public class BirdController : MonoBehaviour
{
    public GameObject player;
    public float jumpForce = 10;
    public LogicScript logic;
    public bool birdIsAlive = true;

    public AudioSource audioSource;

    public AudioClip collisionSound;


    public AudioClip jumpSound;

    public AudioClip offScreenSound;

    private float maxJumpForce = 16f; // Prevents the bird from jumping too high

    private bool hasPlayedOffScreenSound = false;
    public float jumpIncrease = 0.5f; // How much jump force increases per X points

    private bool hasCollided = false;

    private bool gameStarted = false; // Prevents movement until game starts

    public int topHeight = Screen.height/2; //Top Screen
    public int bottomHeight = -Screen.height/2; //Bottom Screen
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        logic.scoreFlag = true;
        hasCollided = false;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return; // Prevent movement before the game starts
        if(Input.GetKeyDown(KeyCode.Space) == true && birdIsAlive){
            Jump();
        }

        OffScreen();
    }

    void Jump(){
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * jumpForce;
        audioSource.PlayOneShot(jumpSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   

         if (!birdIsAlive) return; // Prevents collision sound if the bird is already dead (e.g., went off-screen)
        
        birdIsAlive = false;
        
     
    if (!hasCollided && collision.transform.root.CompareTag("Pipe"))
    {
        hasCollided = true;
        audioSource.PlayOneShot(collisionSound);
        logic.gameOver();
    }
        
    }

    void OffScreen(){ //Function for if the bird goes offScreen 
       if (!birdIsAlive) return; // Prevents offScreenSound from playing after game over
       if(!hasPlayedOffScreenSound && (player.transform.position.y > topHeight || player.transform.position.y < bottomHeight)){
            hasPlayedOffScreenSound = true;
            birdIsAlive = false; // Set to false to prevent repeated calls
            audioSource.PlayOneShot(offScreenSound);
            logic.gameOver();
       }

    }

        // Call this from LogicScript when the Play button is clicked
    public void StartGame()
    {
        gameStarted = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

       // **Call this from LogicScript when the player reaches a certain score**
    public void IncreaseDifficulty()
    {
        jumpForce += jumpIncrease; // Slightly increase jump force
        if (jumpForce > maxJumpForce)
        {
            jumpForce = maxJumpForce; // Prevents excessive jumps
        }
    }
}
