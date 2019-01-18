using System;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaNetDemo.System.Helpers;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class QuarterlyConsumptionCalculatorActor : PersistentActor
    {
        public Guid DeviceId { get; }
        private DateTime _referenceDate;
        private decimal _referenceValue;
        private int _referenceQuarter;
        private MeterReadingReceived _lastMessage;

        public QuarterlyConsumptionCalculatorActor(Guid deviceId)
        {
            DeviceId = deviceId;
        }

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
            SaveSnapshot(new QuartelySnapshot
            {
                LastMessage = message,
                Quarters = quarters
            });
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

        protected override bool ReceiveRecover(object message)
        {
            if (message is MeterReadingReceived mrMessage)
            {
                HandleMeterReading(mrMessage);
            }

            if (message is SnapshotOffer snapshotOffer)
            {
                var snapshot = (QuartelySnapshot) snapshotOffer.Snapshot;
                SetNewReferenceValues(snapshot.LastMessage, snapshot.Quarters);
            }
            return true;
        }

        protected override bool ReceiveCommand(object message)
        {
            if (message is MeterReadingReceived mrMessage)
            {
                Persist(mrMessage, msg => HandleMeterReading(msg));
            }
            return true;
        }

        public override string PersistenceId
            => $"device-quarterly-{DeviceId}";
    }

    public class QuartelySnapshot
    {
        public MeterReadingReceived LastMessage { get; set; }
        public QuarterlyConsumptionResult Quarters { get; set; }
    }
}