using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private bool _threwDie;
    private bool _restartGame;
    private bool _moveBoxComplete;
    private bool _restartComplete;
    private bool _hasShownResult;
    private string _resultMessage = "";
    private GUIStyle _resultStyle;

    [Header("Visuals")]
    public Die die;
    public Box box;
    private bool _moveBoxToView;

    // Start is called before the first frame update
    void Start()
    {
        _resultStyle = new GUIStyle
        {
            fontSize = 48,
            alignment = TextAnchor.UpperCenter,
            fontStyle = FontStyle.Bold
        };
        _resultStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        if (!string.IsNullOrEmpty(_resultMessage))
        {
            GUI.Label(
                new Rect(0, Screen.height * 0.1f, Screen.width, Screen.height * 0.2f),
                _resultMessage,
                _resultStyle);
        }
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
            _resultMessage = $"You rolled a {die.Upface}!";
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
            _resultMessage = "";
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
}
