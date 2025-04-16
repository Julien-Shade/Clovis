using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private float respwawnTime;
    [SerializeField] private SpriteRenderer child;
    [SerializeField] private Animator animator;
    [SerializeField] private bool canPick;
    [SerializeField] private float amount;
    private void Start()
    {
        amount = 20;
        child = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        canPick = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControl playerControl = collision.GetComponent<PlayerControl>();

            if (canPick && playerControl.Endurence != playerControl.MaxEndurence)
            {
                playerControl.AddEndurence(amount); // ajoute Endurence (initialisé à 20 de BASE)
                StartCoroutine(Collect(respwawnTime)); // temps de reaparition du bonus
            }
        }
    }
    /// <summary>
    /// Ce qui se passe après collection du Bonus
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator Collect(float t)
    {
        canPick = false;
        animator.SetTrigger("Collect");
        yield return new WaitForSeconds(2);
        child.enabled = false;
        yield return new WaitForSeconds(t);
        animator.SetTrigger("Spawn");
        child.enabled = true;
        canPick = true;
    }
}
