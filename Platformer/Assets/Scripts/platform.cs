using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    //private float update;
    private Rigidbody2D r2d2;
    private Vector2 velocity;
    private float dirY;
    public bool CanFall = true;
    private bool falling = false;
    // Start is called before the first frame update
    void Start()
    {
        //update = 0.0f;
        r2d2 = GetComponent<Rigidbody2D>();
        r2d2.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanFall)
            return;

        if (transform.position.y < 0) { // dead
            Destroy(gameObject, 0.4f);
            return;
        }
        //transform.rotation = Quaternion.Euler (0,0,0);
        if (falling) {
            //update += Time.deltaTime;
            //if (update > 0.1f)
            //{
                r2d2.constraints = RigidbodyConstraints2D.None;
            //}
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (CanFall)
            falling = true;
    }
}
