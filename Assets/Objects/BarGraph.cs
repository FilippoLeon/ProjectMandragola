using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarGraph : MonoBehaviour {

    public static float scaling = 100f;
    public static float offset = 0.25f;
    Color32 color;
    int h = 0;

    public Vector3 oldPos, targetPos;
    public Vector3 oldScale, targetScale;
    public float animationTimer = 0f;
    public float animationDuration = 1f;
    
    public void setHeight(int h_)
    {
        h = h_;

        oldPos = targetPos;
        oldScale = targetScale;
        targetPos = transform.position;
        targetPos.z = -h / 2f / scaling - offset;
        targetScale = transform.localScale;
        targetScale.z = h / scaling;
        animationTimer = 0f;
    }

    public void Update()
    {
        if (animationTimer <= animationDuration)
        {
            float frac = animationTimer / animationDuration;
            transform.position = Vector3.Lerp(oldPos, targetPos, frac);
            transform.localScale = Vector3.Lerp(oldScale, targetScale, frac);
            animationTimer += Time.deltaTime;
        }
    }

    public void OnEnable()
    {
        targetPos = transform.position;
        targetPos.z = -h / 2f / scaling - offset;
        targetScale = transform.localScale;
        targetScale.z = h / scaling;
        animationTimer = animationDuration;
    }

    public void setColor(Color32 color_)
    {
        // TODO: unsafe!
        color = color_;
        gameObject.GetComponent<Renderer>().materials[0].color = color;
        //gameObject.GetComponent<Renderer>().sharedMaterials[0].color = color;
    }

    public void recolor()
    {
        // TODO: unsafe!
        gameObject.GetComponent<Renderer>().materials[0].color = color;
        //gameObject.GetComponent<Renderer>().sharedMaterials[0].color = color;
    }
}
