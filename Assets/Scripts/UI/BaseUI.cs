using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    protected Dictionary<string, RectTransform> transforms;
    protected Dictionary<string, Button> buttons;
    protected Dictionary<string, TMP_Text> texts;
    protected Dictionary<string, Slider> sliders;
    protected Dictionary<string, Image> images;
    protected Dictionary<string, ToggleGroup> toggleGroups;
    protected Dictionary<string, Toggle> toggles;
    [SerializeField] protected AudioSource clickAudio;

    protected virtual void Awake()
    {
        BindingChildren();
        AddClickAudio();
    }

    protected virtual void BindingChildren()
    {
        transforms = new Dictionary<string, RectTransform>();
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();
        sliders = new Dictionary<string, Slider>();
        images = new Dictionary<string, Image>();
        toggleGroups = new Dictionary<string, ToggleGroup>();
        toggles = new Dictionary<string, Toggle>();

        RectTransform[] childrenRect = GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < childrenRect.Length; i++)
        {
            string key = childrenRect[i].name;
            if (!transforms.ContainsKey(key))
            {
                transforms[key] = childrenRect[i];

                Button btn = childrenRect[i].GetComponent<Button>();
                if (btn)
                {
                    if (!buttons.ContainsKey(key))
                        buttons[key] = btn;
                }

                TMP_Text txt = childrenRect[i].GetComponent<TMP_Text>();
                if (txt)
                {
                    if (!texts.ContainsKey(key))
                        texts[key] = txt;
                }

                Slider sld = childrenRect[i].GetComponent<Slider>();
                if (sld)
                {
                    if (!sliders.ContainsKey(key))
                        sliders[key] = sld;
                }

                Image img = childrenRect[i].GetComponent<Image>();
                if (img)
                {
                    if (!images.ContainsKey(key))
                        images[key] = img;
                }

                ToggleGroup tgg = childrenRect[i].GetComponent<ToggleGroup>();
                if (tgg)
                {
                    if (!toggleGroups.ContainsKey(key))
                        toggleGroups[key] = tgg;
                }

                Toggle tgl = childrenRect[i].GetComponent<Toggle>();
                if (tgl)
                {
                    if (!toggles.ContainsKey(key))
                        toggles[key] = tgl;
                }
            }
        }
    }

    protected virtual void AddClickAudio()
    {
        AudioSource audio = GameManager.Resource.Instantiate<AudioSource>("Audio/SFX/UI");
        audio.transform.parent = transform;
        foreach (KeyValuePair<string, Button> button in buttons)
        {
            button.Value.onClick.AddListener(() => { audio.Play(); });
        }
    }

    public virtual void CloseUI()
    {

    }
}