using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipSectorsManager : MonoBehaviourBase
{
    [SerializeField] private List<ShipSectorUI> sectors;
    protected override void initMyComponents()
    {
        for ( int i = 0; i < Client.client.Model.sectors.Length; i++ )
            sectors[i].init( Client.client.Model.sectors[i] );
    }

    private void init()
    {
        initComponents();
    }

    private void OnEnable()
    {
        init();
    }
}
