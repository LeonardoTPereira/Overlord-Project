//namespace DialogueTrees
//{
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {
        
        }

        protected override void OnStop()
        {
        
        }

        protected override State OnUpdate()
        {
            child.Update();
            //allways return running means allways looping
            return State.Running;
        }
    }
//}
