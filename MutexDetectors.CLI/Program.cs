using CommandLine;
using CommandLine.Text;
using PDDLSharp.CodeGenerators.FastDownward.Plans;
using PDDLSharp.CodeGenerators.PDDL;
using PDDLSharp.ErrorListeners;
using PDDLSharp.Models.PDDL;
using PDDLSharp.Models.PDDL.Domain;
using PDDLSharp.Models.PDDL.Problem;
using PDDLSharp.Parsers.PDDL;
using PDDLSharp.Translators;

namespace MutexDetectors.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);
            parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
            parserResult.WithParsed(Run);
        }

        public static void Run(Options opts)
        {
            opts.DomainPath = RootPath(opts.DomainPath);
            opts.ProblemPath = RootPath(opts.ProblemPath);
            opts.OutPath = RootPath(opts.OutPath);
            if (Directory.Exists(opts.OutPath))
                Directory.Delete(opts.OutPath, true);

            Console.WriteLine("Parsing files...");
            var listener = new ErrorListener();
            var pddlParser = new PDDLParser(listener);
            var domain = pddlParser.ParseAs<DomainDecl>(new FileInfo(opts.DomainPath));
            var problem = pddlParser.ParseAs<ProblemDecl>(new FileInfo(opts.ProblemPath));
            var pddlDecl = new PDDLDecl(domain, problem);

            Console.WriteLine("Building mutex detector...");
            var detector = MutexDetectorBuilder.GetDetector(opts.DetectorOption);

            Console.WriteLine("Finding mutexes...");
            var groups = detector.FindMutexes(pddlDecl);

            Console.WriteLine("Outputting mutexes...");
            var codeGenerator = new PDDLCodeGenerator(listener);
            foreach (var item in groups)
                codeGenerator.Generate(item, $"{item.Name}.pddl");
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            var sentenceBuilder = SentenceBuilder.Create();
            foreach (var error in errs)
                if (error is not HelpRequestedError)
                    Console.WriteLine(sentenceBuilder.FormatError(error));
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AddEnumValuesToHelpText = true;
                return h;
            }, e => e, verbsIndex: true);
            Console.WriteLine(helpText);
            HandleParseError(errs);
        }

        private static string RootPath(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Join(Directory.GetCurrentDirectory(), path);
            path = path.Replace("\\", "/");
            return path;
        }
    }
}