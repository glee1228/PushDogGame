using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    AudioSource audioSource_player;
    public AudioClip notmovesound;
    public AudioClip movesound;
    public AudioClip nonesound;


    public void Awake()
    {
        audioSource_player = GetComponent<AudioSource>();

    }

    public bool Move(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }

        direction.Normalize();

        if (Blocked(transform.position, direction))
        {
            GetComponent<AudioSource>().clip = notmovesound;
            audioSource_player.Play();
            return false;
        }
        else
        {
            transform.Translate(direction);
            GetComponent<AudioSource>().clip = movesound;
            audioSource_player.Play();
            return false;
        }
    }

    bool Blocked(Vector3 position, Vector3 direction)
    {
        Vector3 newPos = new Vector3(position.x, position.y)+ direction;

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
                Bone bn = bone.GetComponent<Bone>();
                if (bn && bn.Move(direction))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
}
