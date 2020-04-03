using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Files
{
    public class FileReader
    {
        public static TextAsset m_MapFile { get; private set; }
        public static int m_mapLenght { get; private set; }
        public static int m_mapWidth { get; private set; }

        public static void setMapFile(TextAsset mapDoc)
        {
            if (m_MapFile == null)
            {
                m_MapFile = mapDoc;
            }
        }


        public static List<string> ReadMapFile()
        {
            List<string> fileContent = new List<string>();
            using (StreamReader reader = new StreamReader("Assets\\Resources\\" + TowerDefense.ProjectPaths.RESOURCES_MAP_SETTINGS + m_MapFile.name + ".txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null) { fileContent.Add(line); }
                reader.Close();
            }
            return fileContent;
        }

        //Funks!! :D
        public static Dictionary<int, int> GetWavesFileContent(List<string> fileContent)
        {
            Dictionary<int, int> waves = new Dictionary<int, int>();

            for (int i = m_mapLenght + 1; i < fileContent.Count; i++)
            {
                int[] unitNumbers = Array.ConvertAll(fileContent[i].Split(' '), element => int.Parse(element));
                waves.Add(unitNumbers[0], unitNumbers[1]);
            }
            return waves;
        }

        public static Dictionary<int, int[]> GetMapDictionary(List<string> fileContent)
        {
            Dictionary<int, int[]> map = new Dictionary<int, int[]>();
            if (m_mapWidth <= 0) { m_mapWidth = fileContent[0].Length; }
            foreach (string line in fileContent)
            {
                if (line.Contains("#")) { break; }

                int[] numberRow = Array.ConvertAll(line.ToCharArray(), element => int.Parse(element.ToString()));
                map.Add(m_mapLenght, numberRow);
                m_mapLenght++;
            }
            return map;
        }


    }

}