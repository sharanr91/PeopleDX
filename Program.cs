using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static System.IO.Directory;
using static System.IO.Path;
using static System.Environment;
using static System.Int32;



namespace RecruitmentStep1
{
    class Program
    {
        static void Main(string[] args)
        {

            //Get file
            string projectDirectory = GetParent(CurrentDirectory).Parent.Parent.FullName;
            string datasetPath = Combine(projectDirectory, "datasets", "dataset1.txt");


            //Read file to lines
            var lines = File.ReadAllLines(datasetPath);

            //Load Attributes from Attribute Mapping section
            var attributesDictionary = LoadAttributesForMapping(lines);


            //set attribute sequence for data set printing and extracting values
            var attributeSequence = lines[1].Split(',');
            Attribute[] attributeObjInSeq = SetAttributeObjectsInSequence(attributesDictionary, attributeSequence);

            //Go through each line and print user record 
            foreach (var line in lines)
            {
                string userDetails;
                string[] dataSplit = line.Split(',');
                if (dataSplit.Length == 6)
                {
                    userDetails = $"{attributeSequence[0]}: {dataSplit[0]}\n" +
                                         $"{attributeSequence[1]}: {dataSplit[1]}\n" +
                                         $"{attributeObjInSeq[0].Name}: {GetAttributeValue(attributeObjInSeq[0], dataSplit[2])}\n" +
                                         $"{attributeObjInSeq[1].Name}: {GetAttributeValue(attributeObjInSeq[1], dataSplit[3])}\n" +
                                         $"{attributeObjInSeq[2].Name}: {GetAttributeValue(attributeObjInSeq[2], dataSplit[4])}\n" +
                                         $"{attributeObjInSeq[3].Name}: {GetAttributeValue(attributeObjInSeq[3], dataSplit[5])}\n";
                    Console.WriteLine(userDetails);
                }
                else
                {
                    userDetails = $"Attributes for this user might be missing";
                }


                Console.WriteLine("\n");
            }
        }

        #region
        static Attribute[] SetAttributeObjectsInSequence(
                    Dictionary<Guid, Attribute> attributesDictionary,
                    string[] attributeSequence)
        {
            Attribute[] attributeObjInSeq =
            {
                attributesDictionary[new Guid(attributeSequence[2].Trim())],
                attributesDictionary[new Guid(attributeSequence[3].Trim())],
                attributesDictionary[new Guid(attributeSequence[4].Trim())],
                attributesDictionary[new Guid(attributeSequence[5].Trim())]
            };
            return attributeObjInSeq;
        }

        static Dictionary<Guid, Attribute> LoadAttributesForMapping(string[] lines)
        {
            var attributesDictionary = new Dictionary<Guid, Attribute>();
            int attributesStartLine = Array.IndexOf(lines, "# Attribute Mapping") + 2;
            for (int i = attributesStartLine; i < lines.Length; i++)
            {
                var lineData = lines[i].Split(',');
                var possibleValues = GetPossibleValuesFromAttribute(lines[i]);
                var attribute = new Attribute()
                {
                    Id = new Guid(lineData[0].Trim()),
                    Name = lineData[1],
                    Minvalue = lineData[3],
                    MaxValue = lineData[4],
                    Optional = bool.Parse(lineData[5]),
                    PossibleValues = possibleValues
                };
                attributesDictionary.Add(new Guid(lineData[0].Trim()), attribute);
            }

            return attributesDictionary;
        }

        static string GetAttributeValue(Attribute attribute, string value)
        {
            if (attribute.Name.Trim().Equals("age", StringComparison.InvariantCultureIgnoreCase) ||
                attribute.Name.Trim().Equals("salary", StringComparison.InvariantCultureIgnoreCase))
            {
                return ValidateAgeOrSalary(value, attribute.Minvalue, attribute.MaxValue);
            }
            else
            {
                return GetFromPossibleValuesList(attribute.PossibleValues, value);
            }

        }
        static Dictionary<Guid, string> GetPossibleValuesFromAttribute(string lineData)
        {
            var possibleValues = new Dictionary<Guid, string>();
            var reg = new Regex("\".*?\"");
            var matches = reg.Matches(lineData);
            if (matches.Count > 4)
            {
                for (var i = 0; i < matches.Count; i += 4)
                {
                    var guid = matches[i + 1].ToString();
                    guid = guid.Substring(1, guid.Length - 2);
                    var value = matches[i + 3].ToString();
                    value = value.Substring(1, value.Length - 2);
                    possibleValues.Add(new Guid(guid), value);
                }

                return possibleValues;
            }
            else
            {
                return null;
            }

        }

        static string ValidateAgeOrSalary(string givenValue, string minValue, string maxValue)
        {
            var success = TryParse(givenValue, out int _value);
            if (_value < Parse(minValue) && _value > Parse(maxValue))
            {
                //can further check and return too high or too low or return as missing
                return "Invalid Age";
            }

            return givenValue;

        }


        static string GetFromPossibleValuesList(Dictionary<Guid, string> possibleValuesList, string guid)
        {
            var userAttributeValue = "";

            if (guid.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                return "Null";
            }

            if (!string.IsNullOrEmpty(guid))
            {
                possibleValuesList.TryGetValue(new Guid(guid), out userAttributeValue);
            }

            return string.IsNullOrEmpty(userAttributeValue) ? $"# Invalid value" : userAttributeValue;

        }
        #endregion

    }
}





