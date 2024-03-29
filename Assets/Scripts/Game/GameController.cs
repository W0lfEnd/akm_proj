﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController
{
    public enum ButtonActionType: byte
    {
        ComboBtn1Id = 0,
        ComboBtn2Id = 1,
        ComboBtn3Id = 2,
        ComboBtn4Id = 3,

        RunBtnId = 4,

        ChangeSppedId = 5,

        MapOutputId = 6,
        ShipStateOutputId = 7,

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
    private int _time = 0;

    public GameController()
    {
        _map = new Map();

        _model = StartGameModelGenerator.Generate(5, 4);
        _model.gameState.Value = GameState.INIT;

        _model.maneverComboValidState = new bool[_map.meteorsData[0].combo.Length];
    }

    public void DoMeteorIteration()
    {
        _model.gameState.Value = GameState.RUN;
        if (_model == null || _model.gameState.Value != GameState.RUN)
        {
            return;
        }

        if (_model.currentTime.Value == _map.meteorsData[_meteorIteration].timeSeconds)
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
                            if (!result[i])
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
            _model.maneverComboValidState = new bool[_map.meteorsData[_meteorIteration].combo.Length];
        }
    }

    public void DoIterationOnEachSeconds()
    {
        _model.gameState.Value = GameState.RUN;
        if ( _model == null || _model.gameState.Value != GameState.RUN )
        {
            return;
        }

        if (_model.curPosition == _model.targetPosition)
        {
            _model.speed.Value = 0;
        }

        _model.health.Value = (short)_model.sectors.Sum(s => s.health);
        moveShipToTarget();

        IncreaseShield();
        CalcOxygen();
        CalcSectorHealth();
        ++_time;
        _model.currentTime.Value = _time;
        _model.health.Value = (short)_model.sectors.Sum(s => s.health);
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
            if (_model.sectors[i].isFire)
            {
                var dmg = _model.sectors[i].health - 4;
                if (dmg <= 0)
                {
                    _model.sectors[i].health = 0;
                }
                else
                {
                    _model.sectors[i].health -= 4;
                }
            }

            if (_model.sectors[i].isRepairing)
            {
                var h = _model.sectors[i].health + 5;
                if (h > 100)
                {
                    _model.sectors[i].health = 100;
                }
                else
                {
                    _model.sectors[i].health += 5;
                }
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
        Vector2 res_in_float = Vector2.MoveTowards( _model.curPosition.Value, _model.targetPosition.Value, Time.fixedDeltaTime * _model.speed.Value * 10 );
        _model.curPosition.Value = new Vector2Int( (int)res_in_float.x, (int)res_in_float.y );
    }

    public bool ApplyPlayerInput(PlayerInput playerInput)
    {
        if(playerInput.actionType == PlayerInput.ActionType.ChangePanel)
        {
            return ChangePanel(playerInput);
        }

        if(playerInput.actionType == PlayerInput.ActionType.ChangeTarget)
        {
            _model.targetPosition.Value = playerInput.targetPosition;
            return true;
        }

        if( playerInput.actionType == PlayerInput.ActionType.FightFire)
        {
            _model.sectors[playerInput.sectorPosition].isFire = false;
        }

        if(playerInput.actionType == PlayerInput.ActionType.Repair)
        {
            _model.sectors[playerInput.sectorPosition].isRepairing = true;
        }
        
        if (!ValidInputElement(playerInput, out InputElement inputElement))
        {
            return false;
        }
        inputElement.inputValue = playerInput.inputValue;

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
                _model.speed.Value = (byte)(inputElement.inputValue * 10);
                break;
            case ButtonActionType.ComboManeuver1Id:
            case ButtonActionType.ComboManeuver2Id:
            case ButtonActionType.ComboManeuver3Id:
            case ButtonActionType.ComboManeuver4Id:
            case ButtonActionType.ComboManeuver5Id:
            case ButtonActionType.ComboManeuver6Id:
                var combo = _map.meteorsData[_model.iteration.Value].combo;
                var index = Array.FindIndex(combo, i => i == (byte)actionType);
                if ( index == -1 || _model.maneverComboValidState.All( c => c == true))
                {
                    return true;
                }

                for (int i = 0; i < _model.maneverComboValidState.Length; i++)
                {
                    if(_model.maneverComboValidState[i])
                    {
                        continue;
                    }

                    if (index == i)
                    {
                        _model.maneverComboValidState[index] = true;
                    }
                    else
                    {
                        _model.maneverComboValidState = new bool[combo.Length];
                        for (int c = 0; c < combo.Length; c++)
                        {
                            for (int p = 0; p < _model.panels.Length; p++)
                            {
                                for (int e = 0; e < _model.panels[p].inputElements.Length; e++)
                                {
                                    var element = _model.panels[p].inputElements[e];
                                    if (combo[c] == element.id)
                                    {
                                        element.inputValue = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                
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
        short damage = 300;
        byte targetCount = 2;
        if( meteorSize == 1)
        {
            damage = 700;
            targetCount = 3;
        }
        else if( meteorSize == 2)
        {
            damage = 900;
            targetCount = 4;
        }

        var damageSector = _model.shield.Value - damage;

        if (damageSector <= 0)
        {
            _model.shield.Value = 0;
            damageSector = Math.Abs(damageSector);
        }
        else
        {
            _model.shield.Value -= damage;
        }
        if (_model.shield.Value <= 0)
        {
            var goodSectors = _model.sectors.Where(s => s.health > 0).Select(s => s.position).ToList();
            var sectorIds = Util.ShuffleList(goodSectors).Take(targetCount);
            damage = (short)(damageSector / targetCount);

            for (int i = 0; i < targetCount; i++)
            {
                var sector = _model.sectors.FirstOrDefault(s => sectorIds.Contains(s.position));
                if (sector == null)
                {
                    _model.gameState.Value = GameState.LOSE;
                    return;
                }
                if (damage > sector.health)
                {
                    sector.health = 0;
                }
                else
                {
                    sector.health -= (byte)damage;
                }
                sector.isFire = new System.Random().Next(0, 10) < 4;
            }
        }

        if (_model.health.Value <= 0)
        {
            _model.gameState.Value = GameState.LOSE;
        }
    }

    private void IncreaseShield()
    {
        if (_model.shield.Value >= 800 || _model.sectors.First( s => s.sectorType == SectorType.shield ).health == 0 )
        {
            return;
        }
        _model.shield.Value += 1;
    }

    private void InscreaseOxygen()
    {
        if (_model.oxygen.Value >= MAX_VALUE )
        {
            return;
        }
        _model.oxygen.Value += 1;
    }

    private void DecreaseOxygen( byte value )
    {
        var dmg = _model.oxygen.Value - value;
        if (dmg <= 0)
        {
            _model.oxygen.Value = 0;
            _model.gameState.Value = GameState.LOSE;
        }
        else
        {
            _model.oxygen.Value -= value;
        }
    }
}

