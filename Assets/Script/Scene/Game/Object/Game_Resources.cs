using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Resources : MonoBehaviour
{
    [SerializeField] // test
    private eResourceType _type = eResourceType.Non;

    [SerializeField] // test
    private int _quantity = 0;

    public void Initialize(Resource info)
    {
        _type = info.type;
        _quantity = info.quantity;
    }
}
