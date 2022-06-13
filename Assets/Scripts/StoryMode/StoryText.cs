using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The base script for dialogue. Taken from an old project of mine.
public class StoryText : MonoBehaviour
{
    public string[] DIALOGUE;
	public int currentLine;
	public Text TextObject;
	public float textSpeed;
    public bool nextLine = false;
    public bool lineFinished = false;

	/*
		Good speeds for textSpeed: 
		
		0.5: Very slow. Good for long drawn out sentances
		0.3: Slow. Good for drawling sentances
		0.1: Kinda slow. Good for cautious sentances
		0.03: Normal. A good default speed
		0.005: Fast. Good for quick sentances
		0.0005: Very fast. Good for hurried talking.
	 */

    void Start() 
    {
        StartCoroutine (AnimateText ());
    }

    void Update() 
    {
        if (nextLine)
        {
            if (lineFinished) 
            {
                currentLine++;
                StartCoroutine (AnimateText ());
                nextLine = false;
            } 
        }
    }

	IEnumerator AnimateText () 
	{
        lineFinished = false;

		for (int i = 0; i < (DIALOGUE[currentLine].Length + 1); i++) 
		{
			TextObject.text = DIALOGUE [currentLine].Substring (0, i);
			yield return new WaitForSeconds (textSpeed);
		}

        yield return new WaitForSeconds (textSpeed * 5f);

        lineFinished = true;
	}
}