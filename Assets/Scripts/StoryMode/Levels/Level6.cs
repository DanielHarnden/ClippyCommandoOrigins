using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Level6 : MonoBehaviour
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

    void OnEnable() 
    {
        // Find objects
        quartermaster = GameObject.FindGameObjectWithTag("Quartermaster");
        worldMap = GameObject.FindGameObjectWithTag("Map").GetComponent<WorldMap>();
        player = GameObject.FindGameObjectWithTag("Player");
        TextObject = quartermaster.transform.GetChild(0).GetChild(0).GetComponent<Text>();

        // Initialize objects
        quartermaster.SetActive(true);
        player.transform.position = new Vector3(298, -180, 0);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(298, -180, 0);
        player.GetComponent<PlayerStatsStory>().Initialize(new bool[]{true, true, false, false}, 2, 64);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft = 20;

        // Initialize level prefab
        tempLevel = Instantiate(level, this.transform.position, Quaternion.identity);

        // Initialize pathfinding for this level
        AstarData data = AstarPath.active.data;
        GridGraph gg = AstarPath.active.data.gridGraph;
        gg.center = new Vector3 (349, -147, 0);
        gg.SetDimensions(225, 50, 0.5f);
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
            } 
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft <= 0 && currentLine != 1)
        {
            currentLine = 1;
            nextLine = true;
        }
    }

	

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Player leave area
        if (other.gameObject.tag == "Player" && currentLine == 1)
        {
            Destroy(tempLevel);
            worldMap.LevelEnd(6);
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