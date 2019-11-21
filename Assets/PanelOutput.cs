using System;
using TMPro;
using UnityEngine;


public class PanelOutput : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI ui_text = null;

  private string prefix = String.Empty;

  public void setPrefix( string prefix ) => this.prefix = prefix;
  
  public void setText( string text ) => ui_text.text = $"{prefix} {text}";
}
