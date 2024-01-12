using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int bananas = 0;

    [SerializeField] private Text bananasText;

    [SerializeField] private AudioSource collectionsSoundEffect;
      
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Banana") )
        {
            collectionsSoundEffect.Play();
            Destroy(collision.gameObject);
            bananas++;
            bananasText.text = "Bananas: " + bananas;
        }
    }
}
