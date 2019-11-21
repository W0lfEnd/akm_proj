using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Model;
using UnityEngine;

public class PanelInputs : MonoBehaviour
{
  [SerializeField] private SpaceShipPlacesManager ship_places_manager;
  
  private byte panel_id;
  private byte owner_id;
  private InputElement[] input_elements;

  public InputBase button_prefab = null;
  public InputBase toggle_prefab = null;
  public InputBase slider_prefab = null;

  public Transform content_root = null;

  public void initPanel( byte panel_id, byte owner_id, InputElement[] input_elements )
  {
    this.panel_id       = panel_id;
    this.owner_id       = owner_id;
    this.input_elements = input_elements;

    spawnElements();
  }

  public void deinit()
  {
    foreach ( Transform child in content_root )
    {
      Destroy( child.gameObject );
    }
  }

  private void spawnElements()
  {
    for ( int i = 0; i < input_elements.Length; i++ )
    {
      switch ( input_elements[i].inputType )
      {
        case InputType.Button:
          spawnElement( button_prefab, input_elements[i], (byte)i );
          break;
        case InputType.Toggle:
          spawnElement( toggle_prefab, input_elements[i], (byte)i );
          break;
        case InputType.Slider: 
          spawnElement( slider_prefab, input_elements[i], (byte)i );
          break;
        
        default: throw new ArgumentOutOfRangeException();
      }
    }
  }

  private void spawnElement( InputBase input_prefab, InputElement inputElemen, byte id )
  {
    InputBase element = Instantiate( input_prefab, ship_places_manager.getInputSpawnPosition[id].position, Quaternion.identity, content_root  );
    element.initInput(id , inputElemen.id, inputElemen.maxValue );
  }
}
