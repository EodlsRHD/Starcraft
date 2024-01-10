using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitInformation : MonoBehaviour
{
    private class ObjectInfo
    {
        public IObserver data;
        public Image multipleImage;

        public void UpdateImage()
        {
            if(multipleImage.gameObject.TryGetComponent(out SpriteRenderer renderer) == true)
            {

            }
            else
            {
                multipleImage.gameObject.AddComponent<SpriteRenderer>();
            }
        }
    }

    [SerializeField]
    private Image _imageObjectTamplate = null;

    [SerializeField]
    private Transform _trTamplateParant = null;

    [SerializeField]
    private GameObject _objMultipleInformation = null;

    [Space(10)]

    [SerializeField]
    private Image _imageHP = null;

    [SerializeField]
    private TMP_Text _textName = null;

    [SerializeField]
    private TMP_Text _textHP = null;

    [SerializeField]
    private TMP_Text _textKillCount = null;

    [SerializeField]
    private Image _imageAttack = null;

    [SerializeField]
    private TMP_Text _textAttackUpgradeCount = null;

    [SerializeField]
    private Image _imageDefence = null;

    [SerializeField]
    private TMP_Text _textDefenceUpgradeCount = null;

    [SerializeField]
    private Image _imageShild = null;

    [SerializeField]
    private TMP_Text _textShildUpgradeCount = null;

    private List<ObjectInfo> _datas = new List<ObjectInfo>();

    public void Initialize()
    {
        _objMultipleInformation.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Open(List<ObjectTamplate> datas, Action<ObjectData> onUpdateCallback)
    {
        Close();

        _datas.Clear();

        foreach(var d in datas)
        {
            ObjectInfo info = new ObjectInfo();
            info.data = d;

            _datas.Add(info);
        }

        if(_datas.Count == 1)
        {
            ObjectData _data = _datas[0].data.GetData();

            _textName.text = _data.name;
            _textHP.text = _data.metaData.currentHp + " / " + _data.maxHp;
            _textKillCount.text = "kill : " + _data.metaData.killCount;
            _textAttackUpgradeCount.text = _data.metaData.upgradeAttack.ToString();
            _textDefenceUpgradeCount.text = _data.metaData.upgradedDefence.ToString();
            _textShildUpgradeCount.text = _data.metaData.upgradeShild.ToString();

            if (_data.metaData.HpKey == 0)
            {
                GameManager.instance.toolManager.RequestPool(ePoolType.Image, "Hp_" + _data.name, (result) =>
                {
                    _data.metaData.HpKey = result.key;
                    _imageHP.sprite = result.GetSprite();
                });
            }
            else
            {
                GameManager.instance.toolManager.RequestPool(ePoolType.Image, _data.metaData.HpKey, (result) =>
                {
                    _imageHP.sprite = result.GetSprite();
                });
            }

            if (_data.hasAttack)
            {
                if (_data.metaData.attackKey == 0)
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, "Attack_" + _data.raceType + "_" + _data.unitType + "_" + _data.isAir + "_" + _data.farAndNeer, (result) =>
                    {
                        _data.metaData.attackKey = result.key;
                        _imageAttack.sprite = result.GetSprite();

                        _textKillCount.gameObject.SetActive(true);
                        _imageAttack.gameObject.SetActive(true);
                    });
                }
                else
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, _data.metaData.attackKey, (result) =>
                    {
                        _imageAttack.sprite = result.GetSprite();

                        _imageAttack.gameObject.SetActive(true);
                    });
                }
            }
            else
            {
                _textKillCount.gameObject.SetActive(false);
                _imageAttack.gameObject.SetActive(false);
            }

            if ((eObject)_data.objType == eObject.Unit)
            {
                if (_data.metaData.defenceKey == 0)
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, "Defence_" + _data.raceType + "_" + _data.unitType + "_" + _data.isAir + "_" + _data.farAndNeer, (result) =>
                    {
                        _data.metaData.defenceKey = result.key;
                        _imageDefence.sprite = result.GetSprite();

                        _imageDefence.gameObject.SetActive(true);
                    });
                }
                else
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, _data.metaData.defenceKey, (result) =>
                    {
                        _imageDefence.sprite = result.GetSprite();

                        _imageDefence.gameObject.SetActive(true);
                    });
                }
            }
            else
            {
                _imageDefence.gameObject.SetActive(false);
            }

            if (_data.hasShild == true)
            {
                if (_data.metaData.shildKey == 0)
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, "Shild_" + _data.raceType + "_" + _data.unitType + "_" + _data.isAir + "_" + _data.farAndNeer, (result) =>
                    {
                        _data.metaData.shildKey = result.key;
                        _imageShild.sprite = result.GetSprite();

                        _imageShild.gameObject.SetActive(true);
                        _textShildUpgradeCount.gameObject.SetActive(true);
                    });
                }
                else
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, _data.metaData.shildKey, (result) =>
                    {
                        _imageShild.sprite = result.GetSprite();

                        _imageShild.gameObject.SetActive(true);
                        _textShildUpgradeCount.gameObject.SetActive(true);
                    });
                }
            }
            else
            {
                _imageShild.gameObject.SetActive(false);
                _textShildUpgradeCount.gameObject.SetActive(false);
            }

            onUpdateCallback?.Invoke(_data);

            this.gameObject.SetActive(true);
        }
        else if(_datas.Count > 1)
        {
            for (int i = 0; i < 16; i++)
            {
                var image = Instantiate(_imageObjectTamplate, _trTamplateParant);

                if (i > _datas.Count - 1)
                {
                    break;
                }

                if (_datas[i].data.GetData().metaData.HpKey == 0)
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, "Hp_" + _datas[i].data.GetData().name, (result) =>
                    {
                        _datas[i].data.GetData().metaData.HpKey = result.key;
                        image.sprite = result.GetSprite();
                    }); ;
                }
                else
                {
                    GameManager.instance.toolManager.RequestPool(ePoolType.Image, _datas[i].data.GetData().metaData.HpKey, (result) =>
                    {
                        _imageHP.sprite = result.GetSprite();
                    });
                }

                _datas[i].multipleImage = image;
                onUpdateCallback?.Invoke(_datas[i].data.GetData());
            }

            _objMultipleInformation.SetActive(true);
        }
    }

    public void UpdateInfo(IObserver data)
    {
        for (int i = _datas.Count - 1; i >= 0; i--)
        {
            if((_datas[i].data.GetData().key == data.GetData().key) == false)
            {
                continue;
            }

            if(data.GetData().metaData.currentHp == 0)
            {
                Destroy(_datas[i].multipleImage.gameObject);
                _datas.Remove(_datas[i]);

                break;
            }

            _datas[i].data = data;

            if (_datas.Count == 1)
            {
                ObjectData _data = _datas[i].data.GetData();

                _textHP.text = _data.metaData.currentHp + " / " + _data.maxHp;
                _textKillCount.text = "kill : " + _data.metaData.killCount;
                _textAttackUpgradeCount.text = _data.metaData.upgradeAttack.ToString();
                _textDefenceUpgradeCount.text = _data.metaData.upgradedDefence.ToString();
                _textShildUpgradeCount.text = _data.metaData.upgradeShild.ToString();
            }

            break;
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        if(_datas.Count == 1)
        {
            if(_imageHP.sprite != null)
            {
                PutBack(ePoolType.Image, _datas[0].data.GetData().metaData.HpKey, _imageHP.sprite);
                _imageHP.sprite = null;
            }

            if (_imageAttack.sprite != null)
            {
                PutBack(ePoolType.Image, _datas[0].data.GetData().metaData.attackKey, _imageAttack.sprite);
                _imageAttack.sprite = null;
            }

            if (_imageDefence.sprite != null)
            {
                PutBack(ePoolType.Image, _datas[0].data.GetData().metaData.defenceKey, _imageDefence.sprite);
                _imageDefence.sprite = null;
            }

            if (_imageShild.sprite != null)
            {
                PutBack(ePoolType.Image, _datas[0].data.GetData().metaData.shildKey, _imageShild.sprite);
                _imageShild.sprite = null;
            }

            _textName.text = string.Empty;
            _textHP.text = string.Empty;
            _textKillCount.text = string.Empty;
            _textAttackUpgradeCount.text = string.Empty;
            _textDefenceUpgradeCount.text = string.Empty;
            _textShildUpgradeCount.text = string.Empty;
        }
        else if(_datas.Count > 1)
        {
            for (int i = 0; i < _datas.Count; i++)
            {
                if(_datas[i].multipleImage.sprite != null)
                {
                    PutBack(ePoolType.Image, _datas[i].data.GetData().metaData.HpKey, _datas[i].multipleImage.sprite);
                }
            }
        }

        _datas.Clear();
    }

    private void PutBack(ePoolType type, int key, Sprite s)
    {
        PutBackPool p = new PutBackPool();
        p.SetData(type, key, s);
        GameManager.instance.toolManager.PutBackPool(p);
    }
}