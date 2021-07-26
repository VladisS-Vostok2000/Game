using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Game.ExtensionMethods;
using System.Drawing;

namespace Parser {
    public static class IniParser {
        public static IDictionary<string, IDictionary<string, string>> Parse(string filePath) {
            var outIniData = new Dictionary<string, IDictionary<string, string>>();
            string parcedText;
            using (var str = new StreamReader(filePath)) {
                parcedText = str.ReadToEnd();
            }

            string[] parcedLines = parcedText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parcedLines.Length; i++) {
                parcedLines[i] = RemoveComment(parcedLines[i]).ClearEmptySpaces();
            }

            for (int i = 0; i < parcedLines.Length; i++) {
                // Парсинг секции.
                if (parcedLines[i].Length == 0 || !IsIniSection(parcedLines[i])) {
                    continue;
                }

                string section = ExtractIniSectionName(parcedLines[i++]);
                if (!outIniData.ContainsKey(section)) {
                    outIniData.Add(section, new Dictionary<string, string>());
                }

                // Парсинг ключа.
                for (; i < parcedLines.Length; i++) {
                    if (IsIniSection(parcedLines[i])) {
                        i--;
                        break;
                    }

                    // Строка некорректна.
                    if (!IsIniKey(parcedLines[i])) {
                        continue;
                    }

                    string[] keyValuePair = parcedLines[i++].Split('=');
                    if (keyValuePair.Length != 2) {
                        continue;
                    }

                    string key = keyValuePair[0];
                    string value = keyValuePair[1];

                    // value разрешается разбивать на строки
                    for (; i < parcedLines.Length; i++) {
                        if (IsIniValue(parcedLines[i])) {
                            value += parcedLines[i];
                        }
                        else {
                            i--;
                            break;
                        }
                    }

                    if (!outIniData.ContainsKey(key)) {
                        outIniData[section].Add(key, value);
                    }
                    else {
                        outIniData[section][key] = value;
                    }
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
        private static string ExtractIniSectionName(string _parcedLine) => _parcedLine.Substring(1, _parcedLine.Length - 2);
        private static bool IsIniSection(string line) => line[0] == '[' && line.Count((char chr) => chr == '[') == 1 && line[line.Length - 1] == ']' && line.Count((char chr) => chr == ']') == 1 && !line.Contains('=');
        private static bool IsIniKey(string line) => line.Count((char chr) => chr == '=') == 1 && !line.Contains('[') && !line.Contains(']');
        private static bool IsIniValue(string line) => !line.Contains('=') && !line.Contains('[') && !line.Contains(']');

    }
}
