using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumRaces;
using EnumJobs;
using FitnessScript;


public class CSVManager
{
    private string csvFileName = "/iterations.csv";

    public void SaveMapFitness(NPC_SO[,] map, int questID, string destinyPath)
    {
        string fileContent = "";

        if (System.IO.File.Exists(destinyPath + csvFileName))
        {
            fileContent = System.IO.File.ReadAllText(destinyPath + csvFileName);
        }
        else
        {
            fileContent = CreateHeader();
        }

        PrepareFileContent(ref fileContent, map, questID);

        System.IO.File.WriteAllText(destinyPath + csvFileName, fileContent);
    }

    public void SaveMapFitness(MapElites mapElite, string destinyPath)
    {
        string fileContent = "";

        if (System.IO.File.Exists(destinyPath + csvFileName))
        {
            fileContent = System.IO.File.ReadAllText(destinyPath + csvFileName);
        }
        else
        {
            fileContent = CreateHeader();
        }

        PrepareFileContent(ref fileContent, mapElite.map, mapElite.questID);

        System.IO.File.WriteAllText(destinyPath + csvFileName, fileContent);
    }

    private string CreateHeader()
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

    // insere uma nova linha com todos os npcs do mapa na string do conteudo
    private void PrepareFileContent(ref string fileContent, NPC_SO[,] map, int questID)
    {
        var sb = new System.Text.StringBuilder(fileContent);

        sb.Append('\n');

        for (int i = 0; i < Races.NumberOfRaces(); i++)
        {
            for (int j = 0; j < Jobs.NumberOfJobs(); j++)
            {
                // trocar quando guardar o fitness dentro de cada npc
                sb.Append(Fitness.Calculate(map[i, j], questID).ToString().Replace(",", "."));

                if (!(i == (Races.NumberOfRaces() - 1) && j == (Jobs.NumberOfJobs() - 1)))
                {
                    sb.Append(',');
                }
            }
        }

        fileContent = sb.ToString();

        return;
    }
}
