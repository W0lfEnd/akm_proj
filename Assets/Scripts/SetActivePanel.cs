using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActivePanel : MonoBehaviour
{
  public void tumbler( GameObject panel )
  {
    panel.SetActive( !panel.activeSelf );
  }
}
