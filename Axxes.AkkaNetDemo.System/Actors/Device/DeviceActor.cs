using System;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class DeviceActor : ReceiveActor
    {
        private IActorRef _quarterlyActor;
        private IActorRef _hourlyActor;
        private IActorRef _hourlyStorageActor;

        public Guid DeviceId { get; }

        public DeviceActor(Guid deviceId)
        {
            DeviceId = deviceId;

            CreateChildActors();

            Receive<MeterReadingReceived>(message => MeterReading(message));
            Receive<QuarterCompleted>(message => QuarterCompleted(message));
            Receive<HourCompleted>(message => HourCompleted(message));
        }



        private void CreateChildActors()
        {
            var quarterlyProps
                = Props.Create<QuarterlyConsumptionCalculatorActor>(DeviceId);
            _quarterlyActor = Context.ActorOf(quarterlyProps);

            var hourlyProps
                = Props.Create<HourlyConsumptionCalculatorActor>();
            _hourlyActor = Context.ActorOf(hourlyProps);

            var hourlyStorageProps
                = Props.Create<HourlyConsumptionStorageActor>();
            _hourlyStorageActor = Context.ActorOf(hourlyStorageProps);
        }

        private void MeterReading(MeterReadingReceived message)
        {
            _quarterlyActor.Tell(message);
        }

        private void QuarterCompleted(QuarterCompleted message)
        {
            _hourlyActor.Tell(message);
        }

        private void HourCompleted(HourCompleted message)
        {
            _hourlyStorageActor.Tell(message);
        }
    }
}