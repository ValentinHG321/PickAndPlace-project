using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PickAndPlace
{
    internal class Program
    {

        static void Main()
        {
            Console.WriteLine("On first line write the names of the columns corresponding to the P&P file.");
            Console.WriteLine("Use the following abbreviations:");
            Console.WriteLine("Designator -> dr , Package -> pg , XAxis -> x , YAxis -> y , Rotation -> rn , PartName -> pn , Skip -> s");
            Console.WriteLine("Use \"s\" to skip the unwanted columns.");
            Console.WriteLine("They must be separated with \"-\".");
            Console.WriteLine("example: dr-s-s-s-pg-x-y-rn-pn");

            List<string> setup = Console.ReadLine().ToLower().Split('-', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (setup.Count < 6)
            {
                setup = Console.ReadLine().ToLower().Split('-', StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            int designator = setup.IndexOf("dr");
            int package = setup.IndexOf("pg");
            int xAxis = setup.IndexOf("x");
            int yAxis = setup.IndexOf("y");
            int rotation = setup.IndexOf("rn");
            int partName = setup.IndexOf("pn");

            List<SMD> smdList = new List<SMD>();

            while (true)
            {
                
                List<string> input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (input[0] == "end")
                {
                    break;
                }

                if (input.ElementAtOrDefault(designator) != null 
                    && input.ElementAtOrDefault(package) != null 
                    && input.ElementAtOrDefault(xAxis) != null
                    && input.ElementAtOrDefault(yAxis) != null
                    && input.ElementAtOrDefault(rotation) != null
                    && input.ElementAtOrDefault(partName) != null)
                {



                    string patternRegexCapFarads = @"(?<farad>(([0-9]+)(\.))?([0-9]+)([munp]))";
                    string patternRegexCapVolts = @"(?<voltage>(([0-9]+)(\.))?(([0-9]+)([vV])))";
                    string patternRegexResistors = @"\b(([0-9]+)(\.)?([0-9]+)?([KkMRr]))?\b";
                    string patternRegexResNoIndication = @"\b([0-9]+)(\.)?([0-9]+)\b";

                    string sDesignator = input[designator];
                    string sPackage = input[package];

                    string[] packagePattern = new string[] { "0603", "0805", "1206", "1806", "1812", "1825", "2010", "2512", "2725", "2920" };

                    foreach (string pattern in packagePattern)
                    {
                        if (sPackage.Contains(pattern))
                        {
                            sPackage = pattern;
                            if (sDesignator.Contains("R"))
                            {
                                sPackage = "R" + sPackage;
                            }
                            else if (sDesignator.Contains("C"))
                            {
                                sPackage = "C" + sPackage;
                            }
                            else if (sDesignator.Contains("LED"))
                            {
                                sPackage = "LED" + sPackage;
                            }
                            else if (sDesignator.Contains("L"))
                            {
                                sPackage = "L" + sPackage;
                            }
                            else if (sDesignator.Contains("D"))
                            {
                                sPackage = "D" + sPackage;
                            }
                            else if (sDesignator.Contains("F"))
                            {
                                sPackage = "FB" + sPackage;
                            }

                        }
                    }

                    string sXAxis = input[xAxis];
                    string sYAxis = input[yAxis];
                    string sRotation = input[rotation];
                    string sPartName = input[partName];

                    Match matchCap = Regex.Match(sPartName, patternRegexCapFarads);
                    Match matchCapVolts = Regex.Match(sPartName, patternRegexCapVolts);
                    Match matchResistors = Regex.Match(sPartName, patternRegexResistors);
                    if (matchCap.Success)
                    {
                        if (matchCap.Groups[3].Success)
                        {
                            sPartName = matchCap.Groups[2].ToString() + matchCap.Groups[5].ToString();

                            bool check = matchCap.Groups[4].ToString() != "0" && matchCap.Groups[4].ToString() != "00";

                            if (check)
                            {
                                sPartName += matchCap.Groups[4].ToString();
                            }

                            if (matchCapVolts.Groups["voltage"].Success)
                            {
                                if (matchCapVolts.Groups[3].Success)
                                {
                                    sPartName += "/" + matchCapVolts.Groups[2].ToString() + matchCapVolts.Groups[6].ToString() + matchCapVolts.Groups[5].ToString();
                                }
                                else if (!matchCapVolts.Groups[3].Success)
                                {
                                    sPartName += "/" + matchCapVolts.Groups["voltage"].ToString();
                                }
                            }

                        }
                        else if (!matchCap.Groups[3].Success)
                        {
                            sPartName = matchCap.Groups["farad"].ToString();
                            if (matchCapVolts.Groups["voltage"].Success)
                            {
                                if (matchCapVolts.Groups[3].Success)
                                {
                                    sPartName += "/" + matchCapVolts.Groups[2].ToString() + matchCapVolts.Groups[6].ToString() + matchCapVolts.Groups[5].ToString();
                                }
                                else if (!matchCapVolts.Groups[3].Success)
                                {
                                    sPartName += "/" + matchCapVolts.Groups["voltage"].ToString();
                                }
                            }
                        }

                    }
                    if (matchResistors.Groups[1].Success)
                    {
                        if (matchResistors.Groups[3].Success)
                        {
                            sPartName = matchResistors.Groups[2].ToString() + matchResistors.Groups[5].ToString();
                            bool check = matchResistors.Groups[4].ToString() != "0" || matchResistors.Groups[4].ToString() != "00";
                            if (check)
                            {
                                sPartName += matchResistors.Groups[4].ToString();
                            }
                        }
                        else if (!matchResistors.Groups[3].Success)
                        {
                            sPartName = matchResistors.Groups[0].ToString();
                            if (input.Contains("1%"))
                            {
                                sPartName = $"{sPartName} 1%";
                            }
                            else if (input.Contains("5%"))
                            {
                                sPartName = $"{sPartName} 5%";
                            }
                        }

                    }

                    Match matchResNoInd = Regex.Match(sPartName, patternRegexResNoIndication);

                    if (input.Contains("1%"))
                    {
                        sPartName += " 1%";
                    }
                    else if (input.Contains("5%"))
                    {
                        sPartName += " 5%";
                    }

                    if (matchResNoInd.Success)
                    {
                        int charR = sDesignator.IndexOf('R');
                        bool check = charR != -1;
                        if (check)
                        {
                            if (matchResNoInd.Groups[2].Success)
                            {
                                sPartName = matchResNoInd.Groups[1].ToString() + 'R' + matchResNoInd.Groups[3].ToString();
                            }
                            else
                            {
                                sPartName = matchResNoInd.Groups[0].ToString() + 'R';
                            }
                        }

                    }

                    SMD currentSMD = new SMD(sDesignator, sPackage, sXAxis, sYAxis, sRotation, sPartName);
                    smdList.Add(currentSMD);


                }
                else
                {
                    string InputErrorOutput = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\INPUT ERROR.txt");
                    try
                    {
                        StreamWriter sw = new StreamWriter(InputErrorOutput);
                        sw.WriteLine("There is an empty column!!!");
                        sw.Close();
                    }
                    catch (Exception message)
                    {
                        throw new ArgumentException(message.Message);
                    }
                    Environment.Exit(0);
                }
            }

            StringBuilder output = new StringBuilder();
            foreach (SMD smd in smdList)
            {
                output.AppendLine(smd.ToString());
            }

            string fileNameOutput = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"\ConvertedPickAndPlace.txt");

            try
            {
                StreamWriter sw = new StreamWriter(fileNameOutput);
                sw.WriteLine(output);
                sw.Close();
            }
            catch (Exception message)
            {
                throw new ArgumentException(message.Message);
            }


        }
    }
}


