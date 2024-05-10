using PDDLSharp.Models.PDDL;
using PDDLSharp.Models.PDDL.Expressions;

namespace MutexDetectors
{
    /// <summary>
    /// Main interface for mutex detectors
    /// </summary>
    public interface IMutexDetectors
    {
        /// <summary>
        /// Method to find a set of mutexed predicates from a PDDL declaration
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        public List<PredicateExp> FindMutexes(PDDLDecl decl);
    }
}
