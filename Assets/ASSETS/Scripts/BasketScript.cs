using UnityEngine;

public class BasketScript : MonoBehaviour
{
    public bool can_move = true;
    private bool moving_right = true;

    private bool is_triggered = false;

    [Header("Movement Elements")]
    private float speed; //Set by spawner
    private float leftLimit;
    private float rightLimit;


    private void Start()
    {
        //Getting screen edged
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;

        //Get half of basket
        float basketHalfLength = GetComponentInChildren<SpriteRenderer>().bounds.extents.x;

        leftLimit = screenLeft + basketHalfLength;
        rightLimit = screenRight - basketHalfLength;

        float startX = Random.Range(leftLimit, rightLimit);
        transform.position = new Vector3(startX, transform.position.y, 0f);

        //Set speed if it wasn't set by spawner
        if(speed <= 0f)
        {
            speed = Random.Range(1.5f, 4f);
        }
    }

    private void Update()
    {
        if (!can_move) return; 

        //Destroy basket if below camera
        if(Camera.main.transform.position.y - transform.position.y > 8f)
        {
            Destroy(gameObject);
            return;
        }

        float step = speed * Time.deltaTime;

        if(moving_right)
        {
            transform.position += Vector3.right * step;
        }
        else
        {
            transform.position += Vector3.left * step;
        }

        //Reverse direction at screen edge
        if(transform.position.x >= rightLimit)
        {
            moving_right = false;
        }
        else if (transform.position.x <= leftLimit)
        {
            moving_right = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (is_triggered) return;

        if(collision.CompareTag("Player"))
        {
            is_triggered = true;
            Debug.Log("PLAYER IS COLLIDING");
            FindAnyObjectByType<BasketSpawner>().SpawnNextBasket();
        }
    }
}
