using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteSystemManager : MonoBehaviour
{
    public static EmoteSystemManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void CreateEmote(Vector3 point, string emoteName,
        float lifeTime = 0.6f, float shakeAmplitude = 15f, float shakePeriod = 25f)
    {
        string prefabPath = $"Prefabs/EmoteBase";
        string spritePath = $"Images/Emotes/emote_{emoteName}";

        SpriteRenderer emotePrefab = Resources.Load<SpriteRenderer>(prefabPath);
        Sprite spriteOrig = Resources.Load<Sprite>(spritePath);

        SpriteRenderer emote = Instantiate(emotePrefab);

        emote.sprite = spriteOrig;

        emote.transform.position = point;

        AnimateEmote(emote, lifeTime, shakeAmplitude, shakePeriod);
    }

    public void CreateEmote(Transform pointOfOrigin, string emoteName, 
        float lifeTime = 0.6f, float shakeAmplitude = 15f, float shakePeriod = 25f )
    {
        string prefabPath = $"Prefabs/EmoteBase";
        string spritePath = $"Images/Emotes/emote_{emoteName}";

        SpriteRenderer emotePrefab = Resources.Load<SpriteRenderer>(prefabPath);
        Sprite spriteOrig = Resources.Load<Sprite>(spritePath);

        SpriteRenderer emote = Instantiate(emotePrefab);

        emote.sprite = spriteOrig;

        AnimateEmote(emote, pointOfOrigin, lifeTime, shakeAmplitude, shakePeriod);
    }

    private void AnimateEmote(SpriteRenderer emote, Transform followTransfrom,
        float lifeTime, float shakeAmplitude, float shakePeriod)
    {
        StartCoroutine(AnimatePosition(emote, followTransfrom));
        StartCoroutine(AnimateWiggle(emote, shakeAmplitude, shakePeriod));
        Destroy(emote.gameObject, lifeTime);
    }

    private void AnimateEmote(SpriteRenderer emote,
        float lifeTime, float shakeAmplitude, float shakePeriod)
    {
        StartCoroutine(AnimateWiggle(emote, shakeAmplitude, shakePeriod));
        Destroy(emote.gameObject, lifeTime);
    }

    private IEnumerator AnimateWiggle(SpriteRenderer emote, float amplitude, float period)
    {
        float timer = 0f;

        while (emote != null)
        {
            timer += Time.deltaTime;
            float angle = Mathf.Sin(period * timer) * amplitude;
            emote.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            yield return null;
        }
    }

    private IEnumerator AnimatePosition(SpriteRenderer emote, Transform followTransfrom)
    {
        while (emote != null)
        {
            emote.transform.position = followTransfrom.position + Vector3.up * 0.6f;

            yield return null;
        }

        yield return null;
    }
}
