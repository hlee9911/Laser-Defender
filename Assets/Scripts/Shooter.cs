using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectSpeed = 10f;
    [SerializeField] float projectLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField] float firinRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [SerializeField] bool useAI;

    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;

    [HideInInspector] public bool isFiring;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();    
    }

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject instance = Instantiate(projectilePrefab,
                                              transform.position,
                                              Quaternion.identity);

            Rigidbody2D rb2d = instance.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                rb2d.velocity = transform.up * projectSpeed;
            }

            Destroy(instance, projectLifetime);
            float timeToNextProjectile = Random.Range(baseFiringRate - firinRateVariance,
                                                      baseFiringRate + firinRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
