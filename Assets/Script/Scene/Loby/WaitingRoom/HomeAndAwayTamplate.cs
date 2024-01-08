using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HomeAndAwayTamplate : MonoBehaviour
{
    [SerializeField]
    private GameObject _objHomeAndAway = null;

    [SerializeField]
    private TMP_Text _textHomeAndAwayLabel = null;

    [SerializeField]
    private GameObject _objPlayer = null;

    [SerializeField]
    private TMP_Text _textPlayerNickName = null;

    private bool _isPlayer = false;

    [Space(10f)]

    [SerializeField]
    private TMP_Dropdown _dropDownRace = null;

    private string _playerID = string.Empty;

    public bool isPlayer
    {
        get { return _isPlayer; }
    }

    public void Initialize()
    {
        _objHomeAndAway.gameObject.SetActive(false);
        _objPlayer.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public eRace GetRaceValue(out string id)
    {
        id = _playerID;

        return (eRace)_dropDownRace.value;
    }

    public void SetHomeAndAway(string label)
    {
        _isPlayer = false;

        _textHomeAndAwayLabel.text = label;

        _objHomeAndAway.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void SetPlayer(string playerID, string label, bool isMe)
    {
        _isPlayer = true;

        _playerID = playerID;
        _textPlayerNickName.text = label;

        _objPlayer.gameObject.SetActive(true);
        _dropDownRace.gameObject.SetActive(isMe);
        this.gameObject.SetActive(true);
    }
}
