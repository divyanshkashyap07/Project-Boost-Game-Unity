using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainengine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainengineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;

    Rigidbody rigidbody;
    enum State { alive,trandescing,dead};
    State state = State.alive;
    AudioSource audioSource;
    bool Collission = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.alive)
        {
            Thrusting();
            Rotate();
        }
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
            LoadNewScene();
        else if (Input.GetKeyDown(KeyCode.C))
            Collission = !Collission;            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.alive || !Collission)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.trandescing;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                successParticle.Play();
                Invoke("LoadNewScene", 1f);
                break;
            default:
                state = State.dead;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticle.Play();
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(mainengine);
            mainengineParticle.Play();
        }
        else
        {
            audioSource.Stop();
            mainengineParticle.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;
        float rotateThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotateThisFrame);
        }
        rigidbody.freezeRotation = false;
    }

}
