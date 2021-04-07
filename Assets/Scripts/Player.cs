using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    private string playerName;
    private Color color;

    private int points;

    private int bricks;
    private int ore;
    private int sheep;
    private int wheat;
    private int wood;

    public Player(string name, Color color) {
        this.playerName = name;
        this.color = color;
    }

    public Color GetColor() {
        return color;
    }

    public string GetName() {
        return playerName;
    }

    public void SetBricks(int amount) {
        bricks = amount;
        //bricksText.GetComponent<Text>().text = amount.ToString();
    }
    public void SetOre(int amount) {
        ore = amount;
        //oreText.GetComponent<Text>().text = amount.ToString();
    }
    public void SetSheep(int amount) {
        sheep = amount;
        //sheepText.GetComponent<Text>().text = amount.ToString();
    }
    public void SetWheat(int amount) {
        wheat = amount;
        //wheatText.GetComponent<Text>().text = amount.ToString();
    }
    public void SetWood(int amount) {
        wood = amount;
        //woodText.GetComponent<Text>().text = amount.ToString();
    }

    public int GetBricks() {
        return bricks;
    }
    public int GetOre() {
        return ore;
    }
    public int GetSheep() {
        return sheep;
    }
    public int GetWheat() {
        return wheat;
    }
    public int GetWood() {
        return wood;
    }
}
