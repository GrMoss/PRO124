using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        LoadComplete();
    }

    public void StartLoading()
    {
        animator.SetTrigger("Loading");
    }

    public void LoadComplete()
    {
        animator.SetTrigger("LoadingBackward");
    }
}
