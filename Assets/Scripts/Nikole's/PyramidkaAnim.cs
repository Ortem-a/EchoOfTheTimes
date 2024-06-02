using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PyramidkaAnim : MonoBehaviour
{
    public Animator anim;
    public VisualEffect pyramidka;

    private bool done = false;

    void Update()
    {
        if(anim != null)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Artifact" && pyramidka != null && done == false && anim != null)
        {
            Debug.Log("VFX should work!");
            anim.SetTrigger("PickUp");
            pyramidka.Play();
            done = true;
        }
    }
}
