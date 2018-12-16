using System;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class DeviceActor : ReceiveActor
    {
        private IActorRef _quarterlyConsumptionActor;
        private IActorRef _hourlyConsumptionActor;
        private IActorRef _hourlyStorageActor;
        private IActorRef _highConsumptionActor;

        public Guid DeviceId { get; }

        public DeviceActor(Guid deviceId)
        {
            DeviceId = deviceId;

            CreateChildActors();
            AddHighConsumptionActor();

            Receive<MeterReadingReceived>(message => Handle(message));
            Receive<QuarterCompleted>(message => Handle(message));
            Receive<HourCompleted>(message => Handle(message));
        }

        private void CreateChildActors()
        {
            var quarterlyConsumptionActorProps = Props.Create<QuarterlyConsumptionCalculatorActor>(DeviceId);
            _quarterlyConsumptionActor = Context.ActorOf(quarterlyConsumptionActorProps);

            var hourlyConsumptionActorProps = Props.Create<HourlyConsumptionCalculatorActor>();
            _hourlyConsumptionActor = Context.ActorOf(hourlyConsumptionActorProps);

            var hourlyStorageActorProps = Props.Create<HourlyConsumptionStorageActor>();
            _hourlyStorageActor = Context.ActorOf(hourlyStorageActorProps);
        }

        private void AddHighConsumptionActor()
        {
            var random = new Random();
            var quarters = random.Next(6);
            var threshold = (decimal) random.Next(100) / 100;

            var highUsageProps = Props.Create<HighUsageAlertActor>(quarters, threshold);
            _highConsumptionActor = Context.ActorOf(highUsageProps);
        }

        public void Handle(MeterReadingReceived message)
        {
            _quarterlyConsumptionActor.Tell(message);
        }

        public void Handle(QuarterCompleted message)
        {
            _hourlyConsumptionActor.Tell(message);
            if (_highConsumptionActor != null)
            {
                _highConsumptionActor.Tell(message);
            }
        }

        public void Handle(HourCompleted message)
        {
            _hourlyStorageActor.Tell(message);
        }
    }
}