using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float cameraSmooth = 2f;

    private void LateUpdate()
    {
        if(player.position.y > transform.position.y)
        {
            Vector3 newPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, cameraSmooth * Time.deltaTime);
        }
    }
}
