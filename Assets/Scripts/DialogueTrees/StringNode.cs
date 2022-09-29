using UnityEngine;

//namespace DialogueTrees
//{
    public class StringNode : ActionNode
    {
        public string message;
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate {message}"); //change later
            return State.Success;
        }
    }
//}
