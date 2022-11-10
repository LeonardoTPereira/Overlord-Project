using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumJobs
{
    public class Jobs
    {
        public enum jobID
        {
            Soldado,
            Comerciante,
            Pirata,
            Mago,
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
