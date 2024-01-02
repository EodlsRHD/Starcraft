using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Windows : MonoBehaviour
{
    [SerializeField]
    private Settings _settings = null;

    private Action<bool> _onInterfaceBackgtroundActivationCallback = null;

    public void Initialize(Action<bool> onInterfaceBackgtroundActivationCallback)
    {
        _settings.Initialize(CloseSettings);

        if(onInterfaceBackgtroundActivationCallback != null)
        {
            _onInterfaceBackgtroundActivationCallback = onInterfaceBackgtroundActivationCallback;
        }
    }

    public void OpenSettings()
    {
        _settings.Open();
    }

    private void CloseSettings()
    {
        _settings.Close();
        _onInterfaceBackgtroundActivationCallback?.Invoke(false);
    }
}
