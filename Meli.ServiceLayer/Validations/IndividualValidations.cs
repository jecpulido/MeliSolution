using Meli.Common.Entities;
using Meli.Common.Enum;
using Meli.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Meli.ServiceLayer.Validations
{
    public class IndividualValidations
    {

        public static readonly Func<Individual, Dictionary<ValidationType, bool>>[] validationsPro = {
            (individual) => new Dictionary<ValidationType, bool>(){ {ValidationType.Request , individual != null } },
            (individual) => new Dictionary<ValidationType, bool>(){ {ValidationType.Request ,individual.Dna != null && individual.Dna.Count > 0 } },
            (individual) => new Dictionary<ValidationType, bool>(){ {ValidationType.Characters , IsValidateString(individual) } },
            (individual) => IsMutant(individual),
        };

        public static bool IsValidateString(Individual individual)
        {
            string pattern = @"[ACGT]+";
            string str = DnaTools.GenerateDnaSecuence(individual.Dna);
            return Regex.Replace(str, pattern, "").Trim().Length == 0;
        }

        public static Dictionary<ValidationType, bool> IsMutant(Individual individual)
        {
            Dictionary<ValidationType, bool> validations = new Dictionary<ValidationType, bool>();
            int row = individual.Dna.Count;
            int col = individual.Dna[0].Length;

            if ((row < 4 || col < 4) || ( row != col ))
            {
                validations.Add(ValidationType.Symmetrical, false);
                return validations;
            }

            string[,] dna = new string[row, col];
            for (int i = 0; i < dna.GetLength(0); i++)
            {
                if (individual.Dna[i].Length != col)
                {
                    validations.Add(ValidationType.Symmetrical, false);
                    return validations;
                }

                for (int j = 0; j < dna.GetLength(1); j++)
                {
                    dna[i, j] = individual.Dna[i][j].ToString();
                }
            }

            validations.Add(ValidationType.Mutant, IsMutant(dna));

            return validations;
        }

        private static bool IsMutant(string[,] dna)
        {
            int secuenceVer = 0, secuenceHor = 0, secuenceDiag = 0, secuenceDiagI = 0, secuence = 0;

            for (int i = 0; i < dna.GetLength(0); i++)
            {
                string letterVer = dna[0, i];
                string letterHor = dna[i, 0];
                for (int j = 0; j < dna.GetLength(1); j++)
                {
                    if (j + 1 < dna.GetLength(0) && letterHor == dna[i, j + 1])
                    {
                        secuenceHor++;
                        if (secuenceHor >= 3)
                        {
                            secuence++;
                            secuenceHor = 0;
                        }
                    }
                    else
                    {
                        if (j + 1 < dna.GetLength(0))
                            letterHor = dna[i, j + 1];
                        else
                            letterHor = dna[i, j];
                        secuenceHor = 0;
                    }

                    if (j + 1 < dna.GetLength(1) && letterVer == dna[j + 1, i])
                    {
                        secuenceVer++;
                        if (secuenceVer >= 3)
                        {
                            secuence++;
                            secuenceVer = 0;
                        }
                    }
                    else
                    {
                        if (j + 1 < dna.GetLength(1))
                            letterVer = dna[j + 1, i];
                        else
                            letterVer = dna[j, i];
                        secuenceVer = 0;
                    }
                }
            }
            if (secuence > 1)
                return true;
            
            int height = dna.GetLength(0), width = dna.GetLength(1), auxwidth = dna.GetLength(1) - 1;
            string letterDiag = "", letterDiagI = "";

            for (int diag = 1 - width; diag <= height - 1; diag += 1)
            {
                for (int vertical = Math.Max(0, diag), horizontal = -Math.Min(0, diag);
                    vertical < height && horizontal < width ;
                    vertical += 1, horizontal += 1)
                {
                    if (string.IsNullOrEmpty(letterDiag))
                    {
                        letterDiag = dna[vertical, horizontal];
                    }

                    if (horizontal + 1 < dna.GetLength(0) && vertical + 1 < dna.GetLength(1) && letterDiag == dna[vertical + 1, horizontal + 1])
                    {
                        secuenceDiag++;
                        if (secuenceDiag >= 3)
                        {
                            secuence++;
                            secuenceDiag = 0;
                        }
                    }
                    else
                    {
                        if (horizontal + 1 < dna.GetLength(0) && vertical + 1 < dna.GetLength(1))
                            letterDiag = dna[vertical + 1, horizontal + 1];
                        else
                            letterDiag = dna[vertical, horizontal ];
                        secuenceDiag = 0;
                    }
                    //Diagonal invertida
                    if (string.IsNullOrEmpty(letterDiagI))
                    {
                        letterDiagI = dna[vertical, auxwidth - horizontal];
                    }

                    if (horizontal + 1 < dna.GetLength(0) && vertical + 1 < dna.GetLength(1) && letterDiagI == dna[vertical + 1, auxwidth - (horizontal + 1)])
                    {
                        secuenceDiagI++;
                        if (secuenceDiagI >= 3)
                        {
                            secuence++;
                            secuenceDiagI = 0;
                        }
                    }
                    else
                    {
                        if (horizontal + 1 < dna.GetLength(0) && vertical + 1 < dna.GetLength(1))
                            letterDiagI = dna[vertical + 1, auxwidth - (horizontal + 1)];
                        else
                            letterDiagI = dna[vertical, auxwidth - horizontal];
                        secuenceDiagI = 0;
                    }
                }
                letterDiag = "";
                letterDiagI = "";
                if (secuence > 1)
                    return true;
            }

            return secuence > 1;
        }

    }
}
