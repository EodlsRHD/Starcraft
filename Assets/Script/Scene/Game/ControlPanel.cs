using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum eOrder
{
    Non = -1,
    Move,
    Stop,
    Attack,
    Patrol,
    Hold,
    Custom_1,
    Custom_2,
    Custom_3,
    Custom_4
}

public class ControlPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _objButtons = null;

    [SerializeField]
    private Button _buttonMove = null;

    [SerializeField]
    private Button _buttonStop = null;

    [SerializeField]
    private Button _buttonAttack = null;

    [SerializeField]
    private Button _buttonPatrol = null;

    [SerializeField]
    private Button _buttonHold = null;

    [SerializeField]
    private Button _buttonCustom_1 = null;

    [SerializeField]
    private Button _buttonCustom_2 = null;

    [SerializeField]
    private Button _buttonCustom_3 = null;

    [SerializeField]
    private Button _buttonCustom_4 = null;

    private int _buildingTakeOff_Key = 0;
    private int _buildingLanding_key = 0;

    private Action<eOrder> _onOrderCallback = null;

    public void Initialize(Action<eOrder> onOrderCallback)
    {
        if(onOrderCallback != null)
        {
            _onOrderCallback = onOrderCallback;
        }


        _buttonMove.onClick.AddListener(() => { OnOrder(eOrder.Move); });
        _buttonStop.onClick.AddListener(() => { OnOrder(eOrder.Stop); });
        _buttonAttack.onClick.AddListener(() => { OnOrder(eOrder.Attack); });
        _buttonPatrol.onClick.AddListener(() => { OnOrder(eOrder.Patrol); });
        _buttonHold.onClick.AddListener(() => { OnOrder(eOrder.Hold); });
        _buttonCustom_1.onClick.AddListener(() => { OnOrder(eOrder.Custom_1); });
        _buttonCustom_2.onClick.AddListener(() => { OnOrder(eOrder.Custom_2); });
        _buttonCustom_3.onClick.AddListener(() => { OnOrder(eOrder.Custom_3); });
        _buttonCustom_4.onClick.AddListener(() => { OnOrder(eOrder.Custom_4); });

        _buttonCustom_1.gameObject.SetActive(false);
        _buttonCustom_2.gameObject.SetActive(false);
        _buttonCustom_3.gameObject.SetActive(false);
        _buttonCustom_4.gameObject.SetActive(false);

        _objButtons.SetActive(false);
    }

    public void SetButtons(List<ObjectTamplate> datas)
    {
        if(datas[0].friendIdentificationType == eFriendIdentification.Alliance || datas[0].friendIdentificationType == eFriendIdentification.Enemy)
        {
            _objButtons.gameObject.SetActive(false);
            return;
        }

        switch(datas[0].GetData().objType)
        {
            case eObject.Building:
                {
                    if (datas[0].GetData().isAir == true)
                    {
                        if (_buildingTakeOff_Key == 0)
                        {
                            GameManager.instance.toolManager.RequestPool("Building_TakeOff", ePoolType.Image, (result) =>
                            {
                                _buildingTakeOff_Key = result.key;
                                _buttonCustom_4.image.sprite = result.GetSprite();

                            });
                        }
                        else
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Image, _buildingTakeOff_Key, (result) =>
                            {
                                _buttonCustom_4.image.sprite = result.GetSprite();
                            });
                        }
                    }
                    else
                    {
                        if (_buildingLanding_key == 0)
                        {
                            GameManager.instance.toolManager.RequestPool("Building_Landing", ePoolType.Image, (result) =>
                            {
                                _buildingLanding_key = result.key;
                                _buttonCustom_4.image.sprite = result.GetSprite();
                            });
                        }
                        else
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Image, _buildingLanding_key, (result) =>
                            {
                                _buttonCustom_4.image.sprite = result.GetSprite();
                            });
                        }
                    }

                    _buttonCustom_4.gameObject.SetActive(true);
                }

                break;

            case eObject.Unit:
                {
                    if(datas.Count == 1)
                    {
                        ObjectData data = datas[0].GetData();

                        _buttonCustom_1.gameObject.SetActive(data.custom.hasCustom_1);
                        _buttonCustom_2.gameObject.SetActive(data.custom.hasCustom_2);
                        _buttonCustom_3.gameObject.SetActive(data.custom.hasCustom_3);
                        _buttonCustom_4.gameObject.SetActive(data.custom.hasCustom_4);

                        if (data.custom.hasCustom_1 == true)
                        {

                        }


                        if (data.custom.hasCustom_2 == true)
                        {

                        }


                        if (data.custom.hasCustom_3 == true)
                        {

                        }


                        if (data.custom.hasCustom_4 == true)
                        {

                        }
                    }
                    else if(datas.Count > 1)
                    {
                        for (int i = 0; i < datas.Count; i++)
                        {
                            ObjectData data = datas[i].GetData();
                        }
                    }
                }
                break;
        }

        _objButtons.SetActive(true);
    }

    private void OnOrder(eOrder order)
    {
        _onOrderCallback?.Invoke(order);
    }

    public void OrderPress(eOrder order)
    {
        _onOrderCallback?.Invoke(order);
    }
}
