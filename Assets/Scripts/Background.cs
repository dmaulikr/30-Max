using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using admob;
using System.Collections.Generic;

public class Background : MonoBehaviour{
    public static GameObject GMMenu;
    public static GameObject Extrapoint;
    public static GameObject MainMenu;
    public static GameObject HighScore;
    public static GameObject PlayAgain;
    public static GameObject Fire;
    public static GameObject Exit;

    public static int score;
    public static float obsspeed;
    GameObject[] obj;
    public GameObject spawn;
    public Material godmaterial;
    public static float time;
    public static GameObject scoreboard;
    public static int n;
    public static bool active;
    public static int extras;
    float t;
    float flash;
    bool flashing;
    public static int objcount;

    void Awake()
    {
        System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    // Use this for initialization
    void Start ()
    {
        Exit = GameObject.Find("/Canvas/Exit");
        Exit.SetActive(false);
        GMMenu = GameObject.Find("/Canvas/GodModeActivation");
        GMMenu.SetActive(false);
        Extrapoint = GameObject.Find("/Canvas/ExtraPoint");
        Extrapoint.SetActive(false);
        HighScore = GameObject.Find("/Canvas/HighScore");
        HighScore.SetActive(false);
        PlayAgain = GameObject.Find("/Canvas/PlayAgain");
        PlayAgain.SetActive(false);
        Fire = GameObject.Find("/Player/Fire");
        Fire.SetActive(false);
        MainMenu = GameObject.Find("/Canvas/MainMenu");

        extras = 0;
        flashing = true;
        active = false;
        n = 3;
        obsspeed = 5;
        score = 0;
        time = 4;
        flash = time;
        t = time;
        obj = GameObject.FindGameObjectsWithTag("StartUp");
        scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
        Rect vrect = Camera.main.pixelRect;        
        vrect.height *= (Screen.width / 893f)*(458f / Screen.height); 
        Camera.main.pixelRect = vrect;
        vrect.height = Screen.height - vrect.height;
        vrect.y = Screen.height - vrect.height;
        Camera.allCameras[1].pixelRect = vrect;

        ObstacleMoves.godmode = false;
        objcount = 0;

        //LOAD SCORE
        if (File.Exists(Application.persistentDataPath + "/30Max"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/30Max", FileMode.Open);
            SaveData sd = (SaveData)bf.Deserialize(fs);
            GameObject.Find("/Canvas/MainMenu/HighScore").GetComponent<Text>().text = "High Score : \n" + sd.playername + " - " + sd.playerscore;
            fs.Close();
        }

        // $_$
        Admob.Instance().initAdmob("ca-app-pub-7513429653336442/4823870216", "ca-app-pub-7513429653336442/4324342617");
        Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            t -= Time.deltaTime;
            //FLASH INCOMING
            if (flashing)
            {
                if (GameObject.FindGameObjectWithTag("Incoming") != null && t < flash)
                {
                    if (GameObject.FindGameObjectWithTag("Incoming").transform.GetComponent<TextMesh>().text == "")
                    {
                        GameObject.FindGameObjectWithTag("Incoming").transform.GetComponent<TextMesh>().text = "Watch your step ...";
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("Incoming").transform.GetComponent<TextMesh>().text = "";
                    }
                    flash -= 0.5f;
                }
            }

            if (t < 0)
            {
                //DESTROY INCOMING TEXT
                if (flashing)
                {
                    if (GameObject.FindGameObjectWithTag("Incoming") != null)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("Incoming"));
                    }
                }

                //DECIDE OBSTACLE
                int i = 0;
                int r = new System.Random().Next(1, 11);
                if (r < 8)
                {
                    i = 1;
                }
                else
                {
                    i = 0;
                }

                //CREATE OBSTACLE
                if (objcount < 30 || ObstacleMoves.godmode)
                {
                    Instantiate(obj[i], spawn.transform.position, transform.rotation);
                    objcount++;
                }

                //RESTART TIMER
                t = time;

                //STOP FLASHING
                flashing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Exit.activeSelf)
            {
                Exit.SetActive(false);
            }
            else
            {
                Exit.SetActive(true);
            }
        }
	}
    
    public void SetHighScore(UnityEngine.UI.InputField playername)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs;
        fs = File.Create(Application.persistentDataPath + "/30Max");
        SaveData ss = new SaveData();
        ss.playerscore = score + extras;
        ss.playername = playername.text;
        bf.Serialize(fs, ss);
        fs.Close();
        SceneManager.LoadScene("Game1");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Game1");
    }

    public void StartGame()
    {
        active = true;
        ObstacleMoves.active = true;
        MainMenu.SetActive(false);
    }

    public void StartGM()
    {
        active = true;
        ObstacleMoves.active = true;
        GameObject.Find("Player").GetComponent<Renderer>().material = godmaterial;
        Fire.SetActive(true);
        Destroy(GMMenu);
    }

    public void AppQuit()
    {
        Application.Quit();
    }
}
