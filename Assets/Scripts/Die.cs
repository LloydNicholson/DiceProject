using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private bool _hitSomething;
    private Rigidbody _diceRb;

    [Header("Movement")]
    public float force;
    public float resetSpeed = 3f;

    [Header("Visuals")]
    public GameObject model;

    [Header("Scripts")]
    public FaceHandler faceHandler;

    public int HitCount { get; set; }
    public RaycastHit HitData { get; set; }
    public int? Upface { get; private set; }
    public bool LandedWithUpFace { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _diceRb = model.GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 targetPosition)
    {
        var randomFloat = Random.Range(-0.2f, 0.2f);
        var offset = new Vector3(randomFloat, randomFloat, 0);
        _diceRb.useGravity = true;
        var normalisedValue = targetPosition - (transform.position + offset) + (Vector3.up * 0.6f);
        _diceRb.AddForce(normalisedValue * force);
    }

    private void Update()
    {
        if (Physics.Raycast(model.transform.position, Vector3.down, out var hit, 0.15f))
        {
            HitData = hit;
            _hitSomething = true;
            HitCount++;
            if (Vector3.Distance(_diceRb.velocity, Vector3.zero) <= 0.01f)
            {
                var upFace = faceHandler.CheckFaceUp();
                Upface = upFace;
                LandedWithUpFace = true;
            }
        }
    }

    public bool ResetPosition()
    {
        _diceRb.useGravity = false;
        _diceRb.velocity = Vector3.zero;
        _hitSomething = false;
        LandedWithUpFace = false;
        HitCount = 0;

        var targetStartRot = Quaternion.Euler(Vector3.zero);
        if (model.transform.rotation != targetStartRot && model.transform.position != transform.position)
        {
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetStartRot, resetSpeed * Time.deltaTime);
            model.transform.position = Vector3.Lerp(model.transform.position, transform.position, resetSpeed * Time.deltaTime);

            return false;
        }

        return true;

    }

    public void Spin()
    {
        var rand = Random.Range(0, 720) * Time.deltaTime;
        model.transform.Rotate(Vector3.Lerp(model.transform.position, model.transform.position, 0.5f), rand);
    }
}
