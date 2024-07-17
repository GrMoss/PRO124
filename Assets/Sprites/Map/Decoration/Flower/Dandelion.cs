using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dandelion : MonoBehaviour
{
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        while (true)
        {
            float i = Random.Range(2f, 10f);
            yield return new WaitForSeconds(i);
            ani.SetTrigger("Move");
        }
    }
}
