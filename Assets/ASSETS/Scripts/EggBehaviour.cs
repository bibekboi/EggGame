using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EggBehaviour : MonoBehaviour
{
    public bool IS_ALIVE = true;

    [SerializeField]
    private Rigidbody2D _eggRigidbody;

    private float eggJumpForce = 10f;

    private bool is_sitting_on_basket = false;

    private Vector2 basketVelocity;

    //Cooldown Variables
    private bool jumpCooldown = false;
    private float jumpCooldownTime = 0.15f; //150 ms

    private void Start()
    {
        Time.timeScale = 1.0f;
        IS_ALIVE = true;

        _eggRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.started) return;

        if (jumpCooldown) return;

        if (is_sitting_on_basket)
        {
            StartCoroutine(DetachAndJump());
            return;
        }

        //Don't jump if egg is not on the basket
        return;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.collider.CompareTag("Basket"))
        {
            //Start safe snap after physics
            StartCoroutine(SnapToBasketAfterPhysics(collision));
        }
    }

    private IEnumerator SnapToBasketAfterPhysics(Collision2D collision)
    {
        //Wait for physics to finish this frame
        yield return new WaitForFixedUpdate();

        //Stick to basket
        transform.SetParent(collision.transform);

        //Turning off physics
        _eggRigidbody.bodyType = RigidbodyType2D.Kinematic;

        //Make sure linear velocity is zero
        _eggRigidbody.linearVelocity = Vector2.zero;

        //Snap Egg to Basket
        Vector3 snapToBasketPos = collision.collider.transform.position;

        //Snap to X, but keep Y and Z as it is
        transform.position = new Vector3(snapToBasketPos.x, transform.position.y, transform.position.z);

        //Capture velocity if basket has Rigidbody2D
        Rigidbody2D basketRigidbody = collision.collider.attachedRigidbody;
        if (basketRigidbody != null)
        {
            basketVelocity = basketRigidbody.linearVelocity;
        }
        else
        {
            //Estimate manual velocity
            basketVelocity = (collision.collider.transform.position - transform.position) / Time.deltaTime;
        }

        is_sitting_on_basket = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Update basket velocity every frame
        if (is_sitting_on_basket && collision.collider.CompareTag("Basket"))
        {
            Rigidbody2D basketRigidbody = collision.collider.attachedRigidbody;
            if (basketRigidbody != null)
            {
                basketVelocity = basketRigidbody.linearVelocity;
            }
        }
    }

    private IEnumerator DetachAndJump()
    {
        jumpCooldown = true; // Start cooldown

        //Detach immediately
        transform.SetParent(null, true); // Keeps world values stable

        //Move egg slightly up to avoid collider overlapping
        transform.position += new Vector3(0, 0.1f, 0);

        //Reenable physics
        _eggRigidbody.bodyType = RigidbodyType2D.Dynamic;
        _eggRigidbody.gravityScale = 1f; // Manually set gravity

        //Wait one physics frame
        yield return new WaitForFixedUpdate();

        _eggRigidbody.linearVelocity = new Vector2(basketVelocity.x, eggJumpForce);

        is_sitting_on_basket = false;

        //Wait for cooldown timer
        yield return new WaitForSeconds(jumpCooldownTime);

        jumpCooldown = false;
    }
}
