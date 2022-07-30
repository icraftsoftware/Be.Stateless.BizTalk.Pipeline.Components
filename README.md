# Be.Stateless.BizTalk.Pipeline.Components

##### Build Pipelines

[![][pipeline.mr.badge]][pipeline.mr]

[![][pipeline.ci.badge]][pipeline.ci]

##### Latest Release

[![][nuget.badge]][nuget]

[![][nuget.unit.badge]][nuget.unit]

[![][nuget.nunit.badge]][nuget.nunit]

[![][nuget.xunit.badge]][nuget.xunit]

[![][release.badge]][release]

##### Release Preview

[![][nuget.preview.badge]][nuget.preview]

[![][nuget.unit.preview.badge]][nuget.unit.preview]

[![][nuget.nunit.preview.badge]][nuget.nunit.preview]

[![][nuget.xunit.preview.badge]][nuget.xunit.preview]

##### Documentation

[![][doc.main.badge]][doc.main]

[![][doc.this.badge]][doc.this]

[![][help.badge]][help]

[![][help.unit.badge]][help.unit]

[![][help.nunit.badge]][help.nunit]

[![][help.xunit.badge]][help.xunit]

## Overview

`Be.Stateless.BizTalk.Pipeline.Components` is part of the [BizTalk.Factory Application][biztalk.factory.application] Package. This component provides very lightweight yet tremendously helpful Microsoft BizTalk Server® pipeline components. It is accompanied by a set of unit-testing helper components meant to support the developer in writing custom pipeline components and their unit tests.

<!-- badges -->

[doc.main.badge]: https://img.shields.io/static/v1?label=BizTalk.Factory%20SDK&message=User's%20Guide&color=8CA1AF&logo=readthedocs
[doc.main]: https://www.stateless.be/ "BizTalk.Factory SDK User's Guide"
[doc.this.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Pipeline.Components&message=User's%20Guide&color=8CA1AF&logo=readthedocs
[doc.this]: https://www.stateless.be/BizTalk/Pipeline/Components "Be.Stateless.BizTalk.Pipeline.Components User's Guide"
[github.badge]: https://img.shields.io/static/v1?label=Repository&message=Be.Stateless.BizTalk.Pipeline.Components&logo=github
[github]: https://github.com/icraftsoftware/Be.Stateless.BizTalk.Pipeline.Components "Be.Stateless.BizTalk.Pipeline.Components GitHub Repository"
[help.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Pipeline.Components&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Pipeline/Components/README.md "Be.Stateless.BizTalk.Pipeline.Components Developer Help"
[help.nunit.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Pipeline.Components.NUnit&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help.nunit]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Pipeline/Components/NUnit/README.md "Be.Stateless.BizTalk.Pipeline.Components.NUnit Developer Help"
[help.unit.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Pipeline.Components.Unit&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help.unit]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Pipeline/Components/Unit/README.md "Be.Stateless.BizTalk.Pipeline.Components.Unit Developer Help"
[help.xunit.badge]: https://img.shields.io/static/v1?label=Be.Stateless.BizTalk.Pipeline.Components.XUnit&message=Developer%20Help&color=8CA1AF&logo=microsoftacademic
[help.xunit]: https://github.com/icraftsoftware/biztalk.factory.github.io/blob/master/Help/BizTalk/Pipeline/Components/XUnit/README.md "Be.Stateless.BizTalk.Pipeline.Components.XUnit Developer Help"
[nuget.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Pipeline.Components.svg?label=Be.Stateless.BizTalk.Pipeline.Components&style=flat&logo=nuget
[nuget]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Pipeline.Components "Be.Stateless.BizTalk.Pipeline.Components NuGet Package"
[nuget.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Pipeline.Components?logo=nuget
[nuget.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Pipeline.Components&protocolType=NuGet "Be.Stateless.BizTalk.Pipeline.Components Preview NuGet Package"
[nuget.nunit.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Pipeline.Components.NUnit.svg?label=Be.Stateless.BizTalk.Pipeline.Components.NUnit&style=flat&logo=nuget
[nuget.nunit]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Pipeline.Components.NUnit "Be.Stateless.BizTalk.Pipeline.Components.NUnit NuGet Package"
[nuget.nunit.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Pipeline.Components.NUnit?logo=nuget
[nuget.nunit.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Pipeline.Components.NUnit&protocolType=NuGet "Be.Stateless.BizTalk.Pipeline.Components.NUnit Preview NuGet Package"
[nuget.unit.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Pipeline.Components.Unit.svg?label=Be.Stateless.BizTalk.Pipeline.Components.Unit&style=flat&logo=nuget
[nuget.unit]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Pipeline.Components.Unit "Be.Stateless.BizTalk.Pipeline.Components.Unit NuGet Package"
[nuget.unit.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Pipeline.Components.Unit?logo=nuget
[nuget.unit.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Pipeline.Components.Unit&protocolType=NuGet "Be.Stateless.BizTalk.Pipeline.Components.Unit Preview NuGet Package"
[nuget.xunit.badge]: https://img.shields.io/nuget/v/Be.Stateless.BizTalk.Pipeline.Components.XUnit.svg?label=Be.Stateless.BizTalk.Pipeline.Components.XUnit&style=flat&logo=nuget
[nuget.xunit]: https://www.nuget.org/packages/Be.Stateless.BizTalk.Pipeline.Components.XUnit "Be.Stateless.BizTalk.Pipeline.Components.XUnit NuGet Package"
[nuget.xunit.preview.badge]: https://badge-factory.azurewebsites.net/package/icraftsoftware/be.stateless/BizTalk.Factory.Preview/Be.Stateless.BizTalk.Pipeline.Components.XUnit?logo=nuget
[nuget.xunit.preview]: https://dev.azure.com/icraftsoftware/be.stateless/_packaging?_a=package&feed=BizTalk.Factory.Preview&package=Be.Stateless.BizTalk.Pipeline.Components.XUnit&protocolType=NuGet "Be.Stateless.BizTalk.Pipeline.Components.XUnit Preview NuGet Package"
[pipeline.ci.badge]: https://dev.azure.com/icraftsoftware/be.stateless/_apis/build/status/Be.Stateless.BizTalk.Pipeline.Components%20Continuous%20Integration?branchName=master&label=Continuous%20Integration%20Build
[pipeline.ci]: https://dev.azure.com/icraftsoftware/be.stateless/_build/latest?definitionId=37&branchName=master "Be.Stateless.BizTalk.Pipeline.Components Continuous Integration Build Pipeline"
[pipeline.mr.badge]: https://dev.azure.com/icraftsoftware/be.stateless/_apis/build/status/Be.Stateless.BizTalk.Pipeline.Components%20Manual%20Release?branchName=master&label=Manual%20Release%20Build
[pipeline.mr]: https://dev.azure.com/icraftsoftware/be.stateless/_build/latest?definitionId=38&branchName=master "Be.Stateless.BizTalk.Pipeline.Components Manual Release Build Pipeline"
[release.badge]: https://img.shields.io/github/v/release/icraftsoftware/Be.Stateless.BizTalk.Pipeline.Components?label=Release&logo=github
[release]: https://github.com/icraftsoftware/Be.Stateless.BizTalk.Pipeline.Components/releases/latest "Be.Stateless.BizTalk.Pipeline.Components Release"

<!-- links -->

[biztalk.factory.application]: https://www.stateless.be/BizTalk/Factory/Application "BizTalk.Factory Application"
