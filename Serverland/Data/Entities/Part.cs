namespace Serverland.Data.Entities;

public class Part
{
    public int Id { get; set; }
    public required string CPU { get; set; }
    public required string RAM { get; set; }
    public required string Raid { get; set; }
    public required string Network { get; set; }
    public string SSD { get; set; }
    public string HDD { get; set; }
    public required string PSU { get; set; }
    public required bool Rails { get; set; }
    public int serverId { get; set; }
}