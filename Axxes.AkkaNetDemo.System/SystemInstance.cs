using Akka.Actor;
using Akka.Persistence.SqlServer;
using Axxes.AkkaNetDemo.System.Actors;

namespace Axxes.AkkaNetDemo.System
{
    public class SystemInstance
    {
        public static readonly SystemInstance Current = new SystemInstance();

        public ActorSystem ActorSystem { get; }

        public IActorRef DevicesActor { get; }

        public SystemInstance()
        {
            ActorSystem = ActorSystem.Create("Axxes-AkkaNetDemo");

            var persistence = SqlServerPersistence.Get(ActorSystem);

            DevicesActor = ActorSystem.ActorOf(new Props(typeof(DevicesActor)), "devices");
        }
    }
}
