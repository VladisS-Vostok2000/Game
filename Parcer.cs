using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Undefinded;

namespace IniParcer {
    public static class Parcer {
        public static Dictionary<string, Dictionary<string, string>> Parse(string filePath) {
            var outIniData = new Dictionary<string, Dictionary<string, string>>();
            string parcedText;
            using (var str = new StreamReader(filePath)) {
                parcedText = str.ReadToEnd();
            }

            string[] parcedLines = parcedText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parcedLines.Length; i++) {
                parcedLines[i] = RemoveComment(parcedLines[i]).ClearEmptySpaces();
            }

            for (int i = 0; i < parcedLines.Length; i++) {
                if (!IsIniSection(parcedLines[i])) {
                    continue;
                }

                string section = ExtractIniSectionName(parcedLines[i++]);
                if (!outIniData.ContainsKey(section)) {
                    outIniData.Add(section, new Dictionary<string, string>());
                }

                for (; i < parcedLines.Length; i++) {
                    if (IsIniSection(parcedLines[i])) {
                        i--;
                        break;
                    }

                    if (!IsIniKey(parcedLines[i])) {
                        continue;
                    }

                    string[] keyValuePair = parcedLines[i++].Split('=');
                    if (keyValuePair.Length != 2) {
                        continue;
                    }

                    string key = keyValuePair[0];
                    string value = keyValuePair[1];

                    // value разрещается разбивать на строки
                    for (; i < parcedLines.Length; i++) {
                        if (IsIniValue(parcedLines[i])) {
                            value += parcedLines[i];
                        }
                        else {
                            i--;
                            break;
                        }
                    }

                    outIniData[section].Add(key, value);
                }
            }

            return outIniData;
            
        }
        private static string RemoveComment(string line) {
            string outString = "";
            for (int i = 0; i < line.Length; i++) {
                if (line[i] == ';') {
                    break;
                }

                outString += line[i];
            }
            return outString;
        }
        private static bool IsIniSection(string line) => line[0] == '[' && line[line.Length - 1] == ']';
        private static string ExtractIniSectionName(string _parcedLine) => _parcedLine.Extract(1, _parcedLine.Length - 2);
        private static bool IsIniKey(string line) => line.Contains("=");
        private static bool IsIniValue(string line) => !(IsIniSection(line) || IsIniKey(line));

    }
}
