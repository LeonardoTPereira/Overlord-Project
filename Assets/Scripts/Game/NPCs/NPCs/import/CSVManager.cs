using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnumRaces;
using EnumJobs;
using FitnessScript;
using Game.NPCsa;
using MapElite;

namespace CsvManagement
{
    public class CSVManager
    {
        private string csvIterations = "/iterations.csv";
        private string csvChart = "/chart.csv";
        private string csvDetailed = "/gen_details.csv";
        MapElites mapElite;

        public void SetMapElite(MapElites mapElite)
        {
            this.mapElite = mapElite;
        }

        public bool isMapEliteSet()
        {
            if (mapElite != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // save iterations OK
        public void SaveIterationsFitness(string destinyPath)
        {
            if (!isMapEliteSet())
            {
                Debug.LogError("The MapElite variable was not attributed.");
                return;
            }

            string fileContent = "";
            string uniqueFileName = AssetDatabase.GUIDToAssetPath(destinyPath + csvIterations);

            if (uniqueFileName == "")
            {
                uniqueFileName = destinyPath + csvIterations;
            }

            fileContent = CreateIterationsHeader();

            IterationsFileContent(ref fileContent);

            Debug.Log("filename = " + uniqueFileName);
            System.IO.File.WriteAllText(uniqueFileName, fileContent);
        }

        // save chart for pie OK
        public void SaveChartForPiePlot(string destinyPath)
        {
            if (!isMapEliteSet())
            {
                Debug.LogError("The MapElite variable was not attributed.");
                return;
            }

            string fileContent = "";
            string uniqueFileName = AssetDatabase.GUIDToAssetPath(destinyPath + csvChart);

            if (uniqueFileName == "")
            {
                uniqueFileName = destinyPath + csvChart;
            }

            fileContent = CreateChartHeader();

            ChartFileContent(ref fileContent);

            System.IO.File.WriteAllText(uniqueFileName, fileContent);
        }

        // save chart for pie OK
        public void SaveGenerationDetails(string destinyPath)
        {
            if (!isMapEliteSet())
            {
                Debug.LogError("The MapElite variable was not attributed.");
                return;
            }

            string fileContent = "";
            string uniqueFileName = AssetDatabase.GUIDToAssetPath(destinyPath + csvDetailed);

            if (uniqueFileName == "")
            {
                uniqueFileName = destinyPath + csvDetailed;
            }

            fileContent = CreateDetailsHeader();

            DetailsFileContent(ref fileContent);

            System.IO.File.WriteAllText(uniqueFileName, fileContent);
        }

        // create header for iterations csv file OK
        private string CreateIterationsHeader()
        {
            string header = "";

            for (int i = 0; i < Races.NumberOfRaces(); i++)
            {
                for (int j = 0; j < Jobs.NumberOfJobs(); j++)
                {
                    header += ((Races.raceID)i).ToString() + "-" + ((Jobs.jobID)j).ToString();

                    // trocar quando guardar o fitness dentro de cada npc
                    if (!(i == (Races.NumberOfRaces() - 1) && j == (Jobs.NumberOfJobs() - 1)))
                    {
                        header += ",";
                    }
                }
            }

            return header;
        }

        // create header for chart csv file OK
        private string CreateChartHeader()
        {
            string header = "Social,Attain,Violence,Explore";
            return header;
        }

        private string CreateDetailsHeader()
        {
            string header = "f1,f2,race,job,social,attain,violence,explore";
            return header;
        }

        // insere uma nova linha com todos os npcs do mapa na string do conteudo OK
        private void IterationsFileContent(ref string fileContent)
        {
            var sb = new System.Text.StringBuilder(fileContent);
            NPC_SO[,] map = mapElite.map;

            sb.Append('\n');

            for (int i = 0; i < Races.NumberOfRaces(); i++)
            {
                for (int j = 0; j < Jobs.NumberOfJobs(); j++)
                {
                    if (map[i, j] == null)
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        sb.Append(map[i, j].fitness.ToString().Replace(",", "."));
                    }

                    if (!(i == (Races.NumberOfRaces() - 1) && j == (Jobs.NumberOfJobs() - 1)))
                    {
                        sb.Append(',');
                    }
                }
            }

            fileContent = sb.ToString();

            return;
        }

        private void ChartFileContent(ref string fileContent)
        {
            var sb = new System.Text.StringBuilder(fileContent);
            NPC_SO[,] map = mapElite.map;
            int fixedIndex = mapElite.questID;

            float sSocial = 0, sAttain = 0, sViolence = 0, sExplore = 0;

            sb.Append('\n');

            //lógica do L
            if (map[fixedIndex, fixedIndex] != null)
            {
                sSocial += map[fixedIndex, fixedIndex].social;
                sAttain += map[fixedIndex, fixedIndex].attain;
                sViolence += map[fixedIndex, fixedIndex].violence;
                sExplore += map[fixedIndex, fixedIndex].explore;
            }

            for (int i = 0; i < 4; i++)
            {
                if (i != fixedIndex && map[fixedIndex, i] != null)
                {
                    sSocial += map[fixedIndex, i].social;
                    sAttain += map[fixedIndex, i].attain;
                    sViolence += map[fixedIndex, i].violence;
                    sExplore += map[fixedIndex, i].explore;
                }

                if (i != fixedIndex && map[i, fixedIndex] != null)
                {
                    sSocial += map[i, fixedIndex].social;
                    sAttain += map[i, fixedIndex].attain;
                    sViolence += map[i, fixedIndex].violence;
                    sExplore += map[i, fixedIndex].explore;
                }
            }

            sb.Append(sSocial.ToString().Replace(",", "."));
            sb.Append(",");
            sb.Append(sAttain.ToString().Replace(",", "."));
            sb.Append(",");
            sb.Append(sViolence.ToString().Replace(",", "."));
            sb.Append(",");
            sb.Append(sExplore.ToString().Replace(",", "."));

            fileContent = sb.ToString();

            return;
        }

        private void DetailsFileContent(ref string fileContent)
        {
            var sb = new System.Text.StringBuilder(fileContent);
            NPC_SO[,] map = mapElite.map;

            sb.Append('\n');

            for (int i = 0; i < Races.NumberOfRaces(); i++)
            {
                for (int j = 0; j < Jobs.NumberOfJobs(); j++)
                {
                    if (map[i, j] == null)
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        sb.Append(map[i, j].fitness1.ToString().Replace(",", "."));
                        sb.Append(",");
                        sb.Append(map[i, j].fitness2.ToString().Replace(",", "."));
                        sb.Append(",");

                        sb.Append(i);
                        sb.Append(",");
                        sb.Append(j);
                        sb.Append(",");

                        sb.Append(map[i, j].social.ToString().Replace(",", "."));
                        sb.Append(",");
                        sb.Append(map[i, j].attain.ToString().Replace(",", "."));
                        sb.Append(",");
                        sb.Append(map[i, j].violence.ToString().Replace(",", "."));
                        sb.Append(",");
                        sb.Append(map[i, j].explore.ToString().Replace(",", "."));

                    }

                    if (!(i == (Races.NumberOfRaces() - 1) && j == (Jobs.NumberOfJobs() - 1)))
                    {
                        sb.Append('\n');
                    }
                }
            }


            fileContent = sb.ToString();

            return;
        }
    }

}