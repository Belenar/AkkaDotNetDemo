using Akka.Actor;
using Akka.Persistence.SqlServer;
using Axxes.AkkaNetDemo.System.Actors.Device;

namespace Axxes.AkkaNetDemo.System
{
    public class SystemInstance
    {
        public static readonly SystemInstance Current
            = new SystemInstance();
        public ActorSystem ActorSystem { get; set; }
        public IActorRef DevicesActor { get; set; }

        public SystemInstance()
        {
            ActorSystem = Akka.Actor.ActorSystem
                .Create("Axxes-AkkaNetDemo");

            var persistence = SqlServerPersistence.Get(ActorSystem);

            var devicesProps = new Props(typeof(DevicesActor));

            DevicesActor = ActorSystem.ActorOf(devicesProps, "devices");
        }
    }
}
