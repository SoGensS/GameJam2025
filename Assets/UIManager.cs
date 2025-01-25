using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("DeathScreen")]
    public GameObject DeathScreen;
    public TMP_Text textDeathReason;
    public Image image;
    public Sprite[] imageMeme;

    public Animation anim;

    public void Death(GameObject DeathReason)
    {
        anim.Play("DieOpen");
        image.sprite = imageMeme[Random.Range(0, imageMeme.Length)];
        textDeathReason.text = DeathReason.name;
    }
    public void ReStart()
    {
        SceneManager.LoadScene(1);
    }
}
