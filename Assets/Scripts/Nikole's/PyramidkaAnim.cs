using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PyramidkaAnim : MonoBehaviour
{
    public Animator anim;
    public GameObject pyramidka;
    public GameObject shadow;

    private bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Artifact" && pyramidka != null && done == false && anim != null)
        {
            Debug.Log("VFX should work!");
            anim.SetTrigger("PickUp");
            pyramidka.SetActive(true);
			shadow.SetActive(false);
            done = true;
        }
    }

    public void ShakeCamera1()
    {
		CameraShaking.Shake(0.3f, 0.1f);
	}

	public void ShakeCamera2()
	{
		CameraShaking.Shake(0.5f, 0.25f);
	}

	public void ShakeCamera3()
	{
		CameraShaking.Shake(0.7f, 0.1f);
	}
}
