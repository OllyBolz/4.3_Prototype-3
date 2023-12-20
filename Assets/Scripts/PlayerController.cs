using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    private float defaultAnimSpeed;

    private float jumpForce = 40f;
    private float dJumpMultiplier = 0.8f;
    private float gravityModifier = 10f;

    public float moveLeftModifier;

    public bool isOnGround = true;
    public bool dJump = false;

    public bool gameOver = false;
    private GameObject gOText;
    private TextMeshProUGUI scoreText;

    public int scoreTotal = 0;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        Physics.gravity *= gravityModifier;

        defaultAnimSpeed = playerAnim.speed;

        gOText = GameObject.Find("/Canvas/Game Over");
        gOText.SetActive(false);

        scoreText = GameObject.Find("/Canvas/Score").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            dJump = true;
            playerAnim.SetTrigger("Jump_trig");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && dJump && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce * dJumpMultiplier, ForceMode.Impulse);
            dJump = false;
            playerAnim.SetTrigger("Jump_trig");
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveLeftModifier = 2f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeftModifier = 0.75f;
        }
        else
        {
            moveLeftModifier = 1f;
        }

        if (playerAnim.speed != defaultAnimSpeed * moveLeftModifier)
        {
            playerAnim.speed = defaultAnimSpeed * moveLeftModifier;
        }

        //Debug.Log(scoreTotal.ToString());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dJump = false;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            gameOver = true;
            gOText.SetActive(true);
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
    }

    public void AddScore()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            scoreTotal += 125;
        }
        else
        {
            scoreTotal += 100;
        }
        scoreText.text = scoreTotal.ToString();
    }
}
