using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

  protected Animator anim;


  protected virtual void Start()
  {
    anim = GetComponent<Animator>();
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
