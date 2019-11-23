using Model;
using UnityEngine;


public static class InputTextHelper
{
  public static string getInputName( GameController.ButtonActionType type )
  {
    return ((int)type).ToString();
    switch ( type )
    {
      case GameController.ButtonActionType.ComboManeuver1Id: return "1";
      case GameController.ButtonActionType.ComboManeuver2Id: return "2";
      case GameController.ButtonActionType.ComboManeuver3Id: return "3";
      case GameController.ButtonActionType.ComboManeuver4Id: return "4";
      case GameController.ButtonActionType.ComboManeuver5Id: return "5";
      case GameController.ButtonActionType.ComboManeuver6Id: return "6";
    }

    return "";
  }
}