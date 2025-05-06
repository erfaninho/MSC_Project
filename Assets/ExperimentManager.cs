using System.Collections.Generic;
using UnityEngine;

// Define data structure i.e. what data you will collect from the participants
public class DataStructure
{   
    public List<float> xPos; 
    public List<float> yPos; 
    public List<float> force; 
    public List<float> time; 

    public DataStructure()
    {
        xPos = new List<float>(); 
        yPos = new List<float>(); 
        force = new List<float>(); 
    }

}

public class ExperimentManager : MonoBehaviour
{
    
    DataStructure datastruc = new DataStructure(); 

    // Define experimental conditions, trials and repetitions etc. 
    public string ptxID = "pilot_01";

    // Path to data folder i.e. where you store info and trial data etc. 
    public string path = ""; 


    // Variables to save 
    public Transform fingerPos; 

    void Start()
    {
        
    }

    void Update()
    {
        datastruc.xPos.Add(fingerPos.position.x); 
        datastruc.yPos.Add(fingerPos.position.z); 
    }


}
