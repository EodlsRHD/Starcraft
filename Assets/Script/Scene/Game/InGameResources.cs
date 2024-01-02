using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameResources : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textMineral = null;

    [SerializeField]
    private TMP_Text _textGas = null;

    [SerializeField]
    private TMP_Text _textPopulation = null;

    public void Initialize()
    {
        _textMineral.text = "0";
        _textGas.text = "0";
        _textPopulation.text = "0";

        this.gameObject.SetActive(true);
    }

    public void SetResources(int mineral, int gas, int population, int maxPopulation)
    {
        _textMineral.text = mineral.ToString();
        _textMineral.text = gas.ToString();
        _textMineral.text = population + " / " + maxPopulation;
    }
}
