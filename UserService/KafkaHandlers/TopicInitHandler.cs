using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Constants;
using UserService.Helpers;

namespace UserService.Handlers
{
  public class TopicInitHandler
  {
    private readonly KafkaConfig _kafkaConfig;
    private readonly ILogger<TopicInitHandler> _loggging;

    public TopicInitHandler(IOptions<KafkaConfig> kafkaConfig, ILogger<TopicInitHandler> loggging)
    {
      _kafkaConfig = kafkaConfig.Value;
      _loggging = loggging;
    }
    public async Task TopicInit()
    {

      var config = new ProducerConfig
      {
        BootstrapServers = _kafkaConfig.BootstrapServers,
        ClientId = Dns.GetHostName(),
      };
      using (var adminClient = new AdminClientBuilder(config).Build())
      {
        List<string> topics = new List<string>();
        topics.Add(TopicList.OrderTopic);
        foreach (var topic in topics)
        {
          try
          {
            await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                new TopicSpecification {
                    Name = topic,
                    NumPartitions = _kafkaConfig.NumPartitions,
                    ReplicationFactor = _kafkaConfig.ReplicationFactor
                } });
            _loggging.LogInformation($"Topic {topic} created successfully");
          }
          catch (CreateTopicsException e)
          {
            if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
            {
              _loggging.LogError($"An error occured creating topic {topic}: {e.Results[0].Error.Reason}");
            }
            else
            {
              _loggging.LogInformation($"Topic {topic} already exists");
            }
          }
        }

      }
    }
  }
}