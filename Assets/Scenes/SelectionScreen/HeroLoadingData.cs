using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLoadingData : MonoBehaviour
{
    public static string[] heroLoadingData;
    public static readonly int NumberOfHeroes = 6;

    public void Start()
    {
        heroLoadingData = new string[NumberOfHeroes];
    }
}
