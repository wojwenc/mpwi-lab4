using Lab4.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4 {
    class Program {
        static void Main(string[] args) {
            Generator generator = new Generator();

            generator
                .GenerateValues()
                .PrintValues()
                .CalcDistribution()
                .PrintDistribution();

            Console.Read();
        }
    }
}
