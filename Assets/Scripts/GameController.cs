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

    [Header("Visuals")]
    public Die die;
    public Box box;
    private bool _moveBoxToView;

    // Start is called before the first frame update
    void Start()
    {
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

        if (die.LandedWithUpFace && !_moveBoxComplete)
        {
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
