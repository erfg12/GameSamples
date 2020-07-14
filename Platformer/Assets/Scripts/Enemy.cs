using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    public int health = 1;
    public bool moveRight = false;
    float moveSpeed = 1f;
    public AudioClip TheAudioClip;

    Vector2 startHead;
    Vector2 endHead;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking",true);
        animator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ls = transform.localScale;
        int directionX = 3;
        float dirX;

        startHead = transform.position + new Vector3(-0.2f,0.3f,0);
        endHead = transform.position + new Vector3(0.2f,0.3f,0);

        // check if we're hitting a wall, turn around
        if (Physics2D.Linecast(transform.position + new Vector3(-0.5f,0,0), transform.position + new Vector3(-0.6f,0,0),LayerMask.GetMask("Ground") + LayerMask.GetMask("Enemy"))) {
            moveRight = true;
        } else if (Physics2D.Linecast(transform.position + new Vector3(0.5f,0,0), transform.position + new Vector3(0.6f,0,0),LayerMask.GetMask("Ground") + LayerMask.GetMask("Enemy"))) {
            moveRight = false;
        }

        if (Physics2D.Linecast(startHead, endHead)) {
            health = 0;
            dirX = 0;
        }

        // keep walking, do not stop
        if (moveRight) {
            directionX = -3;
            dirX = 3 * moveSpeed * Time.deltaTime;
        } else {
            directionX = 3;
            dirX = -3 * moveSpeed * Time.deltaTime;
        }

        ls.x = directionX;
        if (health <= 0)
            ls.y = 2;
        else
            ls.y = transform.localScale.y;

        transform.localScale = ls;
        
        transform.position = new Vector2(transform.position.x + dirX, transform.position.y);
    }

    void FixedUpdate() {
        //Debug.DrawLine(startHead, endHead, Color.red);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (health <= 0) {
            AudioSource.PlayClipAtPoint(TheAudioClip, transform.position);
            animator.SetBool("isWalking",false);
            animator.SetBool("isDead", true);
            moveSpeed = 0;
            //Destroy(GetComponent<Rigidbody>());
            Destroy(gameObject, 0.4f);
        }
    }
}
