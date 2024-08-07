using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float brainrot = 0f;
    public float maxBrainrot = 100f;
    public int score;
    private GameObject arm;
    public BaseWeapon weapon;
    public GameObject gameOverScreen;
    public GameObject hud;
    public PlayerControls playerControls;
    public Interactable interactableObject = null;

    //When the player takes damage, remove from health and reset regeneration delay
    public void TakeDamage(float damage)
    {
        brainrot += damage;
        brainrot = Mathf.Clamp(brainrot, 0, maxBrainrot + 1);
        Debug.Log($"{maxBrainrot}, {brainrot}");
        if (brainrot < maxBrainrot)
        {
            CancelInvoke();
            Invoke("FixBrainrot", 2f);
        }
        else
        {
            Debug.Log("Died");
            Death();
        }
    }

    //If player interacts with interactablle object, run the object's interact function
    public void Interact()
    {
        if (interactableObject != null)
        {
            interactableObject.Interact();
        }
    }

    //Regenerate health up until max
    private void FixBrainrot()
    {
        if (brainrot >= maxBrainrot)
        {
            brainrot -= 1;
            Invoke("FixBrainrot", 0.05f);
        }
    }

    //Upon death, restart the game after 3 seconds
    private void Death()
    {
        hud.GetComponent<uiInteractions>().GameEnded();
        playerControls.Movement.Disable();
        weapon.gameObject.SetActive(false);
        //hud.SetActive(false);
        //gameOverScreen.SetActive(true);
        Invoke("Restart", 4f);
    }

    //Restart by reloading the scene
    public void Restart()
    {
        GetComponent<PlayerCamera>().DisableCameraStuff();
        GetComponent<PlayerCamera>().enableCamera = false;
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
