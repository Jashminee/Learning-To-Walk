using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DudeManager : MonoBehaviour
{
    List<TriggerDetector> detectors = new List<TriggerDetector>();
    List<Jumper> jumpers = new List<Jumper>();

    List<DudeInput> inputs = new List<DudeInput>();
    List<DudeOutput> outputs = new List<DudeOutput>();

    System.Random rand = new System.Random();
    private void Awake()
    {
        detectors = Resources.FindObjectsOfTypeAll<TriggerDetector>().ToList();
        jumpers = Resources.FindObjectsOfTypeAll<Jumper>().ToList();

    }

    private void Update()
    {
       //int detectorNumber = rand.Next(0, detectors.Count - 1);
       //int jumperNumber = rand.Next(0, jumpers.Count - 1);
       //
       //int detectorEable = rand.Next(0, 2);
       //int detectorEable = rand.Next(0, 2);
    }

    public class DudeInput
    {
        public TriggerDetector detector;
        public State state;

        public enum State
        {
            On,
            Off
        }

        public DudeInput(TriggerDetector detector)
        {
            this.detector = detector;
            this.state = State.Off;

            detector.OnStartCollideWithGround += EnterGroundEventHandler;
            detector.OnEndCollideWithGround += ExitGroundEventHandler;
        }

        public void EnterGroundEventHandler(Collider2D collider)
        {
            state = State.On;
        }

        public void  ExitGroundEventHandler(Collider2D collider)
        {
            state = State.Off;
        }
    }

    public class DudeOutput
    {
        public Jumper jumper;

        public DudeOutput(Jumper jumper)
        {
            this.jumper = jumper;
        }

        public void Open(float value)
        {
            jumper.Open();
        }

        public void Close(float value)
        {
            jumper.Close();
        }
    }
}
