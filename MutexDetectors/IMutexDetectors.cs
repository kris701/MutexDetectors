using PDDLSharp.Models.PDDL;
using PDDLSharp.Models.PDDL.Expressions;

namespace MutexDetectors
{
    public interface IMutexDetectors
    {
        public List<PredicateExp> FindMutexes(PDDLDecl decl);
    }
}
