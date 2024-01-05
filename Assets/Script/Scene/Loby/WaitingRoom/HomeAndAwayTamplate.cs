using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public void Initialize()
    {
        _objHomeAndAway.gameObject.SetActive(false);
        _objPlayer.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void SetHomeAndAway(string label)
    {
        _textHomeAndAwayLabel.text = label;

        this.gameObject.SetActive(true);
        _objHomeAndAway.gameObject.SetActive(true);
    }

    public void SetPlayer(string label)
    {
        _textPlayerNickName.text = label;

        this.gameObject.SetActive(true);
        _objPlayer.gameObject.SetActive(true);
    }
}
