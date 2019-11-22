using Model;


public static class InputTextHelper
{
  public static string getInputName( GameController.ButtonActionType type )
  {
    switch ( type )
    {
      case GameController.ButtonActionType.ComboManeuver1Id: return "☆";
      case GameController.ButtonActionType.ComboManeuver2Id: return "☈";
      case GameController.ButtonActionType.ComboManeuver3Id: return "☘";
      case GameController.ButtonActionType.ComboManeuver4Id: return "☯";
      case GameController.ButtonActionType.ComboManeuver5Id: return "♆";
      case GameController.ButtonActionType.ComboManeuver6Id: return "⛛";
    }

    return "⛱";
  }
}