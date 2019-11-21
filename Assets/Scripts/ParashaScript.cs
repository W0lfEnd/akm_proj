using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ParashaScript : MonoBehaviour
{
    [SerializeField] public TMP_InputField textName;

    public void saveNickname()
    {
        CommonData.NickName = textName.text;
    }
}
