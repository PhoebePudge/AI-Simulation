
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

//I based my gene structure from this tutorial
//https://medium.com/analytics-vidhya/genetic-algorithm-in-unity-using-c-72f0fafb535c

namespace Genes {
    [System.Serializable]
    public class DNA {

        public readonly bool isMale;
        int[] target = new int[] { 0, 1, 2, 1, 0, 1, 2, 1, 0, 1, 2 };

        public int[] gene;
        public float fitness;
        public float mutationRate = 0.01f;

        private System.Random rng;

        public DNA(int count) {
            rng = new System.Random();


            isMale = (float)rng.NextDouble() < 0.5f;

            

            gene = new int[count];

            for (int i = 0; i < count; i++) {
                gene[i] = Random.Range(0, 4);
            }
        }
        //Cross over our gene (Splice a gene to make a new one)
        public DNA crossOver(DNA partner) {
            DNA child = new DNA(target.Length);
            int midpt = Random.Range(0, target.Length);
            for (int i = 0; i < target.Length; i++) {
                //splice between this and partner gene at the midpoint
                child.gene[i] = i <= midpt ? this.gene[i] : partner.gene[i];
            }

            return child;
        }
        //Mutate our gene
        public void Mutate() {
            for (int i = 0; i < gene.Length; i++) {
                if (rng.NextDouble() < mutationRate) {
                    if (i == target.Length - 1) {
                        gene[i] = gene[0];
                        gene[0] = gene[i];
                    } else {
                        gene[i] = gene[i + 1];
                        gene[i + 1] = gene[i];
                    }
                }
            }
        }
        //Translate our data to a string for debugging
        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < gene.Length; i++)
            {
                output += gene[i].ToString();

                if (i > gene.Length - 1)
                {
                    output += "\n";
                }
            }
            return output;
        }
    }
}