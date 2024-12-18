using UnityEngine;
using UnityEngine.UI;

public class CopyImage : MonoBehaviour
{
    public Image imageToCopy;
    private Image thisImage;

    private void Awake()
    {
		thisImage = GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(thisImage.sprite != imageToCopy.sprite)
			thisImage.sprite = imageToCopy.sprite;
    }
}
