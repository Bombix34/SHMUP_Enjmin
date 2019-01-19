using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclesManager : MonoBehaviour {

    public static TentaclesManager instance;


    [SerializeField]
    GameObject rightWall;

   // public GameObject[] tentacles;

    public float distanceAtEachCapture = 2f;
    public float moveSpeedForward = 0.3f;

    public float distanceAtEachSave = 2f;
    public float moveSpeedBackward = 1.5f;
    public float backwardTimeAtEachSave = 1f;

    private float timerCooldown = 0f;
    private float timeRemaining = 0f;

    private Coroutine deplacement;

    public float distanceDone = 0;
    
    [SerializeField]
    List<ParticleSystem> fishParticles;

    Vector2 initPosition;
    Vector2 initPositionRightWall;

    GameManager manager;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        manager=GameManager.instance;
    }

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
        //transform.position.Scale(new Vector3(0.5F, 0.5F, 0.5F));
        transform.Translate(pos / 2);

        initPosition=transform.position;
        initPositionRightWall=rightWall.transform.position;

        //position du right wall => a tweaker
       // rightWall.transform.position= new Vector2(Camera.main.orthographicSize * Camera.main.aspect+(LevelManager.instance.GetGameObjectWidth(rightWall)/3),0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ToSave")
        {
           // MoveForward();
           Debug.Log(collision.gameObject);

            LevelManager.instance.ChangeScore(LevelManager.instance.reglages.malusAmiMangeParKraken);

            AkSoundEngine.PostEvent("Play_Kraken_Eat_Pnj", gameObject);

            GetComponentInChildren<TentacleDetection>().Retract();
        }
        else if(collision.gameObject.tag=="Player")
        {
            GameManager.instance.GameOver();
        }
    }

    void Update()
    {
        if(manager.IsGameOver())
            return;
        if(timeRemaining > 0)
        {
           /*  if(distanceDone<0)
            {
                distanceDone=0;
                timeRemaining=0;
                return;
            }*/
            if(transform.position.x<initPosition.x||rightWall.transform.position.x>initPositionRightWall.x)
            {
                transform.position=initPosition;
                rightWall.transform.position=initPositionRightWall;
                distanceDone=0;
                timeRemaining=0;
                return;
            }
            //Vector3 hg = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -dist));
            distanceDone-=moveSpeedBackward*Time.deltaTime;
            transform.position = new Vector3(transform.position.x - moveSpeedBackward * Time.deltaTime, transform.position.y, transform.position.z);
            rightWall.transform.position = new Vector3(rightWall.transform.position.x + moveSpeedBackward * Time.deltaTime, rightWall.transform.position.y, rightWall.transform.position.z);
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            distanceDone+=moveSpeedForward*Time.deltaTime;

            float modifSpeed=1f;

            if(distanceDone>6f)
                modifSpeed=2.5f;

            transform.position = new Vector3(transform.position.x + moveSpeedForward * Time.deltaTime * modifSpeed, transform.position.y, transform.position.z);
            rightWall.transform.position = new Vector3(rightWall.transform.position.x - moveSpeedForward * Time.deltaTime * modifSpeed, rightWall.transform.position.y, rightWall.transform.position.z);    

            if(distanceDone>=7f)
                GameManager.instance.GameOver();
        }
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

    public void MoveBackward()
    {
        // deplacement des tentacules
        distanceDone -= distanceAtEachSave;

        timeRemaining += backwardTimeAtEachSave;

        if((Camera.main.GetComponent<CameraShaker>()!=null))
			Camera.main.GetComponent<CameraShaker>().LaunchShake(0.5f,0.05f);

        //StartCoroutine(MoveBackwardCoroutine());

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
