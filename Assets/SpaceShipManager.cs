using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipManager : MonoBehaviour
{
  public Animator ship_anim;

  public void Start()
  {
  }

  public void destroyShip()
  {
    ship_anim.SetTrigger( "Explosion" );
  }
  
  public void miss()
  {
    ship_anim.SetTrigger( "Miss" );
  }
  
  public void destroyByMeteor()
  {
    ship_anim.SetTrigger( "ExploseByMeteor" );
  }
  
  public void meteorDamage()
  {
    ship_anim.SetTrigger( "MeteorDamage" );
  }
}
