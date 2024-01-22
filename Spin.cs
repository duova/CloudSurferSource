using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float value;
    
    void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, value * Time.deltaTime, 0));
    }
}
