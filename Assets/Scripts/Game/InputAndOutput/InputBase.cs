using System;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InputBase : MonoBehaviour
{
  private byte panel_id;
  private GameController.ButtonActionType input_element_id;
  private byte max_lenght;

  [SerializeField] private TextMeshProUGUI text;
  
  
  public void initInput( byte panel_id, byte input_element_id, byte max_lenght )
  {
    this.panel_id = panel_id;
    this.input_element_id = (GameController.ButtonActionType)input_element_id;
    this.max_lenght = max_lenght;

    if( text )
      text.text = InputTextHelper.getInputName( this.input_element_id );
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
      inputElementId = (byte)input_element_id,
      inputValue = input_value,
    });
    
    Debug.LogWarning( $"{gameObject.name} is changed  id: {(int)input_element_id} value: {input_value}" );
  }
}
