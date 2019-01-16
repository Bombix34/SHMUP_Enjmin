using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesManager : MonoBehaviour {

    public static TentaclesManager instance;

    public GameObject[] tentacles;

    public float distanceAtEachCapture = 2f;
    public float moveSpeedForward = 0.1f;

    public float distanceAtEachSave = 2f;
    public float moveSpeedBackward = 0.1f;

    private Coroutine deplacement;

    public float distanceDone = 0;
    
    [SerializeField]
    List<ParticleSystem> fishParticles;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        foreach(ParticleSystem particle in fishParticles)
        {
            particle.gameObject.SetActive(false);
            particle.Stop();
        }

        float dist = Camera.main.transform.position.z;

        Vector3 hg = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -dist));
        Vector3 bg = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -dist));

        Vector3 pos;

        //bordure Gauche
        transform.position = hg;
        pos = bg - hg;
        transform.position.Scale(new Vector3(0.5F, 0.5F, 0.5F));
        transform.Translate(pos / 2);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ToSave")
        {
            MoveForward();

            LevelManager.instance.ChangeScore(LevelManager.instance.reglages.malusAmiMangeParKraken);

            AkSoundEngine.PostEvent("Play_Kraken_Eat_Pnj", gameObject);

            GetComponentInChildren<TentacleDetection>().Retract();
        }
        else if(collision.gameObject.tag=="Player")
        {
            GameManager.instance.GameOver();
        }
    }

    public void MoveForward()
    {
        // TODO : animation de capture
        if(Camera.main.GetComponent<CameraShaker>()!=null)
			Camera.main.GetComponent<CameraShaker>().LaunchShake(1f,0.1f);
        // deplacement des tentacules
        distanceDone += distanceAtEachCapture;
        
        StartCoroutine(LaunchFishParticles());

        StartCoroutine(MoveForwardCoroutine());
    }

    IEnumerator LaunchFishParticles()
    {
        float rand = Random.Range(0f,100f);
        yield return new WaitForSeconds(1f);
        if(rand>=55)
        {
            foreach(ParticleSystem particle in fishParticles)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }
        }
    }

    public void MoveBackward()
    {
        // deplacement des tentacules
        distanceDone -= distanceAtEachSave;

        if((Camera.main.GetComponent<CameraShaker>()!=null)&&(distanceDone==0))
			Camera.main.GetComponent<CameraShaker>().LaunchShake(0.5f,0.05f);

        StartCoroutine(MoveBackwardCoroutine());
    }

    IEnumerator MoveForwardCoroutine()
    {
        float dist = 0;
        while(distanceAtEachCapture > dist)
        {
            transform.position = new Vector3(transform.position.x + moveSpeedForward, transform.position.y, transform.position.z);
            dist += moveSpeedForward;
            yield return null;
        }
    }

    IEnumerator MoveBackwardCoroutine()
    {
        float dist = 0;
        while (distanceAtEachSave > dist && distanceDone >= 0)
        {
            transform.position = new Vector3(transform.position.x - moveSpeedBackward, transform.position.y, transform.position.z);
            dist += moveSpeedBackward;
            yield return null;
        }
    }
}
