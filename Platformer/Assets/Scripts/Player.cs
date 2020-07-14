using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Animator animator;
    int directionX = 3;
    int directionY = 3;
    private float update;
    float dirX, moveSpeed, dirY, jumpSpeed, jmp, previousheight, currentheight;
    private Rigidbody2D r2d2;
    public float MySpeed = 0;
    float Invincibility = 0.0f;
    public GameObject pausemenu;
    public AudioClip AudioDeath;
    Vector2 start1;
    Vector2 end1;
    Vector2 start2;
    Vector2 end2;
    private bool FallingToDeath = false;
    float step;

    // Start is called before the first frame update
    void Start()
    {
        r2d2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveSpeed = 5.5f;
        jumpSpeed = 12.4f;
        update = 0.0f;

        PlayerStats.respawn = new Vector3(-5.17f, 2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        update += Time.deltaTime;
        Invincibility += Time.deltaTime;

        if (PlayerStats.lives == 0)
        {
            SceneManager.LoadScene("Level 1");
            PlayerStats.lives = 3;
            Debug.Log("Player ran out of lives");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausemenu.activeSelf)
            {
                pausemenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pausemenu.SetActive(true);
                Time.timeScale = 0;
            }
            //Debug.Log("esc key pressed");
        }

        if (transform.position.y < -5)
        { // fell in a pit
            transform.position = new Vector3(transform.position.x, -7, 0);
            r2d2.velocity = new Vector3(0, 0, 0);
            if (!FallingToDeath)
            {
                PlayerHit();
                FallingToDeath = true;
            }
            return;
        }

        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        dirY = Input.GetAxisRaw("Vertical") * jumpSpeed * Time.deltaTime;

        // point me in the right direction
        if (gameObject.activeInHierarchy && update < 0.1f)
        {
            dirX = 0.01f;
        }

        start1 = transform.position + new Vector3(-0.35f, -0.6f, 0);
        end1 = transform.position + new Vector3(-0.35f, 0.8f, 0);
        start2 = transform.position + new Vector3(0.38f, -0.6f, 0);
        end2 = transform.position + new Vector3(0.38f, 0.8f, 0);

        if (dirY > 0 && animator.GetBool("isGrounded"))
            r2d2.velocity = new Vector2(r2d2.velocity.x, jumpSpeed);

        transform.position = new Vector2(transform.position.x + dirX, transform.position.y);

        if (dirX != 0f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if ((Input.GetAxisRaw("Vertical") > 0f) && !animator.GetBool("isGrounded"))
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if (dirX > 0)
            directionX = -3;
        else if (dirX < 0)
            directionX = 3;

        Vector3 ls = transform.localScale;
        ls.x = directionX;
        ls.y = directionY;
        transform.localScale = ls;

        // check if we're falling
        if (currentheight < previousheight)
        {
            animator.SetBool("isFalling", true);
        }
        else if (currentheight > previousheight)
        {
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
        previousheight = currentheight;

        Debug.DrawLine(transform.position + new Vector3(0.05f, -1.1f, 0), transform.position + new Vector3(-0.05f, -1.1f, 0), Color.white);

        // check if we're on the ground
        if (Physics2D.Linecast(transform.position + new Vector3(0.05f, -1.1f, 0), transform.position + new Vector3(-0.05f, -1.2f, 0)) && dirY <= 0)
        {
            //Debug.Log("on ground");
            animator.SetBool("isGrounded", true);
        }
        else
            animator.SetBool("isGrounded", false);

        MySpeed = r2d2.velocity.magnitude;
    }

    void LateUpdate()
    {
        if (transform.position.y < -5)
            return;

        currentheight = transform.position.y;
    }

    public void PlayerHit() {
        Debug.Log("Player has been hit!");
        AudioSource.PlayClipAtPoint(AudioDeath, transform.position);
        animator.SetBool("isHit",true);
        //Invincibility = 0;
        StartCoroutine(ReloadLVL());
    }

    IEnumerator ReloadLVL() {
        yield return new WaitForSeconds(3);
        PlayerStats.lives--;
        if (PlayerStats.respawn == new Vector3(0,0,0)) {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
            Debug.Log("respawn coords nil, restarting level");
        } else {
            animator.SetBool("isHit",false);
            transform.position = PlayerStats.respawn;
            Debug.Log("respawning at coordinates " + PlayerStats.respawn);
        }
        FallingToDeath = false;
    }

    void OnCollisionEnter2D(Collision2D col) {
        // got hit by an enemy
        if (col.gameObject.name.Contains("Mushroom")) {
            if (!FallingToDeath && col.gameObject.GetComponent<Enemy>().health > 0 && (Physics2D.Linecast(start1, end1, LayerMask.GetMask("Enemy")) || 
            Physics2D.Linecast(start2, end2, LayerMask.GetMask("Enemy")))) {
                FallingToDeath = true;
                PlayerHit();
            }
        }
    }
}
