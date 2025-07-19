using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField] private ParticleSystem ExplosionFX;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            StartCoroutine(WaitforExplosionAnimation());
        }
    }
    
    private IEnumerator WaitforExplosionAnimation()
    {
        ExplosionFX.Play();
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        
    }
}
