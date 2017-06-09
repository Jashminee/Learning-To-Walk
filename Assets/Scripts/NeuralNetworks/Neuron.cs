using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class Neuron
    {
        private List<Synapse> inputs = new List<Synapse>();
        private List<Synapse> outputs = new List<Synapse>();
        double weightedSum;
        private double output;
        public Neuron()
        {
            weightedSum = 0.65;
            inputs = new List<Synapse>();
        }


        public double Activate()
        {
            weightedSum = 0;
            foreach (Synapse synapse in inputs)
            {
                if (!NetworkResources.visitedNeurons.ContainsKey(synapse.GetSource()))
                {
                    NetworkResources.visitedNeurons.Add(synapse.GetSource(), 1.0);
                    weightedSum += synapse.getWeight() * synapse.GetSource().Activate();
                }
            }
            if (inputs.Count == 0) return output;
            return Sigmoid(weightedSum);
        }

        public static double Sigmoid(double weight)
        {
            return 1.0 / (1 + Mathf.Exp(-1.0f * (float)weight));
        }

        public void AddInput(Synapse input)
        {
            inputs.Add(input);
        }

        public void AddOutput(Synapse output)
        {
            outputs.Add(output);
        }

        public List<Synapse> GetInputs()
        {
            return this.inputs;
        }

        public List<Synapse> GetOutputs()
        {
            return this.outputs;
        }

        public double GetOutput()
        {
            return this.output;
        }

        /* Use this for sensors */
        public void SetOutput(double output)
        {
            this.output = output;
        }
    }
}