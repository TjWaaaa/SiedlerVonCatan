using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject villageBlue;
    public GameObject villageRed;
    public GameObject villageWhite;
    public GameObject villageYellow;

    public GameObject cityBlue;
    public GameObject cityRed;
    public GameObject cityWhite;
    public GameObject cityYellow;

    public GameObject roadBlue;
    public GameObject roadRed;
    public GameObject roadWhite;
    public GameObject roadYellow;



    public void BuildVillage(Vector3 position, Color playerColor)
    {

        if (playerColor.Equals(Color.blue))
        {
            Instantiate(villageBlue, position, Quaternion.identity);
        }
        else if (playerColor.Equals(Color.red))
        {
            Debug.Log("CLIENT" + villageRed.name);
            Instantiate(villageRed, position, Quaternion.identity);
        }
        else if (playerColor.Equals(Color.white))
        {
            Instantiate(villageWhite, position, Quaternion.identity);
        }
        else if (playerColor.Equals(Color.yellow))
        {
            Instantiate(villageYellow, position, Quaternion.identity);
        }
    }
    public void BuildCity(Vector3 position, Color playerColor)
    {

        if (playerColor.Equals(Color.blue))
        {
            Instantiate(cityBlue, position, Quaternion.identity);
        }
        else if (playerColor.Equals(Color.red))
        {
            Instantiate(cityRed, position, Quaternion.identity);
        }
        else if (playerColor.Equals(Color.white))
        {
            Instantiate(cityWhite, position, Quaternion.identity);
        }
        else if (playerColor.Equals(Color.yellow))
        {
            Instantiate(cityYellow, position, Quaternion.identity);
        }
    }

    public void BuildRoad(Vector3 position, Quaternion rotation, Color playerColor)
    {

        if (playerColor.Equals(Color.blue))
        {
            Instantiate(roadBlue, position, rotation);
        }
        else if (playerColor.Equals(Color.red))
        {
            Instantiate(roadRed, position, rotation);
        }
        else if (playerColor.Equals(Color.white))
        {
            Instantiate(roadWhite, position, rotation);
        }
        else if (playerColor.Equals(Color.yellow))
        {
            Instantiate(roadYellow, position, rotation);
        }
    }
}
