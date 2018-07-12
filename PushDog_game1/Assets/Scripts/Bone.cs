using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

    public AudioSource audioSource_bone;

    public bool m_OnHome;
    public Sprite heart;

    public void Awake()
    {
        audioSource_bone = GetComponent<AudioSource>();
    }

    public bool Move(Vector2 direction)
    {
        if (BoxBlocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            TestForOnHome();
            return true;
        }
    }

    bool BoxBlocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPos = new Vector2(position.x, position.y) + direction;

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            if (wall.transform.position.x == newPos.x && wall.transform.position.y == newPos.y)
            {
                return true;
            }
        }

        GameObject[] bones = GameObject.FindGameObjectsWithTag("Bone");
        foreach (var bone in bones)
        {
            if (bone.transform.position.x == newPos.x && bone.transform.position.y == newPos.y)
            {
                return true;
            }
        }
        return false;
    }

    public void TestForOnHome()
    {
        GameObject[] homes = GameObject.FindGameObjectsWithTag("Home");
        foreach (var home in homes)
        {
            if (transform.position.x == home.transform.position.x && transform.position.y == home.transform.position.y)
            {
                GetComponent<SpriteRenderer>().sprite = heart;

                audioSource_bone.Play();

                m_OnHome = true;

                return;

            }
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        m_OnHome = false;
    }
}
