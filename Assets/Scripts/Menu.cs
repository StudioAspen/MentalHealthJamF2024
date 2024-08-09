using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private FadeCanvas fadeCanvas;

    [SerializeField] private Button playButton;

    private bool coroutineStarted;

    private void Awake()
    {
    }
}
