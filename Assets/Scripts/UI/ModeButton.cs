using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ModeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject hover;
	public TextMeshProUGUI modeDesc;
	public AudioSource sfxPlayer;

	[TextArea]
	public string description;

	private Animator hoverAnim;

	private void Start()
	{
		hoverAnim = hover.GetComponent<Animator>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		modeDesc.text = description;
		hoverAnim.SetTrigger("hover");

		sfxPlayer.pitch = Random.Range(0.8f, 1.2f);
		sfxPlayer.PlayOneShot(sfxPlayer.clip);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hoverAnim.SetTrigger("left");
	}
}
