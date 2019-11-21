using System;
using Model;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;


public class SpaceShipPlacesManager : MonoBehaviour
{
  [SerializeField] private PanelInputs[] btn_places;
  [SerializeField] private Transform[] spawn_point;
  [SerializeField] private PanelOutput test1;
  [SerializeField] private PanelOutput test2;
  [SerializeField] private PanelOutput test3;
  [SerializeField] private PanelOutput test4;
  [SerializeField] private PanelOutput test5;

  private int my_id => Client.client.Id;

  

  private void Start()
  {
    setAllPlacesGreen();
    
    test1.setPrefix( "speed:" );
    test1.setText( Client.client.Model.speed.Value.ToString() );
    Client.client.Model.speed.onValueChange += b => test1.setText( b.ToString() );
    
    test2.setPrefix( "health:" );
    test2.setText( Client.client.Model.health.Value.ToString() );
    Client.client.Model.health.onValueChange += b => test2.setText( b.ToString() );
    
    test3.setPrefix( "oxygen:" );
    test3.setText( Client.client.Model.oxygen.Value.ToString() );
    Client.client.Model.oxygen.onValueChange += b => test3.setText( b.ToString() );
    
    test4.setPrefix( "petrol:" );
    test4.setText( Client.client.Model.petrol.Value.ToString() );
    Client.client.Model.petrol.onValueChange += b => test4.setText( b.ToString() );
    
    test5.setPrefix( "shield:" );
    test5.setText( Client.client.Model.shield.Value.ToString() );
    Client.client.Model.shield.onValueChange += b => test5.setText( b.ToString() );
  }

  private void Update()
  {
    Debug.Log( Client.client.Model.speed.Value );
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
