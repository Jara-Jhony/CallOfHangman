﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Player {

    [SerializeField]
    private string[] words;
    [SerializeField]
    private AnimationCurve behaviourDistribution;

    //Char list
    private List<char> vocals;
    private List<char> commonConsonants;
    private List<char> unusualConsonants;

    private float behaviourFactor;
    private float behaviourGap = 4.0f;

    public void SelectWord()
    {
        word = words[Random.Range(0, words.Length)];
    }

    public void DoMove()
    {
        int playableTurn = (int)(GameManager.Singleton.turn * 0.5) - 1;

        behaviourFactor = playableTurn + Random.Range(-behaviourGap, behaviourGap) * behaviourFactor;

        if (behaviourFactor >= -behaviourGap && behaviourFactor <= behaviourGap)
            PlayVocal();
        else if (behaviourFactor >= 0 && behaviourFactor <= 2 * behaviourGap)
            PlayCommonConsonant();
        else if (behaviourFactor >= behaviourGap && behaviourFactor <= 3 * behaviourGap)
            PlayUnusualConsonant();
    }

    public void PlayVocal()
    {
        //TODO
    }

    public void PlayCommonConsonant()
    {
        //TODO
    }

    public void PlayUnusualConsonant()
    {
        //TODO
    }
}
