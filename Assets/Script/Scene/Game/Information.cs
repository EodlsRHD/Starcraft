using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public interface ISubject
{
    public void Add(eObject type, IObserver data);
    public void Remove(eObject type, IObserver data);
    public void Notify(eObject type);
}

public interface IObserver
{
    public void UpdateData(ObjectData data);
    public eBrood BroodType();
    public eUnitType UnitType();
    public eUnitSizeType UnitSizeType();
    public void SetUpgrade(eUpgrade type);
    public ObjectData GetData();
}

public enum eObject
{
    Non = -1,
    Building,
    Unit
}

public enum eBrood
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

[System.Serializable]
public class ObjectData
{
    public string _id = string.Empty;
    public int key = 0;

    public eObject objType = eObject.Non;
    public eBrood broodType = eBrood.Non;
    public eUnitType unitType = eUnitType.Non;
    public eUnitSizeType unitSizeType = eUnitSizeType.Non;
    public eUnitAttackType unitAttackType = eUnitAttackType.Non;
    public eFarAndNeer farAndNeer = eFarAndNeer.Non;

    public string name = string.Empty;
    public int productionCode = 0;

    public bool isAir = false;
    public int maxHp = 0;

    public bool hasShild = false;
    public int amxShild = 0;

    public bool hasEnergy = false;
    public int maxEnergy = 0;

    public bool hasAttack = false;
    public bool hasAirAttack = false;
    public int attack = 0;
    public float attackRate = 0f;
    public float attackRange = 0f;

    public int defence = 0;
    public float moveSpeed = 0f;

    public ObjectCustom custom = null;
    public ObjectMatadata metaData = null;
}

[System.Serializable]
public class ObjectMatadata
{
    public int killCount = 0;

    public int HpKey = 0;
    public int attackKey = 0;
    public int defenceKey = 0;
    public int shildKey = 0;

    public int currentHp = 0;
    public int currentShild = 0;
    public int currentEnergy = 0;

    public int upgradeAttack = 0;
    public int upgradedDefence = 0;
    public int upgradeShild = 0;

    public int upgradeAttackCoefficient = 1;
    public int upgradeDefenceCoefficient = 1;
    public int upgradeShildCoefficient = 1;

    public bool isProduction = false;
    public string[] productionUnitIDs = null;
}

[System.Serializable]
public class ObjectCustom
{
    public bool hasCustom_1 = false;
    public string custom_1_key = string.Empty;

    public bool hasCustom_2 = false;
    public string custom_2_key = string.Empty;

    public bool hasCustom_3 = false;
    public string custom_3_key = string.Empty;

    public bool hasCustom_4 = false;
    public string custom_4_key = string.Empty;
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
