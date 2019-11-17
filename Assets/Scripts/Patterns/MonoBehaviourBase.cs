using System;
using UnityEngine;


public abstract class MonoBehaviourBase : MonoBehaviour
{
  public bool was_inited_components { get; private set; } = false;


  protected void initComponents()
  {
    if ( was_inited_components )
      return;

    init();

    was_inited_components = true;
  }

  private void init()
  {
    initMyComponents();
    afterInitMyComponents();
  }

  protected virtual void initMyComponents() { }

  protected virtual void afterInitMyComponents() { }
}