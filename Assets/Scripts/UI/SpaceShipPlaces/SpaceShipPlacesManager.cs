using System;
using Model;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;


public class SpaceShipPlacesManager : MonoBehaviourBase
{
  [SerializeField] private PanelInputs[] btn_places;
  [SerializeField] private Transform[] spawn_point;
  [SerializeField] private PanelOutput[] test_outputs;

  private long my_id => Client.client.Id;


  protected override void initMyComponents()
  {
    //TEST
    test_outputs[0].setPrefix( "speed:" );
    test_outputs[0].setText( Client.client.Model.speed.Value.ToString() );
    Client.client.Model.speed.onValueChange += b => test_outputs[0].setText( b.ToString() );

    test_outputs[1].setPrefix( "health:" );
    test_outputs[1].setText( Client.client.Model.health.Value.ToString() );
    Client.client.Model.health.onValueChange += b => test_outputs[1].setText( b.ToString() );

    test_outputs[2].setPrefix( "oxygen:" );
    test_outputs[2].setText( Client.client.Model.oxygen.Value.ToString() );
    Client.client.Model.oxygen.onValueChange += b => test_outputs[2].setText( b.ToString() );

    test_outputs[3].setPrefix( "petrol:" );
    test_outputs[3].setText( Client.client.Model.petrol.Value.ToString() );
    Client.client.Model.petrol.onValueChange += b => test_outputs[30].setText( b.ToString() );

    test_outputs[4].setPrefix( "shield:" );
    test_outputs[4].setText( Client.client.Model.shield.Value.ToString() );
    Client.client.Model.shield.onValueChange += b => test_outputs[4].setText( b.ToString() );
  }

  private void Update()
  {

    if ( Client.client != null && Client.client.Model != null )
      initComponents();
    else return;
    
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
