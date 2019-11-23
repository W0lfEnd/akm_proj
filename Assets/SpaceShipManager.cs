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

  public int old_hp = -1000;
  private void Update()
  {
    if ( Client.client == null || Client.client.Model == null )
      return;
    
    if ( Client.client.Model.health.Value <= 0 )
      destroyByMeteor();
    else
      if ( old_hp != -1000 && old_hp > Client.client.Model.health.Value )
        meteorDamage();

    old_hp = Client.client.Model.health.Value;
  }
}
