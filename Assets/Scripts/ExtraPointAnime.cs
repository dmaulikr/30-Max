using UnityEngine;
using System.Collections;

public class ExtraPointAnime : MonoBehaviour {
    public static float timer;

	// Use this for initialization
	void Start () {
        timer = 2;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Background.Extrapoint.SetActive(false);
            timer = 2;
        }
	}
}
