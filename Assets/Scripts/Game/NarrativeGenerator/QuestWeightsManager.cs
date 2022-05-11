using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Util;

namespace Game.NarrativeGenerator
{
    public class QuestWeightsManager
    {
        public Dictionary<string, Func<int,int>> killSymbolWeights = new Dictionary<string, Func<int,int>>();
        public Dictionary<string, Func<int,int>> talkSymbolWeights = new Dictionary<string, Func<int,int>>();
        public Dictionary<string, Func<int,int>> getSymbolWeights = new Dictionary<string, Func<int,int>>();
        public Dictionary<string, Func<int,int>> exploreSymbolWeights = new Dictionary<string, Func<int,int>>();

        public int CalculateTotalQuestionsWeight (List<int> answers)
        {
            int totalQuestionsWeight = 0;
            for (var i = 2; i < 12; i++)
            {
                if ( i < 11 )
                {
                    if ( i == 2 )
                    {
                        totalQuestionsWeight += answers[i] -3;
                    }
                    else
                    {
                        totalQuestionsWeight += answers[i];
                    }
                }
                else
                {
                    totalQuestionsWeight -= 3*(answers[i] -3);
                }
            }
            return totalQuestionsWeight;
        }

        public void CalculateTerminalSymbolsWeights ()
        {           
        }
        
        public int GetTalkQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Talk questions = 10, 11
            int [] talkWeightQuestions = {answers[9], answers[10], (-1*(answers[11]-3)), (-1*(answers[12]-3))};
            int talkWeight = CalculateWeightSum( talkWeightQuestions, totalQuestionsWeight );
            return talkWeight;
        }

        public int GetGetQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Get questions = 7, 8
            int [] getWeightQuestions = {answers[7], answers[8], (-1*(answers[11]-3)), (-1*(answers[12]-3))};
            int getWeight = CalculateWeightSum( getWeightQuestions, totalQuestionsWeight );
            return getWeight;
        }

        public int GetExploreQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Explore questions = 5, 6
            int [] exploreWeightQuestions = {answers[5], answers[6], (-1*(answers[11]-3)), (-1*(answers[12]-3))};
            int exploreWeight = CalculateWeightSum( exploreWeightQuestions, totalQuestionsWeight );
            return exploreWeight;
        }

        public int GetKillQuestWeight ( List<int> answers, int totalQuestionsWeight )
        {
            // Kill questions = 2, 3 e 4
            int [] killWeightQuestions = {answers[2]-3, answers[3], answers[4]};
            int killWeight = CalculateWeightSum( killWeightQuestions, totalQuestionsWeight );
            return killWeight;
        }

        private int CalculateWeightSum ( int [] answers, int totalQuestionsWeight )
        {
            int weightSum = (int)((float)(answers.Sum()/totalQuestionsWeight)*100);
            return weightSum;
        }
    }
}