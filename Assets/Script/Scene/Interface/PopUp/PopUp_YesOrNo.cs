using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopUp_YesOrNo : MonoBehaviour, IPopUp
{
    [SerializeField]
    private TMP_Text _textTitle = null;

    [SerializeField]
    private TMP_Text _textDescription = null;

    [SerializeField]
    private TMP_Text _textButtonCancel = null;

    [SerializeField]
    private TMP_Text _textButtonConfirm = null;

    [Space(10)]

    [SerializeField]
    private Button _buttonCancel = null;

    [SerializeField]
    private Button _buttonConfirm = null;

    private Action _onCancelCallback = null;

    private Action _onConfirmCallback = null;

    public void initilaize(string title, string description, Action onCancelCallback, Action onConfirmCallback)
    {
        _textTitle.text = title;
        _textDescription.text = description;

        if(onCancelCallback != null)
        {
            _onCancelCallback = onCancelCallback;
        }

        if(onConfirmCallback != null)
        {
            _onConfirmCallback = onConfirmCallback;
        }

        _buttonCancel.onClick.AddListener(OnCancel);
        _buttonConfirm.onClick.AddListener(OnConfirm);

        this.gameObject.SetActive(true);
    }

    public void SetButtonText(string cancel, string confirm)
    {
        if(cancel.Length > 0)
        {
            _textButtonCancel.text = cancel;
        }

        if (confirm.Length > 0)
        {
            _textButtonConfirm.text = confirm;
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
        _textButtonConfirm.text = "Confirm";

        _onCancelCallback = null;
        _onConfirmCallback = null;
    }

    void IPopUp.initilaize(string title, string description, Action onCancelCallback)
    {
        throw new NotImplementedException();
    }

    void IPopUp.SetButtonText(string cancel)
    {
        throw new NotImplementedException();
    }
}
