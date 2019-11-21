using System;
using Model;
using UnityEngine;
using UnityEngine.UI;


public class SpaceShipPlacesManager : MonoBehaviour
{
  [SerializeField] private PanelInputs[] btn_places;
  [SerializeField] private Transform[] spawn_point;

  private long my_id => Client.client.Id;
  
  private void Start()
  {
    setAllPlacesGreen();
  }

  public void deinitAllPanels()
  {
    foreach ( PanelInputs panel in btn_places )
      panel.deinit();
  }
  

  public void changePlace( int idx_of_place )
  {
    Debug.Log( $"Ви сіли на {idx_of_place} місце" );
    
    deinitAllPanels();
    
    Panel cur_panel = Client.client.Model.panels[idx_of_place];
    
    btn_places[idx_of_place].initPanel( (byte)idx_of_place, (byte)cur_panel.ownerId, cur_panel.inputElements );
    
    changeColorPlace( idx_of_place );
    gameObject.SetActive( false );// Тушим панель
  }

  private void changeColorPlace( int idx_of_place )
  {
    setAllPlacesGreen();
    btn_places[idx_of_place].GetComponent<Image>().color = Color.black;
  }

  private void setAllPlacesGreen()
  {
    foreach ( PanelInputs btn in btn_places )
      btn.GetComponent<Image>().color = Color.green;
  }

  public Transform[] getInputSpawnPosition => spawn_point;
}
