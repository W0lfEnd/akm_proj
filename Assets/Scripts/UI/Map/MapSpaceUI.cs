using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;


public class MapSpaceUI : MonoBehaviourBase
{
  [SerializeField] private Image         img_ship;
  [SerializeField] private RectTransform rect_space_map_background;
  [SerializeField] private Image         img_ship_to_target_connector;

  [SerializeField] private GameObject location_prefab;

  private readonly List<MapSpaceLocationPointUI> location_points = new List<MapSpaceLocationPointUI>();

  private MapSpaceLocationPointUI target_location = null;

  protected override void initMyComponents()
  {
    foreach ( Vector2Int location in Client.client.Map.coords )
    {
      MapSpaceLocationPointUI location_script = Instantiate( location_prefab, rect_space_map_background.transform ).GetComponent<MapSpaceLocationPointUI>();
      location_script.transform.localPosition = fromServerCoords( location );
      location_script.onLocationClicked += onLocationClicked;
      location_script.init();

      location_points.Add( location_script );
    }
  }

  
  public void OnEnable()
  {
    initComponents();
    
    img_ship.transform.localPosition = fromServerCoords( Client.client.Model.curPosition.Value );
  }


  public void onLocationClicked( MapSpaceLocationPointUI location )
  {
    target_location = location;
    foreach ( MapSpaceLocationPointUI l in location_points )
      l.init( l == location );
  }

  private void Update()
  {
    if ( !was_inited_components )
      return;

    img_ship.transform.localPosition = fromServerCoords( Client.client.Model.curPosition.Value );
    drawShipToTargetConnector();
  }

  private void drawShipToTargetConnector()
  {
    img_ship_to_target_connector.enabled = (bool)target_location;
    if ( !target_location )
      return;

    img_ship_to_target_connector.transform.position = img_ship.transform.position;
    img_ship_to_target_connector.rectTransform.sizeDelta = new Vector2( img_ship_to_target_connector.rectTransform.sizeDelta.x, Vector2.Distance( img_ship.transform.position, target_location.transform.position ) );
  }

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
}