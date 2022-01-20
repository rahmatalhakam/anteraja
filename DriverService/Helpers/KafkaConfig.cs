namespace DriverService.Helpers
{
  public class KafkaConfig
  {

    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public int NumPartitions { get; set; }
    public short ReplicationFactor { get; set; }
  }
}