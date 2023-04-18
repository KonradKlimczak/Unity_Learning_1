using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
  private Rigidbody2D rb;
  private Animator animator;

  private enum State { Idle, Running, Jumping, Falling, Hurt }
  private State state = State.Idle;

  private Collider2D col;
  [SerializeField] private LayerMask ground;
  [SerializeField] private float speed = 5f;
  [SerializeField] private float jump = 10f;

  [SerializeField] private int cherries = 0;
  [SerializeField] private Text cherryText;
  [SerializeField] private Text stateText;
  [SerializeField] private float hurtForce = 10f;

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    col = GetComponent<Collider2D>();

    cherryText.text = cherries.ToString();
  }

  void Update()
  {
    if (state != State.Hurt)
    {
      Movement();
    }

    VelocityState();
    animator.SetInteger("state", (int)state);

    stateText.text = state.ToString();
  }

  private void Movement()
  {
    float horizontal = Input.GetAxis("Horizontal");

    if (horizontal < 0)
    {
      rb.velocity = new Vector2(-speed, rb.velocity.y);
      transform.localScale = new Vector3(-1, 1, 1);
    }
    else if (horizontal > 0)
    {
      rb.velocity = new Vector2(speed, rb.velocity.y);
      transform.localScale = new Vector3(1, 1, 1);
    }

    if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
    {
      Jump();
    }
  }

  private void Jump()
  {
    rb.velocity = new Vector2(rb.velocity.x, jump);
    state = State.Jumping;
  }

  private void VelocityState()
  {
    if (state == State.Jumping)
    {
      if (rb.velocity.y < 0)
      {
        state = State.Falling;
      }
    }
    else if (state == State.Falling)
    {
      if (col.IsTouchingLayers(ground))
      {
        state = State.Idle;
      }
    }
    else if (state == State.Hurt)
    {
      if (Mathf.Abs(rb.velocity.x) < .1f)
      {
        state = State.Idle;
      }
    }
    else if (Mathf.Abs(rb.velocity.x) > 2f)
    {
      state = State.Running;
    }
    else
    {
      state = State.Idle;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Collectable")
    {
      Destroy(collision.gameObject);
      cherries += 1;

      cherryText.text = cherries.ToString();
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Enemy")
    {
      Enemy_Controller enemy = other.gameObject.GetComponent<Enemy_Controller>();
      if (state == State.Falling)
      {
        enemy.JumpedOn();
        Jump();
      }
      else
      {
        if (other.gameObject.transform.position.x > transform.position.x)
        {
          rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
        }
        else
        {
          rb.velocity = new Vector2(hurtForce, rb.velocity.y);
        }
        state = State.Hurt;
      }
    }
  }
}
