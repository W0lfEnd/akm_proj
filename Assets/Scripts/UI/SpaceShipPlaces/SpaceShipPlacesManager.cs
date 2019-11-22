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

  private long my_id => Client.client.Id;
  

  private void Start()
  {
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
    

    // foreach ( Panel panel in Client.client.Model.panels )
    // {
    //   panel.ownerId.onValueChange +=
    //     value =>
    //     {
    //       btn_places[panel.id].GetComponent<Image>().color =
    //         value == 255 || value == -1 ?new Color( 0.82f, 0.82f, 0.82f ) : value == my_id ? Color.red : Color.green;
    //       
    //       btn_places[panel.id].GetComponent<CanvasGroup>().interactable = value == 255 || value == -1;
    //     };
    // }
  }

  private void Update()
  {
    Debug.Log( Client.client.Model.speed.Value );
    
    foreach ( Panel panel in Client.client.Model.panels )
    {
      long value = panel.ownerId;

      btn_places[panel.id].GetComponent<Image>().color =
       value == -1 ? new Color( 0.82f, 0.82f, 0.82f ) : value == my_id ? Color.green : Color.red;
          
      btn_places[panel.id].GetComponent<CanvasGroup>().interactable = value == -1;
    }
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
    Client.client.Send( new PlayerInput{ ownerId = my_id, actionType = PlayerInput.ActionType.ChangePanel, panelId = (byte)idx_of_place } );

    
    btn_places[idx_of_place].initPanel( (byte)idx_of_place, (byte)cur_panel.ownerId, cur_panel.inputElements );
    
    gameObject.SetActive( false );// Тушим панель
  }

  public Transform[] getInputSpawnPosition => spawn_point;
}
