
using UnityEngine;

public class FanPush : MonoBehaviour
{

    [SerializeField] private int speed = 2;

    private void OnTriggerStay2D(Collider2D collision)
    {


        if (collision.gameObject.name == "Player")
        {

            collision.attachedRigidbody.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }


    }
}
