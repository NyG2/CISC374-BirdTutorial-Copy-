using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject pipe;
    public float spawnRate = 20;
    public float timer = 0;

    public float heightOffset = 5;

    private bool gameStarted = false;

    private float minSpawnRate = 0.8f; // Prevents spawn rate from becoming too fast

    
    void Start()
    {
        gameStarted = false;
        //spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return;

        if(timer < spawnRate){
            timer += Time.deltaTime;
        }else{
            spawnPipe();
            timer = 0;
        }

    }
      void spawnPipe(){
           float lowestPoint = transform.position.y - heightOffset;
           float highestPoint = transform.position.y + heightOffset;
           Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
       }

      public void StartGame()
    {
        gameStarted = true; 
        timer = 0.2f; 
    }

       // **Call this method from LogicScript when the player reaches a certain score**
    public void IncreaseDifficulty()
    {
        spawnRate -= 0.1f; // Pipes spawn faster over time
        if (spawnRate < minSpawnRate)
        {
            spawnRate = minSpawnRate; // Prevents the game from becoming impossible
        }
    }

}
