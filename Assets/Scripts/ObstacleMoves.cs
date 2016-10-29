using UnityEngine;
using System.Collections;

public class ObstacleMoves : MonoBehaviour{
    public static bool active;
    float gravity = -18f;
    float vel = 0;
    RaycastHit hit;
    Ray r;
    int counter = 0;
    Vector3 moves = new Vector3(0, 0, 0);
    GameObject scoreboard;
    int ballbounce;
    float localobsspeed;
    public static bool godmode;

    // Use this for initialization
    void Start () {
        active = true;
        if (gameObject.layer == 12) 
        {
            ballbounce = new System.Random().Next(5, 15);
        }

        //IS GODMODE ACTIVE
        if (godmode)
        {
            localobsspeed = new System.Random().Next(5, 15);
        }
        else
        {
            localobsspeed = Background.obsspeed;
        }
        moves.x = -1 * localobsspeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            //GRAVITY
            r = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(r, out hit, 0.5f))
            {
                moves.y = 0.5f - hit.distance;
                moves.x = 0;
                transform.Translate(moves);
                moves.x = -1 * localobsspeed;
                moves.y = 0;
                if(hit.transform == GameObject.Find("/Player").transform)
                {
                    Background.extras++;
                    Background.scoreboard.GetComponent<TextMesh>().text = "Score : " + Background.score.ToString() + "\nExtras : " + Background.extras.ToString();
                    Background.Extrapoint.SetActive(true);
                    ExtraPointAnime.timer = 2;
                }
            }
            else
            {
                moves.y += (gravity * Time.deltaTime);
            }

            //OBSTACLE 2
            if (gameObject.layer == 12 && Physics.Raycast(transform.position, Vector3.down, 0.5f))
            {
                moves.y = ballbounce;
            }

            transform.Translate(moves * Time.deltaTime);
            //KILL OBSTACLE
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 0.5f) && hit.transform.tag == "Finish")
            {
                //INC SCORE
                Background.score++;
                Background.scoreboard.GetComponent<TextMesh>().text = "Score : " + Background.score.ToString() + "\nExtras : " + Background.extras.ToString();

                if (Background.score == 30)
                {
                    //START GODMODE
                    godmode = true;
                    Background.time = 0.7f;
                    Background.GMMenu.SetActive(true);
                    Background.active = false;
                    ObstacleMoves.active = false;
                }

                //CHECK SCORE (SPEED AND TIME MANIPULATION)
                if (Background.score % Background.n == 0 && !godmode)
                {
                    if (Background.time > 1)
                    {
                        Background.time -= 0.5f;
                    }
                    else if (Background.obsspeed < 10)
                    {
                        Background.obsspeed += 1f;
                    }
                    Background.n += 3;
                }

                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.y > -5)
            {
                Destroy(gameObject);
            }
        }
    }
}
