using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    public AudioClip ad;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void PlayTransitionBGM(float time)
    {

        StartCoroutine(PlayTransitionBGM_Time(time));
    }

    IEnumerator PlayTransitionBGM_Time(float time)
    {
        yield return new WaitForSeconds(time);
        
        gameObject.GetComponent<AudioSource>().PlayOneShot(ad, 1.0f);
    }
    public void StopBGM()
    {
        gameObject.GetComponent<AudioSource>().Stop();
    }
}
