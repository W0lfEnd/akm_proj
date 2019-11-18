using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
  [SerializeField] private int player_cur_place_id = -1;
  [SerializeField] private ControlPanelManager controlPanelManager = null;

  #region Singltone
  private static TestGameManager instance ;
  public static TestGameManager getInstance() => instance;
  #endregion

  private void Start()
  {
    #region Singltone
    if ( instance == null )
    {
      instance = this; 
    } else if ( instance == this )
    { 
      Destroy( gameObject ); 
    }
    #endregion

  }

  public void setPlayerPlace( int id_place )
  {
    player_cur_place_id = id_place;
    controlPanelManager.updateControlPanel( id_place );
  }
}
