//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public GameObject mapUI;
    public GameObject endUI;
    public GameObject player;

    // The parent transform of all the buttons
    public Transform levelButtons;
    // The parent transform of all the lines between buttons
    public Transform lines;
    // The parent transform of all the level holders
    public Transform levels;
    public int levelsCompleted;

    void Start() 
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        player.SetActive(false);
        mapUI.SetActive(true);

        for (int i = 0; i < levelsCompleted + 1; i++)
        {
            levelButtons.GetChild(i).gameObject.SetActive(true);
            lines.GetChild(i).gameObject.SetActive(true);
            levels.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void LevelEnd(int levelNumber)
    {
        if (levelsCompleted == levelNumber - 1)
        {
            levelsCompleted++;
        }

        foreach (GameObject puddle in GameObject.FindGameObjectsWithTag("GluePuddle"))
        {
            Destroy(puddle);
        }

        UpdateMap();
    }

    public void GameEnd()
    {
        LevelEnd(1);
        endUI.SetActive(true);
    }

    public void GameContinue()
    {
        LevelEnd(1);
        endUI.SetActive(false);
    }




    public void Level1()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(0).gameObject.SetActive(true);
    }

    public void Level2()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(1).gameObject.SetActive(true);
    }

    public void Level3()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(2).gameObject.SetActive(true);
    }

    public void Level4()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(3).gameObject.SetActive(true);
    }

    public void Level5()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(4).gameObject.SetActive(true);
    }

    public void Level6()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(5).gameObject.SetActive(true);
    }

    public void Level7()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(6).gameObject.SetActive(true);
    }

    public void Level8()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(7).gameObject.SetActive(true);
    }

    public void Level9()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(8).gameObject.SetActive(true);
    }

    public void Level10()
    {
        mapUI.SetActive(false);
        player.SetActive(true);
        levels.GetChild(9).gameObject.SetActive(true);
    }
}
