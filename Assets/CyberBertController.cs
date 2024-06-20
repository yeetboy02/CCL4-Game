using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberBertController : MonoBehaviour {

    #region Parameters

    [SerializeField] private GameObject menu;

    [SerializeField] private string role;

    #endregion

    #region HandleInteraction

    void Interaction(bool start) {
        // CHECK ROLE TO HAVE RIGHT INTERACTION WITH PLAYER
        switch(role) {
            case "intro":
                Intro(start);
                break;
            case "moveToCity":
                MoveToCity(start);
                break;
        }
    
    }

    #endregion

    #region Introduction

    void Intro(bool start) {
        // ENABLE/DISABLE MENU
        menu.SetActive(start);
    }

    #endregion

    #region MoveToCity

    void MoveToCity(bool start) {
        // ENABLE/DISABLE MENU
        menu.SetActive(start);
    }

    #endregion

    #region Collision

    void OnTriggerEnter(Collider other) {
        // CHECK IF PLAYER
        if (other.CompareTag("Player")) {
            Interaction(true);
        }
    }

    void OnTriggerExit(Collider other) {
        // CHECK IF PLAYER
        if (other.CompareTag("Player")) {
            Interaction(false);
        }
    }

    #endregion
}
