using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class Network
    {
        private System.Random randomGen = new System.Random();
        private List<Neuron> sensors = new List<Neuron>();
        private List<Neuron> iNodes = new List<Neuron>();
        private List<Neuron> oNodes = new List<Neuron>();
        private List<Neuron> ioNodes = new List<Neuron>();

        private Neuron actuator = new Neuron();

        public Network()
        {
            oNodes.Add(actuator);
        }

        public double Propagate()
        {
            NetworkResources.visitedNeurons.Clear();
            return actuator.Activate();
        }

        public Network Copy()
        {
            /* Needed for breadth first graph traversal */
            Queue<Neuron> queue = new Queue<Neuron>();
            Dictionary<Neuron, Neuron> map = new Dictionary<Neuron, Neuron> ();

            /* Cloned network */
            Network copyNetwork = new Network();

            /* We will start traversal from the actuator */
            Neuron originalActuator = this.actuator;

            Neuron copyActuator = new Neuron();
            copyNetwork.SetActuator(copyActuator);

            /* Add the first in line for processing, the original actuator */
            queue.Enqueue(originalActuator);
            map.Add(originalActuator, copyActuator);


            List<Neuron> copyioNodes = new List<Neuron>();
            List<Neuron> copyiNodes = new List<Neuron>();
            List<Neuron> copyoNodes = new List<Neuron>();
            List<Neuron> copySensors = new List<Neuron>();

            foreach (Neuron s in sensors)
            {
                Neuron temp = new Neuron();
                map.Add(s, temp);
                queue.Enqueue(s);
                copySensors.Add(temp);
                copyiNodes.Add(temp);
            }

            while (queue.Count > 0)
            {
                /* Node we are currently processing */
                Neuron originalNode = queue.Dequeue();

                List<Synapse> inputSynapses = originalNode.GetInputs();
                List<Synapse> outputSynapses = originalNode.GetOutputs();

                /* Clone of the original node that is being processed */
                Neuron copyNode = map[originalNode];

                foreach (Synapse inputSynapse in inputSynapses)
                {
                    /* Get the real input node */
                    Neuron inputOriginal = inputSynapse.GetSource();

                    /* If the copy of the inputOriginal doesn't exist, create it. Otherwise just connect */
                    if (!map.ContainsKey(inputOriginal))
                    {
                        Neuron inputCopy = new Neuron();
                        copyioNodes.Add(inputCopy);
                        Synapse synapseCopy = new Synapse(inputCopy, copyNode, inputSynapse.getWeight());
                        inputCopy.AddOutput(synapseCopy);
                        copyNode.AddInput(synapseCopy);
                        map.Add(inputOriginal, inputCopy);
                        queue.Enqueue(inputOriginal);
                    }
                    else
                    {
                        /* Get the copy of the real node since it exists */
                        Neuron inputCopy = map[inputOriginal];

                        bool alreadyExists = false;
                        foreach (Synapse n in inputCopy.GetOutputs())
                        {
                            if (n.GetSource().Equals(inputCopy) && n.GetDestination().Equals(copyNode))
                            {
                                alreadyExists = true;
                                break;
                            }
                        }
                        if (!alreadyExists)
                        {
                            /* Create input synapse between cloned nodes */
                            Synapse copySynapse = new Synapse(inputCopy, copyNode, inputSynapse.getWeight());
                            copyNode.AddInput(copySynapse);
                            inputCopy.AddOutput(copySynapse);
                        }
                    }
                }
                foreach (Synapse outputSynapse in outputSynapses)
                {
                    /* Get the real output node */
                    Neuron outputOriginal = outputSynapse.GetDestination();

                    /* If the copy of the outputOriginal doesn't exist, create it. Otherwise just connect */
                    if (!map.ContainsKey(outputOriginal))
                    {
                        Neuron outputCopy = new Neuron();
                        copyioNodes.Add(outputCopy);
                        Synapse syn = new Synapse(copyNode, outputCopy, outputSynapse.getWeight());
                        copyNode.AddOutput(syn);
                        outputCopy.AddInput(syn);
                        map.Add(outputOriginal, outputCopy);
                        queue.Enqueue(outputOriginal);
                    }
                    else
                    {
                        /* Get the copy of the real node since it exists */
                        Neuron outputCopy = map[outputOriginal];

                        bool alreadyExists = false;
                        foreach (Synapse n in outputCopy.GetInputs())
                        {
                            if (n.GetSource().Equals(copyNode) && n.GetDestination().Equals(outputCopy))
                            {
                                alreadyExists = true;
                                break;
                            }
                        }
                        if (!alreadyExists)
                        {
                            /* Create output synapse between cloned nodes */
                            Synapse copySynapse = new Synapse(copyNode, outputCopy, outputSynapse.getWeight());
                            copyNode.AddOutput(copySynapse);
                            outputCopy.AddInput(copySynapse);
                        }
                    }

                }
            }

            /* Set newly created sensors */
            copyNetwork.SetSensors(copySensors);

            /* Within cloned network, remove sensors from ioNodes, add to iNodes*/
            //for (Neuron copySensor : copySensors) {
            //   copyioNodes.remove(copySensor);
            //   copyiNodes.add(copySensor);
            // }

            /* Add all nodes (except sensors, and actuator) to iNodes and oNodes */
            foreach (Neuron ioNode in copyioNodes)
            {
                copyiNodes.Add(ioNode);
                copyoNodes.Add(ioNode);
            }

            /* Add actuator to oNodes */
            copyoNodes.Add(copyActuator);

            copyNetwork.SetInputNodes(copyiNodes);
            copyNetwork.SetOutputNodes(copyoNodes);
            copyNetwork.SetInputOutputNodes(copyioNodes);

            // Crap code ahead ================================ WOO HOO WATCH ME ==========================================
            // TODO: Erase this souts once we create a few working networks
            //        System.out.println("Length of ioNodes " + copyioNodes.size());
            //        System.out.println("Length of iNodes  " + copyiNodes.size() + " it should be ioNodes + 3");
            //        System.out.println("Length of oNodes  " + copyoNodes.size() + " it should be ioNodes + 1");
            //        System.out.println(copyNetwork.actuator);
            //        System.out.println(copyActuator);
            //        System.out.println(copyActuator.weightedSum);
            //        for (Neuron n : copyNetwork.iNodes)
            //        {
            //            System.out.println(n.weightedSum);
            //            System.out.println(n.getOutputs());
            //            System.out.println(n.getInputs());
            //        }
            // Crap code finished ============================= WOO HOO WATCH ME ==========================================

            return copyNetwork;
        }

        public void SetInputValues(double[] input)
        {
            if (input.Length != sensors.Count)
            {
                Debug.LogError("Array size of neural network input values must be the same number of input sensors!");
            }

            int i = 0;
            foreach (Neuron sensor in sensors)
            {
                sensor.SetOutput(input[i]);
                i++;
            }
        }

        public void MutateAddNeuron()
        {
            Neuron temp = new Neuron();

            Neuron source = GetRandomNeuron(iNodes);
            Synapse synIn = new Synapse(source, temp, randomGen.NextDouble());
            source.AddOutput(synIn);
            temp.AddInput(synIn);

            Neuron destination = GetRandomNeuron(oNodes);
            Synapse synOut = new Synapse(temp, destination, randomGen.NextDouble());
            destination.AddInput(synOut);
            temp.AddOutput(synOut);

            iNodes.Add(temp);
            oNodes.Add(temp);
            ioNodes.Add(temp);
        }

        public void MutateSplice()
        {
            Neuron source = GetRandomNeuron(iNodes);
            Synapse sourceSynapse = GetRandomSynapse(source.GetOutputs());
            Neuron destination = sourceSynapse.GetDestination();

            Neuron temp = new Neuron();

            sourceSynapse.SetDestinationNeuron(temp);

            destination.GetInputs().Remove(sourceSynapse);

            Synapse tempSynapse = new Synapse(temp, destination, randomGen.NextDouble());

            temp.GetOutputs().Add(tempSynapse);
            destination.GetInputs().Add(tempSynapse);

            iNodes.Add(temp);
            oNodes.Add(temp);
            ioNodes.Add(temp);
        }

        public void MutateAddSynapse()
        {

            Neuron source = GetRandomNeuron(iNodes);
            Neuron dest = source;
            while (dest.Equals(source))
            {
                dest = GetRandomNeuron(oNodes);
            }

            Synapse syn = new Synapse(source, dest, randomGen.NextDouble());
            source.AddOutput(syn);
            dest.AddInput(syn);
        }

        public void MutateRemoveNeuron()
        {
            if (ioNodes.Count != 0)
            {
                Neuron temp = GetRandomNeuron(ioNodes);
                foreach (Synapse syn in temp.GetInputs())
                {
                    syn.GetSource().GetOutputs().Remove(syn);
                }
                foreach (Synapse syn in temp.GetOutputs())
                {
                    syn.GetDestination().GetInputs().Remove(syn);
                }

                temp.GetInputs().Clear();
                temp.GetOutputs().Clear();

                oNodes.Remove(temp);
                iNodes.Remove(temp);
                ioNodes.Remove(temp);
            }
        }

        public void MutateChangeWeights()
        {
            if (ioNodes.Count != 0)
            {
                Neuron temp = GetRandomNeuron(ioNodes);
                foreach (Synapse s in temp.GetInputs())
                {
                    s.SetWeight(randomGen.NextDouble());
                }
            }
        }

        public void Mutate()
        {
            if (randomGen.NextDouble() > 0.4)
            {
                int mutationType = randomGen.Next(4);
                if (mutationType == 0)
                    MutateAddNeuron();
                else if (mutationType == 1)
                    MutateRemoveNeuron();
                else if (mutationType == 2)
                    MutateChangeWeights();
                else if (mutationType == 3)
                    MutateAddSynapse();
            }
            MutateChangeWeights();
        }

        public Neuron GetRandomNeuron(List<Neuron> neuronList)
        {
            int index = randomGen.Next(neuronList.Count);
            return neuronList[index];
        }

        public Synapse GetRandomSynapse(List<Synapse> synapseList)
        {
            int index = randomGen.Next(synapseList.Count);
            return synapseList[index];
        }

        public void SetInputNodes(List<Neuron> iNodes)
        {
            this.iNodes = iNodes;
        }

        public void SetOutputNodes(List<Neuron> oNodes)
        {
            this.oNodes = oNodes;
        }

        public void SetInputOutputNodes(List<Neuron> ioNodes)
        {
            this.ioNodes = ioNodes;
        }

        public void SetSensors(List<Neuron> sensors)
        {
            foreach (Neuron sensor in sensors)
            {
                this.iNodes.Add(sensor);
                this.sensors.Add(sensor);
            }
        }

        public void SetActuator(Neuron actuator)
        {
            this.actuator = actuator;
            this.oNodes.Add(actuator);
        }

    }
}