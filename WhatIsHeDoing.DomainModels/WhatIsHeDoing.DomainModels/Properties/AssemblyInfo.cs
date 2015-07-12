using System;
using System.Reflection;
using System.Resources;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription
    ("A library of domain models for .Net.")]

[assembly: AssemblyProduct("WhatIsHeDoing.DomainModels")]
[assembly: AssemblyTitle("WhatIsHeDoing.DomainModels")]
[assembly: AssemblyVersion("0.1.0")]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en")]
