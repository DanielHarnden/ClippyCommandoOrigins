using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Level4 : MonoBehaviour
{
    // Text and UI Stuff
    public string[] DIALOGUE;
	private int currentLine;
    private GameObject quartermaster;
    private WorldMap worldMap;
	private Text TextObject;
	public float textSpeed;
    private bool nextLine = false;
    private bool lineFinished = false;

    [Header ("Level Related Objects")]
    public GameObject level;
    private GameObject tempLevel;
    private GameObject player;
    private GameObject[] gluers;

    void OnEnable() 
    {
        // Find objects
        quartermaster = GameObject.FindGameObjectWithTag("Quartermaster");
        worldMap = GameObject.FindGameObjectWithTag("Map").GetComponent<WorldMap>();
        player = GameObject.FindGameObjectWithTag("Player");
        TextObject = quartermaster.transform.GetChild(0).GetChild(0).GetComponent<Text>();

        // Initialize objects
        quartermaster.SetActive(true);
        player.transform.position = new Vector3(-186, -113, 0);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(-186, -113, 0);
        player.GetComponent<PlayerStatsStory>().Initialize(new bool[]{true, true, false, false}, 2, 32);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft = 8;

        // Instantiate level prefab
        tempLevel = Instantiate(level, this.transform.position, Quaternion.identity);
        gluers = GameObject.FindGameObjectsWithTag("StoryGluer");

        // Initialize pathfinding for this level
        AstarData data = AstarPath.active.data;
        GridGraph gg = AstarPath.active.data.gridGraph;
        gg.center = new Vector3 (-130, -161, 0);
        gg.SetDimensions(250, 230, 0.5f);
        AstarPath.active.Scan();

        StartCoroutine (AnimateText ());
    }

    void Update() 
    {
        if (nextLine)
        {
            if (lineFinished) 
            {
                nextLine = false;
                if (currentLine <= DIALOGUE.Length - 1)
                {
                    StartCoroutine (AnimateText ());
                }

                // Extra idle dialogue
                if (currentLine == 1)
                {
                    StartCoroutine (Timer ());
                }
            } 
        }

        // Proceeds with dialogue
        if(currentLine == 0 && lineFinished)
        {
            currentLine = 1;
            nextLine = true;
        }

        // Dialogue if gluers are alerted
        if (currentLine <= 2)
        {
            foreach(GameObject gluer in gluers)
            {
                if (gluer != null)
                {
                    if (gluer.GetComponent<StoryGluer>().playerSeen && currentLine != 3)
                    {
                        currentLine = 3;
                        nextLine = true;
                    }
                }
            }
        }
    

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft <= 0 && currentLine != 4)
        {
            currentLine = 4;
            nextLine = true;
        }
        
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Player leave area
        if (other.gameObject.tag == "Player" && currentLine == 4)
        {
            Destroy(tempLevel);
            worldMap.LevelEnd(4);
        }
    }



    IEnumerator Timer()
    {
        yield return new WaitForSeconds (20f);

        if (currentLine == 1)
        {
            currentLine = 2;
            nextLine = true; 
        }   
    }



	IEnumerator AnimateText () 
	{
        lineFinished = false;
        nextLine = false;

		for (int i = 0; i < (DIALOGUE[currentLine].Length + 1); i++) 
		{
			TextObject.text = DIALOGUE [currentLine].Substring (0, i);
			yield return new WaitForSeconds (textSpeed);
		}

        yield return new WaitForSeconds (1);

        lineFinished = true;
	}   
}