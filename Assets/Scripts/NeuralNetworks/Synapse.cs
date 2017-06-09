using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class Synapse
    {
        private Neuron sourceNeuron;
        private Neuron destinationNeuron;
        private double weight;

        public Synapse(Neuron sourceNeuron, Neuron destinationNeuron, double weight)
        {
            this.sourceNeuron = sourceNeuron;
            this.destinationNeuron = destinationNeuron;
            this.weight = weight;
        }


        public Neuron GetSource()
        {
            return this.sourceNeuron;
        }

        public Neuron GetDestination()
        {
            return this.destinationNeuron;
        }

        public double getWeight()
        {
            return this.weight;
        }

        public void SetWeight(double weight)
        {
            this.weight = weight;
        }

        public void SetSourceNeuron(Neuron sourceNeuron)
        {
            this.sourceNeuron = sourceNeuron;
        }

        public void SetDestinationNeuron(Neuron destinationNeuron)
        {
            this.destinationNeuron = destinationNeuron;
        }
    }
}