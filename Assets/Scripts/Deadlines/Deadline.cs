using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deadline : MonoBehaviour {
    [SerializeField] TMP_Text text;
    [SerializeField] Slider slider;
    [SerializeField] GameObject UIParent;
    [SerializeField] int allResourceGain;
    [SerializeField] AudioSource success;
    [SerializeField] AudioSource fail;
    DeadlineData data;
    List<Piece> pieces;
    public float maxDuration;
    public float timer;
    public float timePentalty;
    bool exiting;
    public float exitDuration;
    float exitTimer;
    [SerializeField] AnimationCurve exitCurve;

    private void Start() {
        exitTimer = exitDuration;
    }

    private void Update() {
        if(exiting) {
            exitTimer -= Time.deltaTime;
            float value = exitCurve.Evaluate(exitTimer / exitDuration);

            Image[] images = UIParent.GetComponentsInChildren<Image>();
            foreach(Image image in images) {
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0, image.color.a, value));
            }
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0, text.color.a, value));

            if (exitTimer <= 0) {
                Destroy(gameObject);
            }
        }
        else {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                Fail();
            }

            slider.value = 1 - (timer / maxDuration);
        }
    }

    public void InitalizeDeadline(DeadlineData data, List<Piece> pieces, float maxDuration) {
        this.data = data;
        this.pieces = pieces;
        this.maxDuration = maxDuration;
        timer = maxDuration;

        text.color = data.color;
        text.text = data.deadlineName;
    }

    public void EndDeadline() {
        foreach (Piece piece in pieces) {
            piece.DestroyPiece();
        }
        exiting = true;
    }

    public void TryLockInPiece(Piece piece) {
        if(pieces.Contains(piece)) {
            pieces.Remove(piece);
        }
        if(pieces.Count <= 0) {
            Success();
        }
    }

    public void Success() {
        ResourceManager resourceManager = FindObjectOfType<ResourceManager>();

        resourceManager.AddMental(allResourceGain);
        resourceManager.AddPhysical(allResourceGain);
        resourceManager.AddFinancial(allResourceGain);
        success.Play();

        EndDeadline();
    }

    public void Fail() {
        FindAnyObjectByType<WinConditionTimer>().LowerTimer(timePentalty);
        fail.Play();
        EndDeadline();
    }
}
