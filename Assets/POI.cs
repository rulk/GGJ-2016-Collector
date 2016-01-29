using UnityEngine;
using System.Collections;

public class POI : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Collector.s_myCollector != null)
        {
            Collector.s_myCollector.addPOI(this);
        }
	
	}
}
