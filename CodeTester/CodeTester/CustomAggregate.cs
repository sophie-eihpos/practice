﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CodeTester
{
    [TestClass]
    public class CustomAggregate
    {
        [TestMethod]
        public void WithSeedValue()
        {
            var names = new[] { "Sarah", "Gentry", "Amrit" };

            //integer values for the first letter in each name
            var a = (int)names[0][0];
            var b = (int)names[1][0];
            var c = (int)names[2][0];
            var total = a + b + c;

            var resultWithSeedZero = names.Aggregate(0,
                    (runningTotal, name) => runningTotal + (int)name[0]);

            var resultWithSeedHundred = names.Aggregate(100,
                    (runningTotal, name) => runningTotal + (int)name[0]);
        }

        [TestMethod]
        public void NoSeedValue()
        {
            var nums = new[] {1, 2, 3, 4};

            var resultWithSeed = nums.Aggregate(0, CustomAccumulationFunction);

            // the first value in the sequence is used as seed
            var resultWithNoSeed = nums.Aggregate(CustomAccumulationFunction);
        }

        private int CustomAccumulationFunction(int runningTotal, int num)
        {
            if(num%2 == 0)
            {
                return runningTotal + num;
            }

            return runningTotal;
        }
    }
}
