using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private float respwawnTime;
    [SerializeField] private SpriteRenderer child;
    [SerializeField] private Animator animator;
    private void Start()
    {
        child = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().AddEndurence(); // ajoute Endurence (initialisé à 20 de BASE)

            StartCoroutine(Collect(respwawnTime)); // temps de reaparition du bonus
        }
    }
    /// <summary>
    /// Ce qui se passe après collection du Bonus
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator Collect(float t)
    {
        animator.SetTrigger("Collect");

        child.enabled = false;
        yield return new WaitForSeconds(t);
        child.enabled = true;
    }
}
