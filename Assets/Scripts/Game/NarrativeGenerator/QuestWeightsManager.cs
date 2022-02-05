using UnityEngine;

namespace Game.NarrativeGenerator
{
    public class QuestWeightsManager
    {
        private float CalculateTotalQuestionsWeight (List<int> answers)
        {
            float totalQuestionsWeight = 0;
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

        private void CalculateTerminalSymbolsWeights ()
        {
            killSymbolWeights.Add( Constants.KILL_TERMINAL, x => Mathf.Clamp( 1/(x*0.25f), 0, 1) );
            killSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            talkSymbolWeights.Add( Constants.TALK_TERMINAL, x => Mathf.Clamp( 1/(x*0.25f), 0, 1) );
            talkSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            getSymbolWeights.Add( Constants.ITEM_TERMINAL, x => Mathf.Clamp( 0.3f*(1/(x*0.25f)), 0, .3f));
            getSymbolWeights.Add( Constants.DROP_TERMINAL, x => Mathf.Clamp( 0.3f*(1/(x*0.25f)), 0, .3f));
            getSymbolWeights.Add( Constants.GET_TERMINAL, x => Mathf.Clamp(  0.3f*(1/(x*0.25f)), 0, .3f));
            getSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));

            exploreSymbolWeights.Add( Constants.SECRET_TERMINAL, x => Mathf.Clamp( 1/(x*0.25f), 0, 1) );
            exploreSymbolWeights.Add( Constants.EMPTY_TERMINAL, x => Mathf.Clamp( ( 1 -( 1/(x*0.25f))), 0, 1));
        }
        
        private float GetTalkQuestWeight ( List<int> answers, float totalQuestionsWeight )
            {
            float [] talkWeightQuestions = {(float)answers[9], (float)answers[10], (float)(-1*(answers[11]-3)), (float)(-1*(answers[12]-3))};
            float talkWeight = CalculateWeightSum( talkWeightQuestions, totalQuestionsWeight );
            return talkWeight;
        }

        private float GetGetQuestWeight ( List<int> answers, float totalQuestionsWeight )
        {
            float [] getWeightQuestions = {(float)answers[7], (float)answers[8], (float)(-1*(answers[11]-3)), (float)(-1*(answers[12]-3))};
            float getWeight = CalculateWeightSum( getWeightQuestions, totalQuestionsWeight );
            return getWeight;
        }

        private float GetExploreQuestWeight ( List<int> answers, float totalQuestionsWeight )
        {
            float [] exploreWeightQuestions = {(float)answers[5], (float)answers[6], (float)(-1*(answers[11]-3)), (float)(-1*(answers[12]-3))};
            float exploreWeight = CalculateWeightSum( exploreWeightQuestions, totalQuestionsWeight );
            return exploreWeight;
        }

        private float GetKillQuestWeight ( List<int> answers, float totalQuestionsWeight )
        {
            float [] killWeightQuestions = {(float)answers[2]-3, (float)answers[3], (float)answers[4]};
            float killWeight = CalculateWeightSum( killWeightQuestions, totalQuestionsWeight );
            return killWeight;
        }

        private float CalculateWeightSum ( float [] answers, float totalQuestionsWeight )
        {
            float weightSum = answers.Sum() / totalQuestionsWeight;
            weightSum = float.IsNaN( weightSum ) ? 0 : weightSum;
            return weightSum;
        }
    }
}