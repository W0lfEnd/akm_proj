using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenusController : MonoBehaviour
{
  [SerializeField] private GameObject main_menu_ui_root;
  [SerializeField] private GameObject game_ui_root;
  [SerializeField] private AudioSource main_menu_music;
  [SerializeField] private AudioSource game_music;
  
  public void goToGame()
  {
    main_menu_ui_root.SetActive( false );
    game_ui_root.SetActive( true );
    main_menu_music.Stop();
    game_music.Play();
  }

  public void goToMainMenu()
  {
    main_menu_ui_root.SetActive( true );
    game_ui_root.SetActive( false );
    main_menu_music.Play();
    game_music.Stop();
  }
}