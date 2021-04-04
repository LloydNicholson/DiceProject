using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool _threwDie;
    private bool _canMoveBox = true;
    private bool _restart;

    [Header("Visuals")]
    public Die die;
    public Box box;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (die.HitCount < 1 && !_threwDie)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ThrowDie();
                _threwDie = true;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                SpinDie();
            }
        }

        if (die.LandedWithUpFace && _canMoveBox)
        {
            _canMoveBox = box.CanMoveBoxToView();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _threwDie = false;
            _restart = true;
        }

        if (_restart)
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        _restart = !die.SetOriginalPosition();
        _restart = !box.SetOriginalPosition();
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
