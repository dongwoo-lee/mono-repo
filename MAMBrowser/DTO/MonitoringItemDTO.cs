using System;
using System.Security.Policy;

namespace MAMBrowser.DTO
{
    public class MonitoringItemDTO<T>
    {
        public PcInfo systemMetrics { get; set; }
        public T agentMetrics {get; set;}
    }

    public class PcInfo
    {
        public string SystemInfo { get; set; }
        public string Processor { get; set; }
        public string SystemVersion { get; set; }
        public string RAM { get; set; }
        public string DiskInfo { get; set; }
        public string HostName { get; set; }
        public string DomainName { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public string MemoryUsage { get; set; }
        public string DiskIO { get; set; }
        public string NetworkIO { get; set; }
    }
    public enum ConnectionStatus
    {
        Offline,
        Online,
        Error
    }
    public enum DeviceType
    {
        M,S,B
    }
    public class AgentSoftwareInfo
    {
        public string Status { get; set; }
        public string SoftwareVersion { get; set; }
        public string LoginId { get; set; }
        public string LoginName { get; set; }
    }
    public class SLAPInfo:AgentSoftwareInfo
    {
        public string DeviceName { get; set; }
        // Main, Sub, BackUp
        public string Type { get; set; }
        public string StudioName { get; set; }
        public string EventName { get; set; }
        public string ProductId { get; set; }
        public int FloorInfo { get; set; }
        public DateTime PgmStartTime { get; set; }
        public DateTime PgmEndTime { get; set; }
        public string TD { get; set; }
    }
    public class DCMInfo :AgentSoftwareInfo
    {
        public string TotalItemsCount { get; set; }
        public string CompletedItemsCount { get; set; }
    }
    public class DL3Info : AgentSoftwareInfo { }
    public class MMBInfo : AgentSoftwareInfo 
    { 
        public string Memory { get; set; }
    }
    public class dynamicData
    {

    }
}
