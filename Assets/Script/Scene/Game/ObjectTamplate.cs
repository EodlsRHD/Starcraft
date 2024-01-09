using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ObjectTamplate : MonoBehaviour, IObserver, IUnit
{
    [SerializeField]
    private NavMeshAgent _agnet = null;

    [SerializeField]
    private eFriendIdentification _friendIdentificationType = eFriendIdentification.My;

    [SerializeField]
    private ObjectData _data = null;

    [SerializeField] // test
    private eOrder _handel = eOrder.Non;

    private Vector3 _v3MoveTarget;
    private Vector3 _v3OriginalPosition;

    private GameObject _objectTamplateTarget = null;

    private Action<ObjectData> _onUpdaeInformation = null;

    public eFriendIdentification friendIdentificationType
    {
        get { return _friendIdentificationType; }
    }

    public void Initialize(ObjectData data, Action<ObjectData> onUpdaeInformation)
    {
        _data = data;

        if(_data.objType == eObject.Resources)
        {
            return;
        }

        _agnet.speed = data.moveSpeed;

        if (onUpdaeInformation != null)
        {
            _onUpdaeInformation = onUpdaeInformation;
        }
    }

    public ObjectData GetData()
    {
        return _data;
    }

    public eRace BroodType()
    {
        return _data.raceType;
    }

    public eUnitType UnitType()
    {
        return _data.unitType;
    }

    public eUnitSizeType UnitSizeType()
    {
        return _data.unitSizeType;
    }

    public void SetSelection(bool isSelect)
    {
        if(_friendIdentificationType == eFriendIdentification.Alliance)
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }

        if (_friendIdentificationType == eFriendIdentification.Enemy)
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        if (_friendIdentificationType == eFriendIdentification.My)
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

        this.gameObject.transform.GetChild(0).gameObject.SetActive(isSelect);
    }

    public void SetUpgrade(eUpgrade type)
    {
        switch (type)
        {
            case eUpgrade.Attack:
                _data.metaData.upgradeAttack += 1;
                break;

            case eUpgrade.Defence:
                _data.metaData.upgradeAttack += 1;
                break;

            case eUpgrade.Shild:
                _data.metaData.upgradeAttack += 1;
                break;
        }
    }

    public void UpdateData(ObjectData data)
    {
        _data = data;
    }

    private void Update()
    {
        if (_data.objType == eObject.Resources)
        {
            return;
        }

        Vector3 pos = this.transform.position;

        Action(pos);
        DetectEnemy(pos);
    }

    private void Action(Vector3 pos)
    {
        switch (_handel)
        {
            case eOrder.Move:
                {
                    if (Vector3.Distance(_v3MoveTarget, pos) <= this.transform.localScale.x)
                    {
                        _handel = eOrder.Non;
                    }
                }
                break;

            case eOrder.Stop:
                {
                    _handel = eOrder.Non;
                }
                break;

            case eOrder.Attack:
                {

                }
                break;

            case eOrder.Patrol:
                {
                    float dis_MoveTarget = Vector3.Distance(_v3MoveTarget, pos);
                    float dis_OriginalPosition = Vector3.Distance(_v3OriginalPosition, pos);

                    if (Vector3.Distance(_v3MoveTarget, _v3OriginalPosition) <= this.transform.localScale.x)
                    {
                        return;
                    }

                    if (dis_MoveTarget <= this.transform.localScale.x)
                    {
                        _agnet.SetDestination(_v3OriginalPosition);
                    }

                    if (dis_OriginalPosition <= this.transform.localScale.x)
                    {
                        _agnet.SetDestination(_v3MoveTarget);
                    }
                }
                break;

            case eOrder.Hold:
                {

                }
                break;

            case eOrder.Custom_1:

                break;

            case eOrder.Custom_2:

                break;

            case eOrder.Custom_3:

                break;

            case eOrder.Custom_4:

                break;
        }
    }

    private void DetectEnemy(Vector3 pos)
    {

    }

    public void Move(Vector3 targetPos)
    {
        _handel = eOrder.Move;
        _v3MoveTarget = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);

        _agnet.SetDestination(_v3MoveTarget);
    }

    public void Stop()
    {
        _handel = eOrder.Stop;

        _agnet.SetDestination(this.transform.position);
    }

    public void Attack(Vector3 targetPos)
    {
        _handel = eOrder.Attack;
        _v3MoveTarget = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);

        _agnet.SetDestination(_v3MoveTarget);
    }

    public void Attack(GameObject tamplate)
    {
        _handel = eOrder.Attack;
        _objectTamplateTarget = tamplate;
        _v3MoveTarget = new Vector3(tamplate.transform.position.x, this.transform.position.y, tamplate.transform.position.z);

        _agnet.SetDestination(_v3MoveTarget);
    }

    public void Patrol(Vector3 targetPos)
    {
        _handel = eOrder.Patrol;
        _v3OriginalPosition = this.gameObject.transform.position;
        _v3MoveTarget = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);

        _agnet.SetDestination(_v3MoveTarget);
    }

    public void Hold()
    {
        _handel = eOrder.Hold;

        _agnet.SetDestination(this.transform.position);
    }

    public void Custom_1()
    {
        _handel = eOrder.Custom_1;
    }

    public void Custom_2()
    {
        _handel = eOrder.Custom_2;
    }

    public void Custom_3()
    {
        _handel = eOrder.Custom_3;
    }

    public void Custom_4()
    {
        _handel = eOrder.Custom_4;
    }
}
