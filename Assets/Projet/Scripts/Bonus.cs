using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private float respwawnTime;
    [SerializeField] private SpriteRenderer child;
    private void Start()
    {
        child = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().AddEndurence(); // ajoute Endurence (initialis� � 20 de BASE)
            StartCoroutine(Collect(respwawnTime)); // temps de racparition du bonus
        }
    }
    /// <summary>
    /// Ce qui se passe apr�s collection du Bonus
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator Collect(float t)
    {
        child.enabled = false;
        yield return new WaitForSeconds(t);
        child.enabled = true;
    }
}
