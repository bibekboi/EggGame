using UnityEngine;

public class BasketSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _basketPrefab;

    [SerializeField]
    private BasketScript _basketScript;

    //How high the next basket should spawn at
    private float verticalStep = 4f;
    private float nextY = 0f;

    private void Start()
    {
        //Spawn first basket at fixed start position
        GameObject firstBasket = Instantiate(_basketPrefab, new Vector3(0f, -4f, 0f), Quaternion.identity);
        _basketScript.can_move = false;

        //Set new nextY for the following basket
        nextY = -4f + verticalStep;

        //Spawn the next basket above the first one
        SpawnNextBasket();
    }

    public void SpawnNextBasket()
    {
        //Set random x value
        float x = Random.Range(-2.5f, 2.5f); 

        Vector3 spawnPosition = new Vector3 (x, nextY, 0);

        Instantiate(_basketPrefab, spawnPosition, Quaternion.identity);

        nextY += verticalStep;
    }
}
