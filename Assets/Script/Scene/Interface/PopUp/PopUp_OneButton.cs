using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUp_OneButton : MonoBehaviour, IPopUp
{
    [SerializeField]
    private TMP_Text _textTitle = null;

    [SerializeField]
    private TMP_Text _textDescription = null;

    [SerializeField]
    private TMP_Text _textButtonCancel = null;

    [Space(10)]

    [SerializeField]
    private Button _buttonCancel = null;

    private Action _onCancelCallback = null;

    private Action _onConfirmCallback = null;

    public void initilaize(string title, string description, Action onCancelCallback)
    {
        _textTitle.text = title;
        _textDescription.text = description;

        if (onCancelCallback != null)
        {
            _onCancelCallback = onCancelCallback;
        }

        _buttonCancel.onClick.AddListener(OnCancel);

        this.gameObject.SetActive(true);
    }

    public void SetButtonText(string cancel)
    {
        if (cancel.Length > 0)
        {
            _textButtonCancel.text = cancel;
        }
    }


    public void OnCancel()
    {
        _onCancelCallback?.Invoke();
    }

    public void OnConfirm()
    {
        _onConfirmCallback?.Invoke();
    }

    public void RemoveData()
    {
        this.gameObject.SetActive(false);

        _textTitle.text = string.Empty;
        _textDescription.text = string.Empty;

        _textButtonCancel.text = "Cancel";

        _onCancelCallback = null;
    }

    void IPopUp.initilaize(string title, string description, Action onCancelCallback, Action onConfirmCallback)
    {
        throw new NotImplementedException();
    }

    void IPopUp.SetButtonText(string cancel, string confirm)
    {
        throw new NotImplementedException();
    }
}
