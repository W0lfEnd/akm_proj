using System;
using Model;
using UnityEngine;
using UnityEngine.UI;


public class InputBase : MonoBehaviour
{
  private byte panel_id;
  private byte input_element_id;
  private byte max_lenght;
  
  
  public void initInput( InputType input_type, byte max_lenght )
  {
    this.max_lenght = max_lenght;
    
    switch ( input_type )
    {
      case InputType.Button: 
        
        break;
      case InputType.Toggle: break;
      case InputType.Slider: break;
      default: throw new ArgumentOutOfRangeException( nameof( input_type ), input_type, null );
    }
  }

  #region onValueChange
  public void onValueChangeToggle()
  {
    Toggle toggle = GetComponent<Toggle>();

    if ( toggle.isOn )
      send( 1 );
    else
      send( 0 );
  }

  public void onClickBtn()
  {
    send( 1 );
  }

  public void onSliderChange()
  {
    Slider slider = GetComponent<Slider>();

    send( (byte)slider.value );
  }
  #endregion
  
  private void send( byte input_value )
  {
    Client.client.Send(new PlayerInput
    {
      ownerId = Client.client.Id,
      actionType = PlayerInput.ActionType.PressButton,
      panelId = panel_id,
      inputElementId = input_element_id,
      inputValue = input_value,
    });
    
    Debug.LogWarning( $"{gameObject.name} is changed value: {input_value}" );
  }
}
