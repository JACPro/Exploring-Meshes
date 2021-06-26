using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ScrollNormalMap : MonoBehaviour
{
    [SerializeField] float xSpeed, ySpeed;
    
    float xOffset = 0, yOffset = 0, growOffset;

    [SerializeField] float growSpeed, growAmount;
    Vector2 tileScale;
    Material mat;
    void Awake() {
        mat = GetComponent<MeshRenderer>().material;
        tileScale = mat.GetTextureScale("_DetailAlbedoMap");
    }

    // Update is called once per frame
    void Update()
    {
        xOffset += (xSpeed * Time.deltaTime) / 100;
        //yOffset += (ySpeed * Time.deltaTime) / 100;
        yOffset = Mathf.Sin(Time.time) / 200;
        growOffset = Mathf.Sin(Time.time * growSpeed) / ((1 / growAmount) * 200);
        mat.SetTextureOffset("_DetailAlbedoMap", new Vector2(xOffset, yOffset));
        mat.SetTextureScale("_DetailAlbedoMap", new Vector2(tileScale.x + growOffset, tileScale.y + growOffset));
    }
}
