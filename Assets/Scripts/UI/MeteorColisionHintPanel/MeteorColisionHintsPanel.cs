﻿using Model;
using TMPro;
using UnityEngine;

public class MeteorColisionHintsPanel : MonoBehaviour
{
    [SerializeField] private GameObject meteor_hint_panel;
    [SerializeField] private TextMeshProUGUI meteor_combo_hint;

    private void Update()
    {
        if ( Client.client == null || Client.client.Map == null )
            return;

        
        
            
        MeteorData nearest_meteor = Client.client.Map.meteorsData[Client.client.Model.iteration.Value];
        int hint_time = 0;
        switch ( nearest_meteor.size )
        {
            case 0: hint_time = 6; break;
            case 1: hint_time = 9; break;
            case 2: hint_time = 12; break;
        }
        
        if ( nearest_meteor.timeSeconds - Client.client.Model.currentTime.Value < hint_time )
        {
            meteor_hint_panel.SetActive( true );
        }
        else
        {
            meteor_hint_panel.SetActive( false );
        }

        foreach ( var VARIABLE in Client.client.Model.maneverComboValidState )
        {
            meteor_combo_hint.text += VARIABLE.ToString();
        }
    }
    
    
}