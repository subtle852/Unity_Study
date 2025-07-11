using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum Images
    {
        ItemIcon,
    }

    enum GameObjects
    {
        TestObject,
    }

    private void Start()
    {
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

        GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked);

    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log($"Button Clicked");

        _score++;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }

}
