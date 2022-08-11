using UnityEngine;

//namespace DialogueTrees
//{
    public class WaitNode : ActionNode
    {
        public float duration = 1;
        private float _startTime;

        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnStop()
        {
        
        }

        protected override State OnUpdate()
        {
            return Time.time - _startTime > duration ? State.Success : State.Running;
        }
    }
//}
