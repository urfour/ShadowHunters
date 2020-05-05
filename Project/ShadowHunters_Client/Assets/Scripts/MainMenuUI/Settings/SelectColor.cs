using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kernel.Settings;

public class SelectColor : SettingPrefab
{
    public Slider hueSlider;
    public Image hueTextureTarget;
    public Slider satSlider;
    public Image satTextureTarget;
    public Slider vSlider;
    public Image vTextureTarget;
    public Image colorTarget;
    public RectTransform pointerRect;

    Setting<Color> target;
    bool refreshMode = false;
    public Texture2D huesprite;
    public Texture2D satSprite;
    public Texture2D vSprite;


    public override void Configurate(string label, SettingItem target, string config = null)
    {
        this.target = (Setting<Color>)target;
        //float h, s, v;
        //Color.RGBToHSV(this.target.Value, out h, out s, out v);
        //hueSlider.value = h;
        //satSlider.value = s;
        //vSlider.value = v;
        //colorTarget.color = this.target.Value;
        base.Configurate(label, target, config);
        huesprite = new Texture2D((int)hueTextureTarget.rectTransform.rect.width, (int)hueTextureTarget.rectTransform.rect.height);
        satSprite = new Texture2D((int)satTextureTarget.rectTransform.rect.width, (int)satTextureTarget.rectTransform.rect.height);
        vSprite = new Texture2D((int)vTextureTarget.rectTransform.rect.width, (int)vTextureTarget.rectTransform.rect.height);
        Refresh();
    }

    public void OnChange()
    {
        if (refreshMode) return;
        target.Value = Color.HSVToRGB(hueSlider.value, satSlider.value, vSlider.value);
    }

    public void CalculateSprites()
    {
        float h, s, v;
        Color.RGBToHSV(target.Value, out h, out s, out v);
        for (int x = 0; x < huesprite.width; x++)
        {
            float ch = ((float)x) / huesprite.width;
            Color c = Color.HSVToRGB(ch%1.0f,s,v);
            for (int y = 0; y < huesprite.height; y++)
            {
                huesprite.SetPixel(x, y, c);
            }
        }
        huesprite.Apply();

        for (int x = 0; x < satSprite.width; x++)
        {
            float cs = ((float)x) / satSprite.width;
            Color c = Color.HSVToRGB(h, cs%1.0f, v);
            c.a = 1;
            for (int y = 0; y < satSprite.height; y++)
            {
                satSprite.SetPixel(x, y, c);
            }
        }
        satSprite.Apply();

        for (int x = 0; x < vSprite.width; x++)
        {
            float cv = ((float)x) / vSprite.width;
            Color c = Color.HSVToRGB(h, s, cv%1.0f);
            for (int y = 0; y < vSprite.height; y++)
            {
                vSprite.SetPixel(x, y, c);
            }
        }
        vSprite.Apply();

        hueTextureTarget.sprite = Sprite.Create(huesprite, new Rect(0,0,huesprite.width, huesprite.height), new Vector2(0.5f, 0.5f));
        satTextureTarget.sprite = Sprite.Create(satSprite, new Rect(0, 0, satSprite.width, satSprite.height), new Vector2(0.5f, 0.5f));
        vTextureTarget.sprite = Sprite.Create(vSprite, new Rect(0, 0, vSprite.width, vSprite.height), new Vector2(0.5f, 0.5f));
    }

    public override void Refresh()
    {
        refreshMode = true;
        float h, s, v;
        Color.RGBToHSV(target.Value, out h, out s, out v);
        hueSlider.value = h;
        satSlider.value = s;
        vSlider.value = v;
        colorTarget.color = target.Value;
        CalculateSprites();
        refreshMode = false;
    }
}
