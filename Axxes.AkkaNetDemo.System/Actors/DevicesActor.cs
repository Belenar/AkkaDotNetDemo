using Akka.Actor;
using Axxes.AkkaNetDemo.System.Actors.Device;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors
{
    public class DevicesActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is RegisterDevice)
                RegisterDeviceActor((RegisterDevice)message);
        }

        private void RegisterDeviceActor(RegisterDevice message)
        {
            var actorname = $"device-{message.DeviceId}";

            var child = Context.Child(actorname);

            if (!child.Equals(Nobody.Instance))
                return;

            var deviceActorProps = Props.Create<DeviceActor>(message.DeviceId);
            Context.ActorOf(deviceActorProps, $"device-{message.DeviceId}");
        }
    }
}
