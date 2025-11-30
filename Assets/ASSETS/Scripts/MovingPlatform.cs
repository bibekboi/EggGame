using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform Basket;
    public Transform StartPoint;
    public Transform EndPoint;

    private int movementDirection = 1;
    private float speed = 1.5f;

    private void Update()
    {
        Vector2 target = currentMovementTarget();

        Basket.position = Vector2.MoveTowards(Basket.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2)Basket.position).magnitude;

        if(distance <= 0.1f)
        {
            movementDirection *= -1;
        }
    }

    Vector2 currentMovementTarget()
    {
        if(movementDirection == 1)
        {
            return StartPoint.position;
        }
        else
        {
            return EndPoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        // For debugging a line
        if(Basket != null && StartPoint != null && EndPoint != null)
        {
            Gizmos.DrawLine(Basket.transform.position, StartPoint.position);
            Gizmos.DrawLine(Basket.transform.position, EndPoint.position);
        }
    }
}
