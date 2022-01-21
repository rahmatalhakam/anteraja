namespace DriverService.KafkaHandlers
{
  using Microsoft.Extensions.Hosting;
  using System.Threading;
  using System.Threading.Tasks;
  using System;
  using Confluent.Kafka;
  using System.Text.Json;
  using Microsoft.Extensions.Options;
  using DriverService.Helpers;
  using Microsoft.Extensions.Logging;
  using DriverService.Constants;
  using DriverService.Data.Orders;
  using DriverService.Data;
  using Microsoft.Extensions.Configuration;
  using Microsoft.EntityFrameworkCore;
  using DriverService.Models;

  public class ProcessOrdersService : BackgroundService
  {
    private readonly ConsumerConfig _consumerConfig;
    private readonly KafkaConfig _kafkaConfig;
    private readonly ILogger<ProcessOrdersService> _logging;
    private readonly string _connString;
    private readonly ProducerConfig _producerConfig;
    public ProcessOrdersService(ConsumerConfig consumerConfig,
                                ProducerConfig producerConfig,
                                IOptions<KafkaConfig> kafkaConfig,
                                ILogger<ProcessOrdersService> logging,
                                IConfiguration configuration
                                )
    {
      _producerConfig = producerConfig;
      _consumerConfig = consumerConfig;
      _kafkaConfig = kafkaConfig.Value;
      _logging = logging;
      _connString = configuration.GetConnectionString("DatabaseConn");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await Task.Run(async () =>
       {
         string topic = TopicList.OrderTopic;
         _logging.LogInformation($"Consumer handler started. Listening to topic: {topic}.");
         var conf = new ConsumerConfig
         {
           GroupId = _kafkaConfig.GroupId,
           BootstrapServers = _kafkaConfig.BootstrapServers,
           AutoOffsetReset = AutoOffsetReset.Earliest
         };
         using (var builder = new ConsumerBuilder<Ignore, string>(conf).Build())
         {
           builder.Subscribe(topic);
           var cancelToken = new CancellationTokenSource();
           try
           {
             while (true)
             {
               var consumer = builder.Consume(cancelToken.Token);
               _logging.LogInformation($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
               Order order = JsonSerializer.Deserialize<Order>(consumer.Message.Value);
               using (var context = new KafkaDbContext(_connString))
               {
                 try
                 {
                   var result = await context.Orders.AddAsync(order);
                   await context.SaveChangesAsync();
                 }
                 catch (System.Exception ex)
                 {
                   Console.WriteLine(ex);
                 }

                 //    var result = context.Users.Where(u => u.Id == input.UserID && u.Lock == false).SingleOrDefault();
                 //    if (result == null) return false;
                 //    var result2 = context.TwittorModels.Where(t => t.Id == input.TwittorModelID).SingleOrDefault();
                 //    if (result2 == null) return false;
                 //    var comment = new Comment
                 //    {
                 //      User = result,
                 //      TwittorModel = result2,
                 //      Description = input.Description,
                 //      CreatedAt = DateTime.Now,

                 //    };
                 //    await context.Comments.AddAsync(comment);
                 //    await context.SaveChangesAsync();
                 //    return true;
               }
             }
           }
           catch (Exception)
           {
             builder.Close();
           }
         }
       }, stoppingToken);
    }
  }
}