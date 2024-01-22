using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveButton : MonoBehaviour
{
    [SerializeField] private GameObject disable, enable;

    public void Press()
    {
        disable.SetActive(false);
        enable.SetActive(true);
    }
}
