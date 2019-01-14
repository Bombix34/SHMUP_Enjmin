using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgueAnim : MonoBehaviour {

	[SerializeField]
	List<Sprite> sprites;

	int index=0;

	SpriteRenderer render;

	static float animSpeed=0.065f;

	void Start () {
		render=GetComponentInChildren<SpriteRenderer>();
		index=(int)Random.Range(0f,sprites.Count);
		StartCoroutine(AnimCoroutine());
	}

	IEnumerator AnimCoroutine()
	{
		while(true)
		{
			index++;
			if(index==sprites.Count)
				index=0;
			render.sprite=sprites[index];
			yield return new WaitForSeconds(animSpeed);
		}
	}
}
