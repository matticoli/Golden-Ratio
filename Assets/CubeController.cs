using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour {


    private const float phi = 1.62f;
    private bool varyWidth;
    public float deltaScale;

    private Vector2 screenBounds, screenOrigin;
    private float score = 0f;
    private int lives = 5;

    bool stop = true;

    void Start() {

        varyWidth = false;
        deltaScale = 0.01f;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * 5 / 6, Screen.height * 5 / 6));
        screenOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 6, Screen.height / 6));
        gameObject.transform.localScale= new Vector3(1f, 1f, 1f);
        lives = 5;
        score = 0f;
        updateScoreText();
        updateLives();
        setRatio(1f);
    }

    void Update() {
        if(stop)
        {
            if (Input.anyKeyDown || Input.touchCount > 0)
            {
                Start();
                stop = false;
            }
            return;
        }

        this.IncrementScale();

        if (Input.anyKeyDown || Input.touchCount > 0) // TODO: Scoring
        {
            varyWidth = !varyWidth;
            if (deltaScale < phi / 10)
            {
                deltaScale *= 1.1f;
            }
            if (isGolden())
            {
                score += phi;
                updateScoreText();
                lives++;
            } else if(!stop)
            {
                lives--;
            }
            updateLives();
            if (lives <= 0)
            {
                stop = true;
            }
        }

        if (isOffScreen()) // If cube goes off-screen
        {
            gameObject.transform.localScale /= 1.6f;

        }
    }

    bool isGolden()
    {
        float width = gameObject.transform.localScale.x,
              height = gameObject.transform.localScale.y;
        setRatio(varyWidth ? Mathf.Abs((height / width)) : Mathf.Abs((width / height)));
        return (varyWidth ? Mathf.Abs((height / width) - phi) <= 0.1 : Mathf.Abs((width / height) - phi) <= 0.1);
    }

    bool isOffScreen()
    {
        return (gameObject.transform.position.y + (gameObject.transform.localScale.y / 2) > screenBounds.y) ||
               (gameObject.transform.position.y - (gameObject.transform.localScale.y / 2) < screenOrigin.y) ||
               (gameObject.transform.position.x + (gameObject.transform.localScale.x / 2) > screenBounds.x) ||
               (gameObject.transform.position.x - (gameObject.transform.localScale.x / 2) < screenOrigin.x);
    }

    void IncrementScale()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(currentScale.x + (varyWidth ? deltaScale : 0), currentScale.y + (varyWidth ? 0 : deltaScale), 1f);
    }

    void setRatio(float f)
    {
        GameObject.Find("Num").GetComponent<Text>().text = (f.ToString());
    }

    void updateScoreText()
    {
        GameObject.Find("Score").GetComponent<Text>().text = "Score: " + (score.ToString());
    }

    void updateLives()
    {
        GameObject.Find("Lives").GetComponent<Text>().text = new string('φ', lives);

    }
}
