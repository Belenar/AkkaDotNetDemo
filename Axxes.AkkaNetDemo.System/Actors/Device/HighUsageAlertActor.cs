using System.Configuration;
using System.Data.SqlClient;
using Akka.Actor;
using Axxes.AkkaNetDemo.System.Messages;
using Dapper;

namespace Axxes.AkkaNetDemo.System.Actors.Device
{
    public class HighUsageAlertActor : ReceiveActor
    {
        public int NumberOfQuarters { get; }
        public decimal ThresholdValue { get; }

        public HighUsageAlertActor(int numberOfQuarters, decimal thresholdValue)
        {
            NumberOfQuarters = numberOfQuarters;
            ThresholdValue = thresholdValue;
            Receive<QuarterCompleted>(message => Handle(message));
        }

        private int _numberOfHighQuarters;
        private decimal _totalConsumption;
        public void Handle(QuarterCompleted message)
        {
            if (message.Consumption >= ThresholdValue)
            {
                AddQuarter(message);
            }
            else
            {
                ResetCounter();
            }
        }

        private void ResetCounter()
        {
            _numberOfHighQuarters = 0;
            _totalConsumption = 0;
        }

        private void AddQuarter(QuarterCompleted message)
        {
            _numberOfHighQuarters++;
            _totalConsumption += message.Consumption;

            if (_numberOfHighQuarters >= NumberOfQuarters)
            {
                WriteToDatabase(message);
            }
        }

        private void WriteToDatabase(QuarterCompleted message)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                var parameters = new
                {
                    message.DeviceId,
                    message.Timestamp,
                    Consumption = _totalConsumption,
                    Duration = _numberOfHighQuarters
                };
                connection.Execute(@"INSERT INTO [dbo].[HighConsumptionAlerts] ([DeviceId], [Timestamp], [Consumption], [Duration])
                                     VALUES (@DeviceId, @Timestamp, @Consumption, @Duration)", parameters);
            }
        }
    }
}