using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public interface ISubject
{
    public void Notify(eObject type);
}

public interface IObserver
{
    public void UpdateData(ObjectData data);
    public eRace BroodType();
    public eUnitType UnitType();
    public eUnitSizeType UnitSizeType();
    public void SetUpgrade(eUpgrade type);
    public ObjectData GetData();
}

public enum eObject
{
    Non = -1,
    Building,
    Unit,
    Resources
}

public enum eRace
{
    Non = -1,
    Terran,
    Protoss,
    Zerg
}

public enum eUnitType
{
    Non = -1,
    Biological,
    Mechanical
}

public enum eUnitSizeType
{
    Non = -1,
    Large,
    Middle,
    Small
}

public enum eUnitAttackType
{
    Non = -1,
    Common,
    Concussive,
    Explosive
}

public enum eFarAndNeer
{
    Non = -1,
    Far,
    Neer
}

public class Information : MonoBehaviour
{
    [SerializeField]
    private UnitInformation _unitInformation = null;

    [SerializeField]
    private Image _imageProtrait = null;

    public void Initialize()
    {
        _unitInformation.Initialize();

        this.gameObject.SetActive(true);
    }

    public void SetInformation(List<ObjectTamplate> datas, Action<ObjectData> onSetKeyCallback)
    {
        _unitInformation.Open(datas, onSetKeyCallback);
    }

    public void SetProtrait(Texture texture)
    {
        if(texture == null)
        {
            return;
        }

        _imageProtrait.sprite = GameManager.instance.toolManager.ConvertTextureToSprite(texture);
    }

    public void UpdateInfo(IObserver data)
    {
        _unitInformation.UpdateInfo(data);
    }
}
