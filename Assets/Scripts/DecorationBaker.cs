#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

public class DecorationBaker : MonoBehaviour
{
    [SerializeField] bool bakeTextureButton;
    [SerializeField] List<GameObject> objects;
    public Material renderMaterial;
    [SerializeField] RenderTexture combinedTexture;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        combinedTexture = new RenderTexture(Screen.width, Screen.height, 24);
        combinedTexture.enableRandomWrite = true;
        combinedTexture.Create();
    }

    void Update()
    {
        if (bakeTextureButton)
        {
            bakeTextureButton = false;
            BakeTextures();
        }
    }

    void BakeTextures()
    {
        objects = objects.OrderByDescending(x=>x.transform.position.y).ToList();

        Matrix4x4 matrix;

        for (int i = 0; i < objects.Count; i++)
        {
            GameObject obj = objects[i];
            var sprite = obj.GetComponent<SpriteRenderer>();

            float screenAspectRatio = Screen.height / Screen.width;
            float spriteAspectRatio = sprite.sprite.texture.height/ sprite.sprite.texture.width;
            float cameraWidth = Camera.main.orthographicSize;
            

            int cameraPixelRatio = Camera.main.gameObject.GetComponent<PixelPerfectCamera>().pixelRatio;
            float pixelsPerUnit = sprite.sprite.pixelsPerUnit * cameraPixelRatio;

            Debug.Log(cameraWidth);

            Vector2 offset = sprite.sprite.pivot / sprite.sprite.pixelsPerUnit;

            Vector3 scale = new(obj.transform.localScale.x / cameraWidth, 
                                obj.transform.localScale.y / cameraWidth * spriteAspectRatio / screenAspectRatio,                                                             
                                obj.transform.localScale.z / cameraWidth);

            Vector3 pos = new  ((obj.transform.position.x) / cameraWidth,
                                (obj.transform.position.y) / cameraWidth / screenAspectRatio,
                                (obj.transform.position.z) / cameraWidth);
            
            renderMaterial.mainTexture = sprite.sprite.texture;
            matrix = Matrix4x4.TRS(pos, obj.transform.rotation, scale);
            renderMaterial.SetMatrix("_MyTRSMatrix", matrix);
            Graphics.Blit(sprite.sprite.texture, combinedTexture, renderMaterial);
        }

        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        renderMaterial.mainTexture = combinedTexture;

        Texture2D tex = new Texture2D(Screen.width,Screen.height);
        tex.filterMode = FilterMode.Point;
        Graphics.ConvertTexture(combinedTexture, tex);

        spriteRenderer.sprite = Sprite.Create(tex,new Rect(0f,0f,Screen.width,Screen.height),new Vector2(0.0f,0.0f));
    }
}
#endif