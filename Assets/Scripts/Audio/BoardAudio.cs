using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAudio : MonoBehaviour
{
    [SerializeField] AudioSource pop;
    [SerializeField] AudioSource crumble;
    public void PlayCrumble() {
        if(!crumble.isPlaying) {
            crumble.Play();
        }
    }
    public void PlayPop() {
        if(!pop.isPlaying) {
            pop.Play();
        }
    }
}
