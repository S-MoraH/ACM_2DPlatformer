using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ItemCollector : MonoBehaviour
{
    private int cherry = 0;

    [SerializeField] private Text cherryText;
    [SerializeField] private AudioSource collectionSoundEffect;

    public void Awake()
    {

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            this.cherry = 6;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            this.cherry = 2;
        } else
        {
            this.cherry = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Collect_Cherry"))
        {
            Destroy(collision.gameObject);
            collectionSoundEffect.Play();
            ++cherry;
            UpdateCherryText();
        }
    }

    public void UpdateCherryText()
    {
        cherryText.text = "Cherry: " + this.GetCherry();
    }


    public void SetCherry(int cherries)
    {
        this.cherry = cherries;
    }

    public int GetCherry()
    {
        return this.cherry;
    }


}

