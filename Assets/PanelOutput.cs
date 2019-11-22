using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PanelOutput : MonoBehaviour
{
  [SerializeField] private Text ui_text = null;

  private string prefix = String.Empty;

  public void setPrefix( string prefix ) => this.prefix = prefix;
  
  public void setText( string text ) => ui_text.text = $"{prefix} {text}";
}
