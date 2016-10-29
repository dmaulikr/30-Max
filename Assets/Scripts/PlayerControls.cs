using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerControls : MonoBehaviour {
    float gravity = -18f;
    float vel = 0;
    RaycastHit hit;
    Ray r;
    int counter = 0;
    Vector3 moves = new Vector3(0,0,0);
    bool flag,onceflag;
    GameObject gover;

    // Use this for initialization
    void Start ()
    {
        flag = false;
        onceflag = true;
        gover = GameObject.FindGameObjectWithTag("GameOver");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //ADD GRAVITY PER SEC
        moves.y += (gravity * Time.deltaTime);

        //MOVES
        //UP
        if (Input.touchCount > 0 && (Physics.Raycast(new Ray(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector3.down), 0.51f) || Physics.Raycast(new Ray(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector3.down), 0.51f)))
        {
            moves.y = 10;
        }
        if (moves.y > 0 && (Physics.Raycast(new Ray(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector3.up), 0.5f) || Physics.Raycast(new Ray(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector3.up), 0.5f)))
        {
            moves.y = 0;
        }

        //LEFT RIGHT
        if ((Input.acceleration.x < -0.1) && !Physics.Raycast(transform.position, Vector3.left, 0.5f))
        {
            moves.x = -5f;
        }
        else if ((Input.acceleration.x > 0.1) && !Physics.Raycast(transform.position, Vector3.right, 0.5f))
        {
            moves.x = 5f;
        }
        
        //------------CHECK DEAD
        //LEFT
        for(int i = -4; i <= 4; i++)
        {
            if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y + (i * 0.1f), transform.position.z), Vector3.left, out hit, 0.5f) && hit.transform.tag == "StartUp")
            {
                //DEAD
                if ((hit.transform.position.y >= transform.position.y && hit.transform.position.y <= (transform.position.y + 0.5f)) || (hit.transform.position.y <= transform.position.y && hit.transform.position.y >= (transform.position.y - 0.5f)) && Mathf.Abs(hit.transform.position.x - transform.position.x) >= Mathf.Abs(hit.transform.position.y - transform.position.y))
                {
                    flag = true;
                }
            }
        }
        //RIGHT
        for (int i = -4; i <= 4; i++)
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + (i * 0.1f), transform.position.z), Vector3.right, out hit, 0.5f) && hit.transform.tag == "StartUp")
            {
                //DEAD
                if ((hit.transform.position.y >= transform.position.y && hit.transform.position.y <= (transform.position.y + 0.5f)) || (hit.transform.position.y <= transform.position.y && hit.transform.position.y >= (transform.position.y - 0.5f)) && Mathf.Abs(hit.transform.position.x - transform.position.x) >= Mathf.Abs(hit.transform.position.y - transform.position.y))
                {
                    flag = true;
                }
            }
        }
        //DOWN
        for (int i = -9; i <= 9; i++)
        {
            if (Physics.Raycast(new Vector3(transform.position.x + (i * 0.1f), transform.position.y, transform.position.z), Vector3.down, out hit, 0.5f) && hit.transform.tag == "StartUp")
            {
                //DEAD
                if ((hit.transform.position.x >= transform.position.x && hit.transform.position.y <= transform.position.y) || (hit.transform.position.x <= transform.position.x && hit.transform.position.y <= transform.position.y) && Mathf.Abs(hit.transform.position.x - transform.position.x) <= Mathf.Abs(hit.transform.position.y - transform.position.y))
                {
                    flag = true;
                }
            }
        }

        if (flag)
        {
            if (gover.transform.localPosition.y > 150)
            {
                gover.transform.Translate(new Vector3(0, -5, 0) * Time.deltaTime);
            }
            if (onceflag)
            {
                Background.active = false;
                ObstacleMoves.active = false;

                //CHECK HIGHSCORE AND SAVE
                bool highs = true;
                if (File.Exists(Application.persistentDataPath + "/30Max"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs;
                    fs = File.Open(Application.persistentDataPath + "/30Max", FileMode.Open);
                    SaveData sd = (SaveData)bf.Deserialize(fs);
                    if(!((Background.score + Background.extras) > sd.playerscore))
                    {
                        highs = false;
                    }
                    fs.Close();
                }
                if (highs)
                {
                    Background.HighScore.SetActive(true);
                }
                else
                {
                    Background.PlayAgain.SetActive(true);
                }
            }
            onceflag = false;
        }

        //------------TRANSLATE
        if(moves.y < 0  && Physics.Raycast(new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.up * Mathf.Sign(moves.y)), out hit, 0.5f + Mathf.Abs(moves.y * Time.deltaTime)))
        {
            moves.y = -1 * (hit.distance - 0.5f);
            transform.Translate(new Vector3(0,moves.y,0));
            moves.y = 0;
        }
        transform.Translate(moves * Time.deltaTime);
        moves.x = 0;
    }
}

