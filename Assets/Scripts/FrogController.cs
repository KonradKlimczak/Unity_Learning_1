using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy_Controller
{
  [SerializeField] private float leftCap;
  [SerializeField] private float rightCap;

  [SerializeField] private float jumpLength = 5f;
  [SerializeField] private float jumpHeight = 5f;
  [SerializeField] private LayerMask ground;

  private Collider2D coll;
  private Rigidbody2D rb;


  private bool facingLeft = true;

  protected override void Start()
  {
    base.Start();
    coll = GetComponent<Collider2D>();
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    if (anim.GetBool("jumping"))
    {
      if (rb.velocity.y < .1)
      {
        anim.SetBool("falling", true);
        anim.SetBool("jumping", false);
      }
    }
    if (anim.GetBool("falling") && coll.IsTouchingLayers(ground))
    {
      anim.SetBool("falling", false);
    }
  }

  private void Move()
  {
    if (facingLeft)
    {
      if (transform.position.x > leftCap)
      {
        if (transform.localScale.x != 1)
        {
          transform.localScale = new Vector3(1, 1, 1);
        }
        if (coll.IsTouchingLayers())
        {
          rb.velocity = new Vector2(-jumpLength, jumpHeight);
          anim.SetBool("jumping", true);
        }
      }
      else
      {
        facingLeft = false;
      }
    }
    else
    {
      if (transform.position.x < rightCap)
      {
        if (transform.localScale.x != -1)
        {
          transform.localScale = new Vector3(-1, 1, 1);
        }
        if (coll.IsTouchingLayers())
        {
          rb.velocity = new Vector2(jumpLength, jumpHeight);
          anim.SetBool("jumping", true);
        }
      }
      else
      {
        facingLeft = true;
      }
    }
  }

  public void JumpedOn()
  {
    anim.SetTrigger("death");
  }

  private void Death()
  {
    Destroy(this.gameObject);
  }
}
