using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameConsole : MonoBehaviour
{
    [SerializeField]
    private MiniMap _miniMap = null;

    [SerializeField]
    private Information _information = null;

    [SerializeField]
    private ControlPanel _controlPanel = null;

    public void Initialize(Action<eOrder> onOrderCallback)
    {
        _miniMap.Initialize();
        _information.Initialize();
        _controlPanel.Initialize(onOrderCallback);

        this.gameObject.SetActive(true);
    }

    public void GameStart()
    {

    }

    public void SetInformation(List<ObjectTamplate> tamplates, Action<ObjectData> onSetKeyCallback)
    {
        _information.SetInformation(tamplates, onSetKeyCallback);
    }

    public void SetControlPanal(List<ObjectTamplate> tamplates)
    {
        _controlPanel.SetButtons(tamplates);
    }

    public  void Order(eOrder order)
    {
        _controlPanel.OrderPress(order);
    }

    public void Upgrade(IObserver data)
    {
        _information.UpdateInfo(data);
    }
}
