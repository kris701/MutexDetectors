
<p align="center">
    <img src="https://github.com/kris701/MutexDetectors/assets/22596587/c3099504-3137-4e2b-b3dc-c5f9b3b4b90a" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/MutexDetectors/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/MutexDetectors/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/MutexDetectors)
![Nuget](https://img.shields.io/nuget/dt/MutexDetectors)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/MutexDetectors/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/MutexDetectors)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

# Mutex Detectors
This project is a collection of mutex detectors.

You can use this package by the CLI tool:
```
dotnet run --domain domain.pddl --problem problem.pddl --detector EffectBalance
```

You can also find this project as a package on the [NuGet Package Manager](https://www.nuget.org/packages/MutexDetectors).

## Effect Balance Mutex Detector

There is a simple predicate mutex detector included in PDDLSharp.
It is capable of finding "balanced" predicates in a PDDLDecl, and assumes they are mutexes.
The general process is:
1. Let C be a new list of candidate mutexes
2. Foreach action
   - Get all predicates in the action effects
   - Count how many `add` and `del` there is for the predicate name
   - If the amount of `add` and `del` is in balance, 
      - Then add the predice to C
      - Else, if C contains this predicate, remove it from C and blacklist it from reentering
3. Return C

As an example, for the `gripper` domain, the predicate `at-robby` is "effect balanced". Since that:
* The action `move`'s effects adds 1 and removes 1 `at-robby`
* The action `pick`'s effects does not touch the predicate
* The action `drop`'s effects does not touch the predicate

Hence, there was just as many `add` as `del` of the predicate `at-robby`, so its assumed to be a mutex.

### Examples
An example of how to find mutexes in a domain:
```csharp
IErrorListener listener = new ErrorListener();
IParser<INode> parser = new PDDLParser(listener);
PDDLDecl decl = new PDDLDecl(...)

IMutexDetectors detector = new EffectBalanceMutexes();
List<PredicateExp> mutexes = detector.FindMutexes(decl);
```