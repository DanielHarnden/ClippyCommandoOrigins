using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Level1 : MonoBehaviour
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
    private GameObject bookPile;
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
        player.transform.position = new Vector3(-10, 0, 0);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(-10, 0, 0);
        player.GetComponent<PlayerStatsStory>().Initialize(new bool[]{false, false, false, false}, 0, 32);
        player.GetComponent<PlayerStatsStory>().enemiesLeft = 5;

        // Instantiate level prefab
        tempLevel = Instantiate(level, this.transform.position, Quaternion.identity);
        bookPile = GameObject.Find("pile1");
        
        // Initialize pathfinding for this level
        AstarData data = AstarPath.active.data;
        GridGraph gg = AstarPath.active.data.gridGraph;
        gg.center = new Vector3 (50, 2, 0);
        gg.SetDimensions(275, 110, 0.5f);
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
                currentLine++;
                if (currentLine <= DIALOGUE.Length - 1)
                {
                    StartCoroutine (AnimateText ());

                    // Level specific dialogue events
                    switch(currentLine)
                    {
                        // Proceeds to gun tutorial
                        case 1:
                            nextLine = true;
                            break;
                        
                        // Lets player leave starting area
                        case 2:
                            nextLine = true;
                            Destroy(bookPile);
                            break;
                    }
                }
            } 
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsStory>().enemiesLeft <= 0)
        {
            nextLine = true;
        }
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Pistol story pickup advancement
        if (other.gameObject.tag == "Player" && currentLine == 0)
        {
            nextLine = true;
        }

        // Player leave area
        if (other.gameObject.tag == "Player" && currentLine >= 4)
        {
            Destroy(tempLevel);
            worldMap.LevelEnd(1);
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