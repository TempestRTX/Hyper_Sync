using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Material DisolveMat;
    [SerializeField] private float animationWaitperiod;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
        StartCoroutine(WaitforDisolveAnimation());
        }
    }

    private IEnumerator WaitforDisolveAnimation()
    {
        gameObject.GetComponent<Renderer>().material = DisolveMat;
        yield return new WaitForSeconds(animationWaitperiod);
        gameObject.SetActive(false);
        
    }
    
}
