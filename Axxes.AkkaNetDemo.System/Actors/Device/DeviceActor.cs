using System;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class DeviceActor : ReceiveActor, 
        IHandle<MeterReadingReceived>,
        IHandle<QuarterCompleted>,
        IHandle<HourCompleted>
    {
        private readonly IActorRef _quarterlyConsumptionActor;
        private readonly IActorRef _hourlyConsumptionActor;
        private readonly IActorRef _hourlyStorageActor;
        private IActorRef _highConsumptionActor;

        public Guid DeviceId { get; }

        public DeviceActor(Guid deviceId)
        {
            DeviceId = deviceId;

            var quarterlyConsumptionActorProps = Props.Create<QuarterlyConsumptionCalculatorActor>(DeviceId);
            _quarterlyConsumptionActor = Context.ActorOf(quarterlyConsumptionActorProps);

            var hourlyConsumptionActorProps = Props.Create<HourlyConsumptionCalculatorActor>();
            _hourlyConsumptionActor = Context.ActorOf(hourlyConsumptionActorProps);

            var hourlyStorageActorProps = Props.Create<HourlyConsumptionStorageActor>();
            _hourlyStorageActor = Context.ActorOf(hourlyStorageActorProps);

            AddHighConsumptionActor();
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