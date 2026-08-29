using System;
using TwoWaySync.Services;

namespace TwoWaySync
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var repo = new MappingRepository("redacted and private");
            var mapping = repo.GetEntityByLocalId("Task", new Guid("4d9d892d-ae4a-4800-b08e-263328169e0a"));
            Console.WriteLine($"Fetched LocalId: {mapping?.LocalId}");
        }
    }
}