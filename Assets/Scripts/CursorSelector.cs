using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

    public class CursorSelector : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Sprite cursorSprite;
        [SerializeField] Vector2 hotspot;
        Texture2D cursorTexture;
        void Awake()
        {
            if (cursorSprite != null)
            {
                cursorTexture = new Texture2D((int)cursorSprite.textureRect.width, (int)cursorSprite.textureRect.height, TextureFormat.RGBA32, false);
                for (int y = 0; y < cursorTexture.height; y++)
                {
                    for (int x = 0; x < cursorTexture.width; x++)
                    {
                        cursorTexture.SetPixel(x, y, cursorSprite.texture.GetPixel((int)cursorSprite.textureRect.x + x, (int)cursorSprite.textureRect.y + y));
                    }
                }
                cursorTexture.Apply();
            }
        }

            void Start()
    {
        // apply cursor immediately when scene starts
        if (cursorTexture != null)
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (cursorTexture != null)
                Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
    }
