using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.UI;


public class MapSpaceUI : MonoBehaviourBase
{
  [SerializeField] private ShipViewManager ship_view;
  [Space]
  [SerializeField] private Image         img_ship;
  [SerializeField] private RectTransform rect_space_map_background;
  [SerializeField] private Image         img_ship_to_target_connector;

  [SerializeField] private GameObject location_prefab;

  private readonly List<MapSpaceLocationPointUI> location_points = new List<MapSpaceLocationPointUI>();

  private MapSpaceLocationPointUI target_location = null;
  private Camera main_camera;

  protected override void initMyComponents()
  {
    main_camera = Camera.main;
    foreach ( Vector2Int location in Client.client.Map.coords )
    {
      MapSpaceLocationPointUI location_script = Instantiate( location_prefab, rect_space_map_background.transform ).GetComponent<MapSpaceLocationPointUI>();
      location_script.transform.localPosition = fromServerCoords( location );
      location_script.onLocationClicked += onLocationClicked;
      location_script.init( location, false );

      location_points.Add( location_script );
    }

    onLocationClicked( location_points[0] ); //this is temp start choose location, todo delete
    Client.client.Model.curPosition   .onValueChange += onShipMoved;
    Client.client.Model.targetPosition.onValueChange += onLocationChanged;
    Client.client.Model.speed.onValueChange += new_speed => ship_view.speed_parallax = new_speed / 10.0f;
  }


  public void OnEnable()
  {
    initComponents();

    onShipMoved( Client.client.Model.curPosition.Value );
  }


  #region UI
  private void updateLocations()
  {
    foreach ( MapSpaceLocationPointUI l in location_points )
      l.isSelected = l == target_location;
  }

  private void updateWayConnector()
  {
    img_ship_to_target_connector.enabled = (bool)target_location;
    if ( !target_location )
      return;

    img_ship_to_target_connector.transform.position = img_ship.transform.position;
    img_ship_to_target_connector.rectTransform.sizeDelta =
      new Vector2(
        Vector2.Distance( main_camera.WorldToScreenPoint( img_ship.transform.position ),
          main_camera.WorldToScreenPoint( target_location.transform.position ) ),
        3
      );
    img_ship_to_target_connector.transform.rotation = img_ship_to_target_connector.transform.position.lookAtIn2D( target_location.transform.position );
  }

  private void updateShipRotation()
  {
    Quaternion rotation = img_ship.transform.position.lookAtIn2D( target_location.transform.position );
    img_ship.transform.rotation = rotation;
    ship_view.transform.rotation = rotation;
  }

  private void updateShipPosition( Vector2Int server_location_position )
  {
    target_location = location_points.First( location => location.pos == server_location_position );
  }
  #endregion

  #region On Event Funcs
  private void onLocationChanged( Vector2Int server_location_position )
  {
    updateShipPosition( server_location_position );
    updateShipRotation();
    updateLocations();
    updateWayConnector();
  }

  private void onLocationClicked( MapSpaceLocationPointUI location )
  {
    Client.client.Send( new PlayerInput { actionType = PlayerInput.ActionType.ChangeTarget, ownerId = Client.client.Id, targetPosition = location.pos } );
    onLocationChanged( location.pos );
  }

  private void onShipMoved( Vector2Int server_ship_coords )
  {
    img_ship.transform.localPosition = fromServerCoords( server_ship_coords );
    updateWayConnector();
  }
  #endregion

  #region Helpers
  private Vector2 fromServerCoords( Vector2Int server_coords )
  {
    Vector2 normalized_coords = (Vector2)server_coords / Map.MAP_SIZE;
    return new Vector2( normalized_coords.x * rect_space_map_background.rect.width, normalized_coords.y * rect_space_map_background.rect.height );
  }

  private Vector2Int toServerCoords( Vector2 client_coords )
  {
    Vector2 normalized_coords = new Vector2( client_coords.x / rect_space_map_background.rect.width, client_coords.y / rect_space_map_background.rect.height );
    return new Vector2Int( (int)( normalized_coords.x * Map.MAP_SIZE ), (int)( normalized_coords.y * Map.MAP_SIZE ) );
  }
  #endregion
}