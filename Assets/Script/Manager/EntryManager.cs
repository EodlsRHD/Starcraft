using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    [SerializeField]
    private Login _login = null;

    void Start()
    {
        _login.Initilaize();
    }
}
