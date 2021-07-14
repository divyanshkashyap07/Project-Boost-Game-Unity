using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor;
    Vector3 start;

	void Start ()
    {
        start = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (period == 0)
            period = 0.1f;
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float SineWave = Mathf.Sin(cycles * tau);

        movementFactor = SineWave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = start + offset;
	}
}
