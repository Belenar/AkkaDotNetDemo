using System;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaNetDemo.System.Helpers;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class QuarterlyConsumptionCalculatorActor : PersistentActor
    {
        public override string PersistenceId => $"device-quarterly-{DeviceId}";
        public Guid DeviceId { get; }

        public QuarterlyConsumptionCalculatorActor(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        protected override bool ReceiveCommand(object message)
        {
            if (message is MeterReadingReceived)
            {
                var receivedMessage = (MeterReadingReceived)message;
                Persist(receivedMessage, msg => HandleMeterReading(msg));
            }
            return true;
        }

        protected override bool ReceiveRecover(object message)
        {
            if (message is MeterReadingReceived)
            {
                HandleMeterReading((MeterReadingReceived)message);
            }
            if (message is SnapshotOffer)
            {
                var snapshot = (QuarterlySnapshot)((SnapshotOffer)message).Snapshot;
                SetNewReferenceValues(snapshot.LastMessage, snapshot.Quarters);
            }
            return true;
        }

        private DateTime _referenceDate;
        private decimal _referenceValue;
        private int _referenceQuarter;
        private MeterReadingReceived _lastMessage;

        public void HandleMeterReading(MeterReadingReceived message)
        {
            if (_lastMessage == null)
            {
                SetInitialValues(message);
                return;
            }

            if (message.Timestamp.QuarterNumber() != _referenceQuarter)
            {
                ProcessNewQuarters(message);
            }
            else
            {
                _lastMessage = message;
            }
        }

        private void ProcessNewQuarters(MeterReadingReceived message)
        {
            var processNewQuarters = new QuarterlyConsumptionParameters
            {
                StartDate = _referenceDate,
                StartMeterReading = _referenceValue,
                PreviousMessage = _lastMessage,
                CurrentMessage = message
            };

            var quarters = QuarterlyConsumptionHelper.ComputeQuarterlyConsumption(processNewQuarters);

            foreach (var quarter in quarters.Quarters)
            {
                var quarterMessage = new QuarterCompleted
                {
                    DeviceId = message.DeviceId,
                    Timestamp = quarter.Item1,
                    Consumption = quarter.Item2,
                    Unit = message.Unit
                };
                DistributeMessage(quarterMessage);
            }

            SetNewReferenceValues(message, quarters);
            SaveSnapshot(new QuarterlySnapshot { LastMessage = message, Quarters = quarters });
        }

        private void DistributeMessage(QuarterCompleted quarterMessage)
        {
            Context.Parent.Tell(quarterMessage);
        }

        private void SetInitialValues(MeterReadingReceived message)
        {
            _lastMessage = message;
            _referenceDate = message.Timestamp;
            _referenceValue = message.MeterValue;
            _referenceQuarter = message.Timestamp.QuarterNumber();
        }

        private void SetNewReferenceValues(MeterReadingReceived message, QuarterlyConsumptionResult quarters)
        {
            _referenceDate = quarters.NewReferenceDate;
            _referenceValue = quarters.NewReferenceReading;
            _referenceQuarter = quarters.NewReferenceQuarter;
            _lastMessage = message;
        }
    }

    class QuarterlySnapshot
    {
        public MeterReadingReceived LastMessage { get; set; }
        public QuarterlyConsumptionResult Quarters { get; set; }
    }
}