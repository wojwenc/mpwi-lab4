using Lab3.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Classes {
    class Generator : ICalcDistribution, IGenerateValues, IPrintDistribution, IPrintValues {
        private const int nDist = 4; // dimensions of the distribution array
        private const int numCount = 100000; //number of random numbers to generate

        private double[,] probabilities = new double[nDist, nDist] { { 0, 0, 0.1, 0.1 }, { 0.2, 0, 0, 0.2 },
                                                    { 0, 0, 0.3, 0 }, { 0, 0.05, 0, 0.05 } };

        private double[,] yProbabilities = new double[nDist, nDist];
        private double xProbability = 1.0 / nDist;
        private int[,] distribution = new int[nDist, nDist];

        private Random rand; //random number generator

        private List<Tuple<int, int>> generatedValues;

        public Generator() {
            // calculate probabilites for values on the Y axis
            for (int i = 0; i < nDist; i++) {
                // sum all the values in the row
                double rowSum = 0.0;
                for (int j = 0; j < nDist; j++) {
                    rowSum += probabilities[i, j];
                }

                // calculate probability for each number int the row
                for (int j = 0; j < nDist; j++) {
                    yProbabilities[i, j] = probabilities[i, j] / rowSum;
                }
            }

            // initialize the random number generator
            rand = new Random();

            generatedValues = new List<Tuple<int, int>>();
        }

        // get a pair of random numbers
        private Tuple<int, int> GetRandomNumber() {
            // get a random floating point number between 0 and 1
            double randNum = rand.NextDouble();
            int X = 0, Y = 0; // random values to be generated

            // sum of all previous intervals
            double prevIntervalSum = 0.0;
            // check in which interval the random number is to determine the first returned value
            for (int i = 1; i <= nDist; i++) {
                if (randNum < prevIntervalSum + xProbability) {
                    X = i;
                    break;
                } else {
                    prevIntervalSum += xProbability;
                }
            }
            // get a new random number
            randNum = rand.NextDouble();

            // sum of all previous intervals
            prevIntervalSum = 0.0;
            // check in which interval the random number is to determine the second returned value
            for (int i = 1; i <= nDist; i++) {
                if (randNum < prevIntervalSum + yProbabilities[X - 1, i - 1]) {
                    Y = i;
                    break;
                } else {
                    prevIntervalSum += yProbabilities[X - 1, i - 1];
                }
            }

            // return the pair of random numbers
            return Tuple.Create(X, Y);
        }

        // generate numCount random pairs and save them to a list
        public Generator GenerateValues() {
            generatedValues.Clear();

            for (int i = 0; i < numCount; i++) {
                // get a new random number
                Tuple<int, int> randNum = GetRandomNumber();

                // store the generated value
                generatedValues.Add(randNum);
            }

            return this;
        }

        // print the previously generated pairs
        public Generator PrintValues() {
            foreach (Tuple<int, int> randNum in generatedValues) {
                distribution[randNum.Item1 - 1, randNum.Item2 - 1]++;
            }

            return this;
        }

        // calculate how many pairs were generated in each range
        public Generator CalcDistribution() {
            foreach (Tuple<int, int> randNum in generatedValues) {
                Console.WriteLine("(" + randNum.Item1 + ", " + randNum.Item2 + ")");
            }

            return this;
        }

        // print how many pairs were generated in each range
        public Generator PrintDistribution() {
            Console.WriteLine("Distribution:");

            for (int i = 1; i <= nDist; i++) {
                for (int j = 1; j <= nDist; j++) {
                    Console.WriteLine("(" + i + ", " + j + "): " + distribution[i - 1, j - 1]);
                }
            }

            return this;
        }
    }
}
