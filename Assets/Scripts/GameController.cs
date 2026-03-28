using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool _threwDie;
    private bool _restartGame;
    private bool _moveBoxComplete;
    private bool _restartComplete;
    private bool _hasShownResult;

    [Header("Visuals")]
    public Die die;
    public Box box;
    private bool _moveBoxToView;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI resultText;

    // Start is called before the first frame update
    void Start()
    {
        if (resultText == null)
        {
            resultText = CreateResultText();
        }
        resultText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (die.HitCount < 1 && !_threwDie)
        {
            if (keyboard != null && keyboard.spaceKey.isPressed)
            {
                SpinDie();
            }

            if (keyboard != null && keyboard.spaceKey.wasReleasedThisFrame)
            {
                ThrowDie();
                _threwDie = true;
            }
        }

        if (die.LandedWithUpFace && !_hasShownResult)
        {
            _hasShownResult = true;
            resultText.text = $"You rolled a {die.Upface}!";
            _moveBoxToView = true;
        }

        if (_moveBoxToView)
        {
            _moveBoxComplete = box.MoveBoxToView();
            if (_moveBoxComplete)
            {
                _moveBoxToView = false;
            }
        }

        if (keyboard != null && keyboard.rKey.wasPressedThisFrame && !_restartComplete && _moveBoxComplete)
        {
            _threwDie = false;
            _restartGame = true;
        }

        if (_restartGame)
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        _restartComplete = die.ResetPosition();
        _restartComplete = box.ResetPosition();
        if (_restartComplete)
        {
            _hasShownResult = false;
            resultText.text = "";
            _moveBoxComplete = false;
            _restartGame = false;
            _restartComplete = false;
        }
    }

    private void SpinDie()
    {
        die.Spin();
    }

    private void ThrowDie()
    {
        die.Throw(box.transform.position);
    }

    private TextMeshProUGUI CreateResultText()
    {
        var canvasGO = new GameObject("ResultCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var textGO = new GameObject("ResultText");
        textGO.transform.SetParent(canvasGO.transform, false);
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 72;
        tmp.color = Color.white;

        var rt = tmp.rectTransform;
        rt.anchorMin = new Vector2(0f, 0.6f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        return tmp;
    }
}
