using Akka.Actor;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class DevicesActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is RegisterDevice registerMessage)
            {
                RegisterDeviceActor(registerMessage);
            }
        }

        private void RegisterDeviceActor(RegisterDevice registerMessage)
        {
            var actorName = $"device-{registerMessage.DeviceId}";

            var child = Context.Child(actorName);

            if (!child.Equals(Nobody.Instance))
                return;

            var deviceActorProps =
                Props.Create<DeviceActor>(registerMessage.DeviceId);

            // Actor address = /user/devices/device-{guid}
            Context.ActorOf(deviceActorProps, actorName);
        }
    }
}
