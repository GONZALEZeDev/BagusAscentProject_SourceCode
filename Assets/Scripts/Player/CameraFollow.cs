using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    static public CameraFollow instance;

    public Transform target = null; //généralement, le joueur
    private float smooth = 10;
    Vector3 offset;
    //Forêt claires
    private Vector3 minValueWRLD1 = new Vector3(-45, -20, -10);
    private Vector3 maxValueWRLD1 = new Vector3(4,62,-10);

    private Vector3 minValueBTWN12 = new Vector3(-15f, -18f, -10);
    private Vector3 maxValueBTWN12 = new Vector3(-7f, -6f, -10);

    //Forêt sombre
    private Vector3 minValueWRLD2 = new Vector3(-45, -18, -10);
    private Vector3 maxValueWRLD2 = new Vector3(45, 85, -10);

    private Vector3 minValueBTWN23 = new Vector3(-15f, -18f, -10);
    private Vector3 maxValueBTWN23 = new Vector3(-7f, -3f, -10);

    //Grotte
    private Vector3 minValueWRLD3 = new Vector3(-45, -22, -10);
    private Vector3 maxValueWRLD3 = new Vector3(55, 85, -10);

    private Vector3 minValueBTWN34 = new Vector3(-15f, -18f, -10);
    private Vector3 maxValueBTWN34 = new Vector3(-7f, -3f, -10);

    //Pic enneigé
    private Vector3 minValueWRLD4 = new Vector3(-78, -15, -10);
    private Vector3 maxValueWRLD4 = new Vector3(55, 150, -10);

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        if(PlayerPrefs.GetInt("isFullScreen") == 1)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else Screen.SetResolution(1920, 1080, false);

    }

    private void Start()
    {
        if (target)
        {
            offset = transform.position - target.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (target != null)
        {
            Vector3 targetCamPosition = target.position + offset;
            Vector3 boundPosition;
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 3:
                    //World 1
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueWRLD1.x, maxValueWRLD1.x),
                    Mathf.Clamp(targetCamPosition.y, minValueWRLD1.y, maxValueWRLD1.y),
                    Mathf.Clamp(targetCamPosition.z, minValueWRLD1.z, maxValueWRLD1.z)
                    );
                    break;
                case 4:
                    //World 1-2
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueBTWN12.x, maxValueBTWN12.x),
                    Mathf.Clamp(targetCamPosition.y, minValueBTWN12.y, maxValueBTWN12.y),
                    Mathf.Clamp(targetCamPosition.z, minValueBTWN12.z, maxValueBTWN12.z)
                    );
                    break;
                case 5:
                    //World 2
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueWRLD2.x, maxValueWRLD2.x),
                    Mathf.Clamp(targetCamPosition.y, minValueWRLD2.y, maxValueWRLD2.y),
                    Mathf.Clamp(targetCamPosition.z, minValueWRLD2.z, maxValueWRLD2.z)
                    );
                    break;
                case 6:
                    //World 2-3
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueBTWN23.x, maxValueBTWN23.x),
                    Mathf.Clamp(targetCamPosition.y, minValueBTWN23.y, maxValueBTWN23.y),
                    Mathf.Clamp(targetCamPosition.z, minValueBTWN23.z, maxValueBTWN23.z)
                    );
                    break;
                case 7:
                    //World 3
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueWRLD3.x, maxValueWRLD3.x),
                    Mathf.Clamp(targetCamPosition.y, minValueWRLD3.y, maxValueWRLD3.y),
                    Mathf.Clamp(targetCamPosition.z, minValueWRLD3.z, maxValueWRLD3.z)
                    );
                    break;
                case 8:
                    //World 3-4
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueBTWN34.x, maxValueBTWN34.x),
                    Mathf.Clamp(targetCamPosition.y, minValueBTWN34.y, maxValueBTWN34.y),
                    Mathf.Clamp(targetCamPosition.z, minValueBTWN34.z, maxValueBTWN34.z)
                    );
                    break;
                case 9:
                    //World 4
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueWRLD4.x, maxValueWRLD4.x),
                    Mathf.Clamp(targetCamPosition.y, minValueWRLD4.y, maxValueWRLD4.y),
                    Mathf.Clamp(targetCamPosition.z, minValueWRLD4.z, maxValueWRLD4.z)
                    );
                    break;
                default:
                    //World 1 by default
                    boundPosition = new Vector3(
                    Mathf.Clamp(targetCamPosition.x, minValueWRLD1.x, maxValueWRLD1.x),
                    Mathf.Clamp(targetCamPosition.y, minValueWRLD1.y, maxValueWRLD1.y),
                    Mathf.Clamp(targetCamPosition.z, minValueWRLD1.z, maxValueWRLD1.z)
                    );
                    break;
            }
            
            transform.position = Vector3.Lerp(transform.position, boundPosition, smooth * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }

    public void UpdateSceneCanvas()
    {
        Canvas[] c = FindObjectsOfType<Canvas>();
        foreach (Canvas c2 in c)
        {
            c2.worldCamera = this.GetComponent<Camera>();
            c2.sortingLayerName = "UI";
        }
    }

    public void LookForPlayer()
    {
        target = TeamClass.instance.GetAliveHeroes()[0].transform;
        EncounterManager.instance.player = TeamClass.instance.GetAliveHeroes()[0].gameObject;
        FixThePlayer();
    }

    public void StopFollowPlayer()
    {
        target = null;
    }

    public void FixThePlayer()
    {
        float oldSmooth = smooth;
        smooth = 0;
        transform.position = new Vector3(target.position.x, target.position.y, -10);
        smooth = oldSmooth;
    }


}
