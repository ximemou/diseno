using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.FixtureAlgorithm
{
    public class ConvertFixtureAlgorithm
    {
        public IFixtureAlgorithm ConvertAlgorithm(string algorithmAsString)
        {

            if (string.Equals(algorithmAsString, "Todos contra todos"))
            {
                IFixtureAlgorithm algorithm = new AllAgainstAll();
                return algorithm;
            }
            else
            {

                if(string.Equals(algorithmAsString, "Al azar"))
                {
                    IFixtureAlgorithm algorithm = new RandomAlgorithm();
                    return algorithm;
                }
                else
                {
                    throw new ArgumentException("No existe el algortimo ingresado.");
                }           
            }


        }


    }
}
