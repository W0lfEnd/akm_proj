using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
   [SerializeField] private GameObject main_menu;
   [SerializeField] private GameObject create_game_menu;
   [SerializeField] private GameObject join_to_game_menu;

   public void goToMainMenu()
   {
      main_menu.SetActive( true );
      create_game_menu.SetActive( false );
      join_to_game_menu.SetActive( false );
   }
   
   public void goToCreateGameMenu()
   {
      main_menu.SetActive( false );
      create_game_menu.SetActive( true );
      join_to_game_menu.SetActive( false );
   }
   
   public void goToJoinToGameMenu()
   {
      main_menu.SetActive( false );
      create_game_menu.SetActive( false );
      join_to_game_menu.SetActive( true );
   }
}
