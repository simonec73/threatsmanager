//#define LOGGING

using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Contracts;

#if LOGGING
// This file contains registration of aspects that are applied to several classes of this project.
[assembly: Log(AttributePriority = 1, AttributeTargetMemberAttributes = MulticastAttributes.Protected | MulticastAttributes.Internal | MulticastAttributes.Public | MulticastAttributes.Private)]
[assembly: Log(AttributePriority = 2, AttributeExclude = true, AttributeTargetMembers = "get_*")]
#endif

[assembly: AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Threading, TargetType = typeof(NotNullAttribute)) ]
[assembly: AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Threading, TargetType = typeof(RequiredAttribute)) ]