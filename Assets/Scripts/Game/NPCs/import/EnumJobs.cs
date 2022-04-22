using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EnumJobs
{
    public class Jobs
    {
        public enum jobID
        {
            Fazendeiro,
            Caçador,
            Lenhador,
            Herbalista,
            Coletor,
            Guarda,
            Capitao,
            General,
            Civil,
            Explorador,
            NumberOfJobs
        }

        //usar enum getvalues()
        //pode usar essa classe como singleton

        public static int NumberOfJobs()
        {
            return (int)jobID.NumberOfJobs;
        }
    }
}
