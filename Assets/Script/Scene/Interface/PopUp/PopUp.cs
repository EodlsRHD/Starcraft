using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPopUp
{
    public void initilaize(string title, string description, Action onCancelCallback, Action onConfirmCallback);

    public void initilaize(string title, string description, Action onCancelCallback);

    public void SetButtonText(string cancel, string confirm);

    public void SetButtonText(string cancel);

    public void OnCancel();

    public void OnConfirm();

    public void RemoveData();
}

public class PopUp : MonoBehaviour
{
    [SerializeField]
    private PopUp_YesOrNo _yesOrNo = null;

    [SerializeField]
    private PopUp_OneButton _oneButton = null;

    private IPopUp _activePopUp = null;

    public void Initialize()
    {
           
    }

    public void ClosePopUp()
    {
        _activePopUp.RemoveData();
        _activePopUp = null;
    }

    public void SetYesOrNo(string title, string description, Action onCancelCallback, Action onConfirmCallback, string cancel = null, string confirm = null)
    {
        _activePopUp = _yesOrNo;

        _yesOrNo.SetButtonText(cancel, confirm);
        _yesOrNo.initilaize(title, description, onCancelCallback, onConfirmCallback);
    }

    public void SetOneButton(string title, string description, Action onCancelCallback, string cancel = null)
    {
        _activePopUp = _oneButton;

        _oneButton.SetButtonText(cancel);
        _oneButton.initilaize(title, description, onCancelCallback);
    }
}
