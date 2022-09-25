using System;
using System.Collections.Generic;
using System.Linq;

namespace ATMDenominations
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<List<int>> cartridgeCombinations = GetCartridgeCombinations();
            List<string> combinations = new List<string>();
            while (true)
            {
                int withdrawlAmount = int.Parse(Console.ReadLine());
                combinations.Clear();
                cartridgeCombinations.ForEach(cartridgeCombination =>
                {
                   var result=  GenerateDenominationCombination(cartridgeCombination.OrderByDescending(cartridge => cartridge).ToList(), withdrawlAmount);
                    if(!string.IsNullOrEmpty(result) && !combinations.Contains(result))
                    {
                        combinations.Add(result);
                    }
                });

                //Additonal Combinations
                cartridgeCombinations.ForEach(cartridgeCombination =>
                {
                    var result = GenerateAdditionalDenominationCombination(cartridgeCombination.OrderByDescending(cartridge => cartridge).ToList(), withdrawlAmount);
                    if(result != null && result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            if (!string.IsNullOrEmpty(item) && !combinations.Contains(item))
                            {
                                combinations.Add(item);
                            }
                        }
                    }
                  
                });

                Console.Write("Possible Combinations of : {0}\n", withdrawlAmount);
                combinations.ForEach(i => Console.Write("{0}\n", i));
            }

        }

        private static List<List<int>> GetCartridgeCombinations()
        {
            return new List<List<int>> { new List<int> { 10, 50, 100 }, new List<int> { 10, 50 }, new List<int> { 50, 100 }, new List<int> { 10, 100 }, new List<int> { 10 }, new List<int> { 50 }, new List<int> { 100 } };
        }

        private static string GenerateDenominationCombination(List<int> currencyNotes, int withdrawlAmount)
        {
            Dictionary<int, int> denominationCombinations = new Dictionary<int, int>();
            string finalResult = string.Empty;
            int remainingAmountToWithdraw = withdrawlAmount;
            currencyNotes.ForEach(note =>
            {
                int notesCount = remainingAmountToWithdraw / note;
                if (notesCount > 0)
                {
                    denominationCombinations[note] = notesCount;
                    remainingAmountToWithdraw = remainingAmountToWithdraw % note;
                }
            });

            if (remainingAmountToWithdraw == 0)
            {
                finalResult = string.Empty;
                denominationCombinations.ToList().ForEach(_ =>
                {
                    finalResult += $"{_.Key} X {_.Value} EUR";

                    if (denominationCombinations.Last().Key != _.Key)
                    {
                        finalResult += " + ";
                    }
                });

            }

            return finalResult;
        }

        private static List<string> GenerateAdditionalDenominationCombination(List<int> currencyNotes, int withdrawlAmount)
        {
            List<string> denominationCombinations = new List<string>();
            string finalResult = string.Empty;
            string valueToInsert = string.Empty;
            int remainingAmountToWithdraw = withdrawlAmount;
            int totalCurrencyNotes = currencyNotes.Count;
            int loopCurrencyCodeCount = 1;
            currencyNotes.ForEach(note =>
            {
                if (loopCurrencyCodeCount == totalCurrencyNotes && remainingAmountToWithdraw != 0)
                {
                    int i = 1;
                    while (remainingAmountToWithdraw >= note)
                    {
                        valueToInsert = $"{note} X {i} EUR";
                        remainingAmountToWithdraw = remainingAmountToWithdraw - note;
                        i = i + 1;
                    }
                    if (!string.IsNullOrEmpty(valueToInsert))
                    {
                        if (finalResult.Length == 0)
                        {
                            finalResult += $"{note} X {i - 1} EUR";
                        }
                        else
                        {
                            finalResult += $" + {note} X {i - 1} EUR";
                        }

                    }
                }
                else
                {
                    loopCurrencyCodeCount = loopCurrencyCodeCount + 1;
                    if (remainingAmountToWithdraw >= note)
                    {
                        int notesCount = remainingAmountToWithdraw - note;
                        if (notesCount >= 0)
                        {
                            remainingAmountToWithdraw = remainingAmountToWithdraw - note;
                            if (finalResult.Length == 0)
                            {
                                finalResult += $"{note} X {1} EUR";
                            }
                            else
                            {
                                finalResult += $" + {note} X {1} EUR";
                            }
                        }
                    }
                }

            });

            if (!string.IsNullOrEmpty(finalResult) && remainingAmountToWithdraw == 0)
            {
                denominationCombinations.Add(finalResult);
            }

            return denominationCombinations;
        }
    }
}
