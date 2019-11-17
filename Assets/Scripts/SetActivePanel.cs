using UnityEngine;


public class SetActivePanel : MonoBehaviour
{
  public void tumbler( GameObject panel )
  {
    panel.SetActive( !panel.activeSelf );
  }

  public void setActiveFalse( GameObject panel )
  {
    panel.SetActive( false );
  }

  public void setActiveTrue( GameObject panel )
  {
    panel.SetActive( true );
  }
}