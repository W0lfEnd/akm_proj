using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController
{
    private enum ButtonActionType: byte
    {
        ComboBtn1Id = 0,
        ComboBtn2Id = 1,
        ComboBtn3Id = 2,
        ComboBtn4Id = 3,

        RunBtnId = 4,

        ChangeSppedId = 5,

        PressFire = 6,

        ComboManeuver1Id = 10,
        ComboManeuver2Id = 11,
        ComboManeuver3Id = 12,
        ComboManeuver4Id = 13,
        ComboManeuver5Id = 14,
        ComboManeuver6Id = 15,
    }

    private const int MAX_VALUE = 100;
    private GameModel _model;
    private Map _map;
    private int _meteorIteration = 0;
    private float startTime = 0;

    public GameController()
    {
        _model = StartGameModelGenerator.Generate(3, 5);
        _model.gameState.Value = GameState.INIT;

        _map = new Map();
    }

    public void DoIteration(float time)
    {
        _model.gameState.Value = GameState.RUN;

        if (_model.gameState.Value != GameState.RUN)
        {
            return;
        }

        if (startTime == 0)
        {
            startTime = time;
            _model.currentTime.Value = Convert.ToInt32(time * 1000);
        }
        // todo: time
        // _model.currentTime = time * 1000 - startTime
        if (Convert.ToInt32(_model.currentTime.Value) == _map.meteorsData[_meteorIteration].timeSeconds)
        {
            var combo = _map.meteorsData[_meteorIteration].combo;
            var result = new bool[combo.Length];
            for (int i = 0; i < combo.Length; i++)
            {
                for (int p = 0; p < _model.panels.Length; p++)
                {
                    for (int e = 0; e < _model.panels[p].inputElements.Length; e++)
                    {
                        var element = _model.panels[p].inputElements[e];
                        if (combo[i] == element.id)
                        {
                            result[i] = element.inputValue == element.maxValue;
                            if(!result[i])
                            {
                                goto Label;
                            }
                        }
                    }
                }

                Label:
                Collide(_map.meteorsData[_meteorIteration].size);
                break;
            } 
            ++_meteorIteration;
            _model.iteration.Value = _meteorIteration;
        }

        
        moveShipToTarget();
    }

    public void DoIterationOnEachSeconds()
    {
        if ( _model == null || _model.gameState.Value != GameState.RUN )
        {
            return;
        }

        IncreaseShield();
        CalcOxygen();
        CalcSectorHealth();
    }

    private void CalcOxygen()
    {
        var oxygenDamageCount = (byte)_model.sectors.Count(s => s.health <= 30);
        if ( oxygenDamageCount == 0 )
        {
            InscreaseOxygen();
        }
        else
        {
            DecreaseOxygen(oxygenDamageCount);
        }
    }

    private void CalcSectorHealth()
    {
        for (int i = 0; i < _model.sectors.Length; i++)
        {
            if ( _model.sectors[i].isFire )
            {
                _model.sectors[i].health -= 4;
            }

            if( _model.sectors[i].isRepairing )
            {
                _model.sectors[i].health += 5;
            }
        }
        
    }

    public Map GetMap()
    {
        return _map;
    }

    public GameModel GetModel()
    {
        return _model;
    }

    private void moveShipToTarget()
    {
        Vector2 res_in_float = Vector2.MoveTowards( _model.curPosition.Value, _model.targetPosition.Value, Time.fixedDeltaTime * 10 * _model.speed.Value );
        _model.curPosition.Value = new Vector2Int( (int)res_in_float.x, (int)res_in_float.y );
    }

    public bool ApplyPlayerInput(PlayerInput playerInput)
    {
        if (playerInput.actionType == PlayerInput.ActionType.ChangePanel)
        {
            return ChangePanel(playerInput);
        }

        if(playerInput.actionType == PlayerInput.ActionType.ChangeTarget)
        {
            _model.targetPosition.Value = playerInput.targetPosition;
            return true;
        }
        
        if (!ValidInputElement(playerInput, out InputElement inputElement))
        {
            return false;
        }

        var actionType = (ButtonActionType)inputElement.id;

        switch (actionType)
        {
            case ButtonActionType.ComboBtn1Id:
            case ButtonActionType.ComboBtn2Id:
            case ButtonActionType.ComboBtn3Id:
            case ButtonActionType.ComboBtn4Id:
                SetRunCombination(inputElement);
                break;
            case ButtonActionType.RunBtnId:
                if (_model.gameState.Value == GameState.PREPARE)
                {
                    _model.gameState.Value = GameState.RUN;
                }
                break;
            case ButtonActionType.ChangeSppedId:
                var speed = Convert.ToByte(inputElement.inputValue * 10);
                if(_model.gameState.Value == GameState.RUN && (speed > MAX_VALUE || speed < 0))
                {
                    return false;
                }
                _model.speed.Value = speed;
                break;
            case ButtonActionType.PressFire:
                break;
            case ButtonActionType.ComboManeuver1Id:
            case ButtonActionType.ComboManeuver2Id:
            case ButtonActionType.ComboManeuver3Id:
            case ButtonActionType.ComboManeuver4Id:
            case ButtonActionType.ComboManeuver5Id:
            case ButtonActionType.ComboManeuver6Id:
                break;
            default: return false;
        }

        return true;
    }

    private bool ChangePanel(PlayerInput playerInput)
    {
        if (_model.panels[playerInput.panelId].ownerId > -1)
        {
            return false;
        }
        
        var prev_panel = _model.panels.FirstOrDefault(p => p.ownerId == playerInput.ownerId);
        if (prev_panel != null)
        {
            prev_panel.ownerId = -1;
        }
        _model.panels[playerInput.panelId].ownerId = playerInput.ownerId;

        return true;
    }

    private bool ValidInputElement(PlayerInput playerInput, out InputElement inputElement)
    {
        inputElement = null;
        var panel = _model.panels.FirstOrDefault(p => p.ownerId == playerInput.ownerId && p.id == playerInput.panelId);
        if (panel == null)
        {
            return false;
        }

        inputElement = panel.inputElements.FirstOrDefault(ie => ie.id == playerInput.inputElementId);
        if (inputElement == null)
        {
            return false;
        }

        return true;
    }

    private bool SetRunCombination(InputElement inputElement)
    {
        if (_model.gameState.Value == GameState.RUN)
        {
            return true;
        }
        List<InputElement> inputs = new List<InputElement>();
        for (int i = 0; i < _model.panels.Length; i++)
        {
            for (int j = 0; j < _model.panels[i].inputElements.Length; j++)
            {
                if (_model.panels[i].inputElements[j].groupId == 2)
                {
                    inputs.Add(_model.panels[i].inputElements[j]);
                }
            }
        }

        var index = inputs.FindIndex(inp => inp.id == inputElement.id);
        int countGoodIteration = 0;
        for (int i = 0; i < _model.startCombo.Length; i++)
        {
            var input = inputs[_model.startCombo[i]];

            if (input.inputValue != input.maxValue)
            {
                for (int k = 0; k < inputs.Count; k++)
                {
                    inputs[k].inputValue = 0;
                }
                return false;
            }
            else
            {
                ++countGoodIteration;
            }

            if (index == _model.startCombo[i])
            {
                if (inputs.Count(inp => inp.inputValue > 0) != countGoodIteration)
                {
                    return false;
                }
                break;
            }
        }
        if (countGoodIteration == _model.startCombo.Length)
        {
            _model.gameState.Value = GameState.PREPARE;
        }
        return true;
    }

    private void Collide(byte meteorSize)
    {
        short damage = 150;
        byte targetCount = 2;
        if( meteorSize == 1)
        {
            damage = 350;
            targetCount = 3;
        }
        else if( meteorSize == 2)
        {
            damage = 600;
            targetCount = 4;
        }

        var damageSector = _model.shield.Value - damage;
        _model.shield.Value -= (ushort)damage;

        if (damageSector < 0 )
        {
            damageSector = Math.Abs(damageSector);
        }
        if (_model.shield.Value <= 0)
        {
            var sectorIds = Util.ShuffleList(0, 8).Take(targetCount);
            damage = (short)(damageSector / targetCount);

            for (int i = 0; i < targetCount; i++)
            {
                var sector = _model.sectors.First(s => sectorIds.Contains(s.position));
                sector.health -= (byte)damage;
                sector.isFire = new System.Random().Next(0, 10) < 4;
            }
            _model.shield.Value = 0;
        }

        if (_model.health.Value <= 0)
        {
            _model.gameState.Value = GameState.LOSE;
        }
    }

    private void IncreaseShield()
    {
        if (_model.oxygen.Value >= MAX_VALUE)
        {
            return;
        }
        _model.shield.Value += 1;
    }

    private void InscreaseOxygen()
    {
        if (_model.oxygen.Value >= MAX_VALUE)
        {
            return;
        }
        _model.oxygen.Value += 1;
    }

    private void DecreaseOxygen( byte value )
    {
        _model.oxygen.Value -= value;

        if (_model.oxygen.Value <= 0)
        {
            _model.gameState.Value = GameState.LOSE;
            return;
        }

        _model.oxygen.Value -= 1;
    }
}

