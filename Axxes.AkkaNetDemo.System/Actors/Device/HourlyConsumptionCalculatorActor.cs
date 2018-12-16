using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Helpers;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    class HourlyConsumptionCalculatorActor : ReceiveActor
    {
        private int _referenceHour;
        private List<QuarterCompleted> _hourlyMessages;

        public HourlyConsumptionCalculatorActor()
        {
            Receive<QuarterCompleted>(message => Handle(message));
        }

        public void Handle(QuarterCompleted message)
        {
            if (_referenceHour == 0)
                StartNewHour(message);

            if (message.Timestamp.HourNumber() != _referenceHour)
            {
                ProcessHour(message);
                StartNewHour(message);
            }
            else
            {
                _hourlyMessages.Add(message);
            }
        }

        private void ProcessHour(QuarterCompleted message)
        {
            var hourMessage = new HourCompleted
            {
                DeviceId = message.DeviceId,
                Timestamp = DateTimeExtensions.GetHourDate(_referenceHour),
                Consumption = _hourlyMessages.Sum(x => x.Consumption),
                Unit = message.Unit
            };
            Context.Parent.Tell(hourMessage);
        }

        private void StartNewHour(QuarterCompleted message)
        {
            _referenceHour = message.Timestamp.HourNumber();
            _hourlyMessages = new List<QuarterCompleted> { message };
        }
    }
}