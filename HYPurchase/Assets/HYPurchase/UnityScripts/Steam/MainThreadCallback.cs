using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadCallback : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(RunCallback());
    }

    private IEnumerator RunCallback()
    {
        while (true)
        {
            Steamworks.SteamAPI.RunCallbacks();

            yield return new WaitForSeconds(0.5f);
        }
    }
}
