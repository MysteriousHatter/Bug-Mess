using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    private Player _player;


    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Red"))
        {
            collision.gameObject.GetComponent<RedBacteria>().StartCorutineForFrozen();
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag.Equals("Blue"))
        {
            Debug.Log("1UP");
            _player.ActivateOneUp(collision);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag.Equals("Purple"))
        {
            Debug.Log("Invinciablity");
            _player.ActivateInvinciability(collision);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag.Equals("Yellow"))
        {
            _player.ActivateSlowMotion(collision);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag.Equals("Green"))
        {
            collision.gameObject.GetComponent<GreenBacteria>().GetTimeValue();
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
