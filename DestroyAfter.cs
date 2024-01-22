using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float existTime;

    // Update is called once per frame
    void Update()
    {
        existTime -= Time.deltaTime;
        if (existTime < 0f) Destroy(gameObject);
    }
}
