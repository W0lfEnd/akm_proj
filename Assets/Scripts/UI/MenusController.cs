using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenusController : MonoBehaviour
{
  [SerializeField] private GameObject main_menu_ui_root;
  [SerializeField] private GameObject game_ui_root;

  public void goToGame()
  {
    main_menu_ui_root.SetActive( false );
    game_ui_root.SetActive( true );
  }

  public void goToMainMenu()
  {
    main_menu_ui_root.SetActive( true );
    game_ui_root.SetActive( false );
  }
}