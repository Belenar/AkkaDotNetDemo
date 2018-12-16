using System;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Helpers;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class QuarterlyConsumptionCalculatorActor : ReceiveActor
    {
        private DateTime _referenceDate;
        private decimal _referenceValue;
        private int _referenceQuarter;
        private MeterReadingReceived _lastMessage;

        public QuarterlyConsumptionCalculatorActor()
        {
            Receive<MeterReadingReceived>(message => Handle(message));
        }

        public void Handle(MeterReadingReceived message)
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
}