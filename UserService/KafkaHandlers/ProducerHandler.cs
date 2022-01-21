using System;
using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Helpers;

namespace UserService.KafkaHandlers
{
  public class ProducerHandler
  {
    private readonly ILogger<ProducerHandler> _logging;
    private readonly KafkaConfig _kafkaConfig;

    public ProducerHandler(ILogger<ProducerHandler> logging, IOptions<KafkaConfig> kafkaConfig)
    {
      _logging = logging;
      _kafkaConfig = kafkaConfig.Value;
    }
    public async Task<Boolean> ProduceMessage(string _topic, string key, string value)
    {
      var isSucceed = false;
      var topic = _topic;
      var config = new ProducerConfig
      {
        BootstrapServers = _kafkaConfig.BootstrapServers,
        ClientId = Dns.GetHostName(),
      };
      using (var producer = new ProducerBuilder<string, string>(config).Build())
      {

        producer.Produce(topic, new Message<string, string>
        {
          Key = key,
          Value = value
        }, (deliveryReport) =>
        {
          if (deliveryReport.Error.Code != ErrorCode.NoError)
          {
            _logging.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
          }
          else
          {
            _logging.LogInformation($"Produced message to: {deliveryReport.TopicPartitionOffset} Key: {key} Value: {value}");
            isSucceed = true;
          }
        });
        producer.Flush(TimeSpan.FromSeconds(10));
      }
      return await Task.FromResult(isSucceed);
    }
  }
}