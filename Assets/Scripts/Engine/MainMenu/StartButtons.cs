using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    public void EnterFluxScene()
    {
        SceneManager.LoadScene("Flux Simulator");
    }
    public void EnterParticleScene()
    {
        SceneManager.LoadScene("Particle Simulator");
    }
    public void EnterCircuitScene()
    {
        SceneManager.LoadScene("Circuit Simulator");
    }
}
