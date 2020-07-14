using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove : MonoBehaviour
{
    private GameObject blue;
    private Vector2 target;
    public float MissileSpeed = 10.0f;
    public bool fired = false;
    float step;
    // Start is called before the first frame update
    void Start()
    {
        blue = GameObject.Find("blue");
        target = new Vector2(blue.transform.position.x, blue.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) {
            if (fired) {
                target = new Vector2(blue.transform.position.x, blue.transform.position.y);
                fired = false;
            }
            step = MissileSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), target, step);
        }
        // check if hit the ground
        /*if (Physics2D.Linecast(transform.position + new Vector3(0.4f,-1.1f,0), transform.position + new Vector3(-0.4f,-1.2f,0))) {
            gameObject.SetActive(false);
        }*/

        // check if we reached our final destination
        if (target == new Vector2(transform.position.x, transform.position.y)) {
            gameObject.SetActive(false);
        }
    }
}
