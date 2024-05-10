using CommandLine;

namespace MutexDetectors.CLI
{
    public class Options
    {
        [Option("domain", Required = true, HelpText = "Path to the domain file")]
        public string DomainPath { get; set; } = "";
        [Option("problem", Required = true, HelpText = "Path to the problem file")]
        public string ProblemPath { get; set; } = "";
        [Option("detector", Required = true, HelpText = "What detector to use")]
        public DetectorOptions DetectorOption { get; set; }

        [Option("out", Required = false, HelpText = "Target path to output mutex groups", Default = "out")]
        public string OutPath { get; set; } = "out";
    }
}
