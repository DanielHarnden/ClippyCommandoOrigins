using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Level2 : MonoBehaviour
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
    private GameObject[] rulers;
    private bool idleDialogue;


    void OnEnable() 
    {
        // Find objects
        quartermaster = GameObject.FindGameObjectWithTag("Quartermaster");
        worldMap = GameObject.FindGameObjectWithTag("Map").GetComponent<WorldMap>();
        player = GameObject.FindGameObjectWithTag("Player");
        TextObject = quartermaster.transform.GetChild(0).GetChild(0).GetComponent<Text>();

        // Initialize objects
        quartermaster.SetActive(true);
        player.transform.position = new Vector3(-6.6f, -124.6f, 0);GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(-6.6f, -124.6f, 0);
        player.GetComponent<PlayerStatsStory>().Initialize(new bool[]{true, false, false, false}, 1, 64);
        player.GetComponent<PlayerStatsStory>().enemiesLeft = 10;
    
        // Initialize level prefab
        tempLevel = Instantiate(level, this.transform.position, Quaternion.identity);
        rulers = GameObject.FindGameObjectsWithTag("StoryRuler");

        // Initialize pathfinding for level
        AstarData data = AstarPath.active.data;
        GridGraph gg = AstarPath.active.data.gridGraph;
        gg.center = new Vector3 (50, -168, 0);
        gg.SetDimensions(250, 250, 0.5f);
        AstarPath.active.Scan();

        StartCoroutine (AnimateText ());
        StartCoroutine (Timer ());
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
            } 
        }

        // Level specific dialogue (changes if any ruler is alerted)
        if (currentLine <= 4)
        {
            if (currentLine == 3)
            {
                idleDialogue = true;
            }
            
            foreach(GameObject ruler in rulers)
            {
                if (ruler != null)
                {
                    if (ruler.GetComponent<StoryRuler>().playerSeen)
                    {
                        if (currentLine != 3)
                        {
                            nextLine = true;
                        }

                        currentLine = 3;
                        idleDialogue = false;
                    }
                }
            }

            if (idleDialogue)
            {
                if (currentLine != 4)
                {
                    nextLine = true;
                }

                currentLine = 4;
            }
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft <= 0 && currentLine != 5)
        {
            currentLine = 5;
            nextLine = true;
        }
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Player leave area
        if (other.gameObject.tag == "Player" && currentLine >= 5)
        {
            Destroy(tempLevel);
            worldMap.LevelEnd(2);
        }
    }

	

    // Quartermaster idle dialogue (without spooking rulers)
    IEnumerator Timer()
    {
        yield return new WaitForSeconds (30f);
        if (currentLine == 0)
        {   
            currentLine = 1;
            nextLine = true; 
        }
        
        yield return new WaitForSeconds (30f);
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