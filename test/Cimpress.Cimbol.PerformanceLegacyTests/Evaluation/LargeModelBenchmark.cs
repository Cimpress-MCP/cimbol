using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Containers;
using Cimpress.Cimbol.Engine;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Types;

namespace Cimpress.Cimbol.PerformanceLegacyTests.Evaluation
{
    public class LargeModelBenchmark
    {
        private IContainer[] _arguments;

        private Executable _executable;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var program = new Program();

            program.CreateArgument("Argument01", StringType.String);
            program.CreateArgument("Argument02", StringType.String);
            program.CreateArgument("Argument03", StringType.String);
            program.CreateArgument("Argument04", StringType.String);
            program.CreateArgument("Argument05", StringType.String);
            program.CreateArgument("Argument06", StringType.String);
            program.CreateArgument("Argument07", StringType.String);
            program.CreateArgument("Argument08", StringType.String);
            program.CreateArgument("Argument09", StringType.String);
            program.CreateArgument("Argument10", StringType.String);
            program.CreateArgument("Argument11", StringType.String);
            program.CreateArgument("Argument12", StringType.String);
            program.CreateArgument("Argument13", StringType.String);
            program.CreateArgument("Argument14", StringType.String);
            program.CreateArgument("Argument15", StringType.String);
            program.CreateArgument("Argument16", StringType.String);

            program.CreateConstant("Constant01", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup01Function));
            program.CreateConstant("Constant02", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup02Function));
            program.CreateConstant("Constant03", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup03Function));
            program.CreateConstant("Constant04", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup04Function));
            program.CreateConstant("Constant05", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup05Function));
            program.CreateConstant("Constant06", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup06Function));
            program.CreateConstant("Constant07", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup07Function));
            program.CreateConstant("Constant08", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup08Function));
            program.CreateConstant("Constant09", new FunctionContainer((Func<IType, Task<ObjectContainer>>)Lookup09Function));
            program.CreateConstant("Math", MakeMathModule());

            var module = program.CreateSubprogram("Main");
            module.AddResultVariable(
                "OptionGroup01",
                "object(Option01 = Argument01)");
            module.AddResultVariable(
                "OptionGroup02",
                "object(Option02 = Argument02, Option03 = Argument03, Option04 = Argument04, Option05 = Argument05, Option06 = Argument06, Option07 = Argument07, Option08 = Argument08, Option09 = Argument09, Option10 = Argument10, Option11 = Argument11, Option12 = Argument12, Option13 = Argument13, Option14 = Argument14, Option15 = Argument15, Option16 = Argument16)");
            module.AddResultVariable(
                "Lookup01",
                "await Constant01()");
            module.AddResultVariable(
                "Lookup02",
                "await Constant02()");
            module.AddResultVariable(
                "Lookup03",
                "await Constant03()");
            module.AddResultVariable(
                "Lookup04",
                "await Constant04()");
            module.AddResultVariable(
                "Lookup05",
                "await Constant05()");
            module.AddResultVariable(
                "Lookup06",
                "await Constant06()");
            module.AddResultVariable(
                "Lookup07",
                "await Constant07()");
            module.AddResultVariable(
                "Lookup08",
                "await Constant08()");
            module.AddResultVariable(
                "Lookup09",
                "await Constant09()");
            module.AddResultVariable(
                "Formula001",
                "where(case = OptionGroup02.Option10 == \"Value159\" and Lookup01.LookupField01, do = Lookup01.LookupField02 * OptionGroup02.Option13, case =  OptionGroup02.Option10 == \"Value159\" and Lookup01.LookupField01 == false, do = Math.Min(Formula002, Formula003) * 2.5, else = Math.Min(Formula002, Formula003))");
            module.AddResultVariable(
                "Formula002",
                "Formula102 + Formula103 + Formula104 + Formula105 + Formula106 + Formula107 + Formula108 + Formula109");
            module.AddResultVariable(
                "Formula003",
                "where(case = OptionGroup02.Option10 ==  \"Value173\", do = Formula004 + Formula033 + Formula042 + Formula050 + Formula068 + Formula077 + Formula007 + Formula094 + Formula012 + Formula011, case = OptionGroup02.Option10 ==  \"Value178\", do = Formula004 + Formula033 + Formula042 + Formula050 + Formula068 + Formula077 + Formula007 + Formula094 + Formula012 + Formula011, case = OptionGroup02.Option10 !=  \"Value178\", do = Formula004 + Formula005 + Formula006 + Formula009 + Formula008 + Formula012 + Formula011 + Formula010, case = OptionGroup02.Option10 !=  \"Value173\", do = Formula004 + Formula005 + Formula006 + Formula009 + Formula008 + Formula012 + Formula011 + Formula010, else = 0)");
            module.AddResultVariable(
                "Formula004",
                "Formula013 + Formula018 + Formula022 + Formula017");
            module.AddResultVariable(
                "Formula005",
                "Formula033 + Formula036 + Formula039 + Formula042 + Formula050 + Formula052 + Formula045 + Formula186");
            module.AddResultVariable(
                "Formula006",
                "Formula068 + Formula071 + Formula074 + Formula077 + Formula080 + Formula192");
            module.AddResultVariable(
                "Formula007",
                "where(case = OptionGroup02.Option10 ==  \"Value173\" and OptionGroup01.Option01 <= 1000, do = Lookup07.LookupField43, case = OptionGroup02.Option10 ==  \"Value173\" and OptionGroup01.Option01 > 1000, do = Lookup07.LookupField43 + (OptionGroup01.Option01 - 1000) * Lookup07.LookupField42, case = OptionGroup02.Option10 ==  \"Value178\" and OptionGroup01.Option01 <= 1000, do = 50, case = OptionGroup02.Option10 ==  \"Value178\" and OptionGroup01.Option01 > 1000, do = 50 + (OptionGroup01.Option01 - 1000) * 0.025, else = 0)");
            module.AddResultVariable(
                "Formula008",
                "where(case = OptionGroup02.Option16 ==  \"Value207\" and Formula029 <= 1000, do = 250 / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value207\" and Formula029 > 1000, do = (250 + (Formula029 - 1000) * 0.05) / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value206\" and Formula029 <= 1000, do = 125 / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value206\" and Formula029 > 1000, do = (125 + (Formula029 - 1000) * 0.05) / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value186\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula009",
                "Formula094 + Formula095");
            module.AddResultVariable(
                "Formula010",
                "OptionGroup01.Option01 * Lookup08.LookupField46");
            module.AddResultVariable(
                "Formula011",
                "Formula101 * Formula097");
            module.AddResultVariable(
                "Formula012",
                "where(case = Formula097 <= 18, do = 3.28 + (Formula097 - 1) * 2.25, case = Formula097 > 18, do = Formula100 * 45, else = 0)");
            module.AddResultVariable(
                "Formula013",
                "Formula014 + Formula015 + Formula016");
            module.AddResultVariable(
                "Formula014",
                "(Formula028 * Lookup04.LookupField18) / Lookup02.LookupField04 * (Formula032)");
            module.AddResultVariable(
                "Formula015",
                "(Formula029 / Lookup02.LookupField04) * (1 + Formula023) * Lookup04.LookupField18 * (Formula030)");
            module.AddResultVariable(
                "Formula016",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula019 * 0.13, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula017",
                "Formula020 + Formula021");
            module.AddResultVariable(
                "Formula018",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = Formula013 * 0.05351, case = OptionGroup02.Option14 ==  \"Value158\", do = Formula013 * 0.05851, else = Formula013 * 0)");
            module.AddResultVariable(
                "Formula019",
                "OptionGroup01.Option01 / Lookup02.LookupField04");
            module.AddResultVariable(
                "Formula020",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField27 / Lookup02.LookupField04 * Formula032)");
            module.AddResultVariable(
                "Formula021",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Formula029 / Lookup02.LookupField04 * Lookup05.LookupField31 * Formula030)");
            module.AddResultVariable(
                "Formula022",
                "(Formula024 * Formula026 * (1 + Formula025)) / Lookup02.LookupField04 * (Formula032)");
            module.AddResultVariable(
                "Formula023",
                "0.057");
            module.AddResultVariable(
                "Formula024",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = 5, case = OptionGroup02.Option14 ==  \"Value158\", do = 9, else = 0)");
            module.AddResultVariable(
                "Formula025",
                "0.1");
            module.AddResultVariable(
                "Formula026",
                "2.3");
            module.AddResultVariable(
                "Formula027",
                "where(case = OptionGroup02.Option10 ==  \"String01\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField04 * OptionGroup02.Option07, case = OptionGroup02.Option10 !=  \"String01\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField04, else = 0)");
            module.AddResultVariable(
                "Formula028",
                "112");
            module.AddResultVariable(
                "Formula029",
                "where(case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 == 400 and OptionGroup01.Option01 * OptionGroup02.Option07 > 2500, do = 2500, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * OptionGroup02.Option07 <= 999, do = 500, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * 25 >= 1000 and OptionGroup01.Option01 * OptionGroup02.Option07 < 2000, do = 1000, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * 25 >= 2000 and OptionGroup01.Option01 * OptionGroup02.Option07 < 2500, do = 2000, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * 25 >= 2500 and OptionGroup01.Option01 * OptionGroup02.Option07 < 5000, do = 2500, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 < 400 and OptionGroup01.Option01 * OptionGroup02.Option07 >= 5000, do = 5000, case = Lookup04.LookupField17 == 400 and OptionGroup01.Option01 <= 250 and OptionGroup02.Option10 !=  \"Value154\", do = 250, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 <= 999, do = 500, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 >= 1000 and OptionGroup01.Option01 < 2000, do = 1000, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 >= 2000 and OptionGroup01.Option01 < 2500, do = 2000, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 >= 2500 and OptionGroup01.Option01 < 5000, do = 2500, case = Lookup04.LookupField17 <= 350 and OptionGroup01.Option01 >= 5000, do = 5000, case = Lookup04.LookupField17 == 400 and OptionGroup01.Option01 >= 5000, do = 2500, case = OptionGroup02.Option10 ==  \"Value179\" and Lookup04.LookupField17 == 450 and OptionGroup01.Option01 >= 2500, do = 2500, case = Lookup04.LookupField17 == 450, do = 250, else = 0)");
            module.AddResultVariable(
                "Formula030",
                "Math.Ceil(Formula031)");
            module.AddResultVariable(
                "Formula031",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option07 * OptionGroup02.Option13 / Formula029, case = OptionGroup02.Option10 ==  \"Value137\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Formula029, case = OptionGroup02.Option10 !=  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Formula029, else = 0)");
            module.AddResultVariable(
                "Formula032",
                "where(case = Formula030 > Lookup02.LookupField04, do = Lookup02.LookupField04, case = Formula030 <= Lookup02.LookupField04, do = Formula030, else = 0)");
            module.AddResultVariable(
                "Formula033",
                "Formula034 + Formula035");
            module.AddResultVariable(
                "Formula034",
                "(Formula051 * Formula055 * Formula056 * Formula057) / Lookup02.LookupField04 * (Formula032)");
            module.AddResultVariable(
                "Formula035",
                "((Formula029 / Lookup02.LookupField04) / Formula058) * (Formula059 * Formula056) * (Formula030)");
            module.AddResultVariable(
                "Formula036",
                "Formula037 + Formula038");
            module.AddResultVariable(
                "Formula037",
                "(Formula060 * Formula062 * Formula061 * Formula057) / Lookup02.LookupField04 * (Formula032)");
            module.AddResultVariable(
                "Formula038",
                "((Formula029 / Lookup02.LookupField04) / Formula063) * (Formula064 * Formula061) * (Formula030)");
            module.AddResultVariable(
                "Formula039",
                "Formula040 + Formula041");
            module.AddResultVariable(
                "Formula040",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula067 * Formula062 * Formula061 * Formula057, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula041",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField04 / Formula066 * Formula064 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula042",
                "Formula043 + Formula044");
            module.AddResultVariable(
                "Formula043",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField26 / Lookup02.LookupField04 * Formula032)");
            module.AddResultVariable(
                "Formula044",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Formula029 / Lookup02.LookupField04 / Lookup05.LookupField29 * Lookup05.LookupField30 * Formula030)");
            module.AddResultVariable(
                "Formula045",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula046 + Formula048, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula046 + Formula048, else = 0)");
            module.AddResultVariable(
                "Formula046",
                "Formula047");
            module.AddResultVariable(
                "Formula047",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 2.17, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 2.17, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 2.17, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 2.82, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 3.25, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 3.25, case = OptionGroup02.Option09 == 1, do = 1.04, case = OptionGroup02.Option09 == 2, do = 1.04, case = OptionGroup02.Option09 == 3, do = 1.04, case = OptionGroup02.Option09 == 4, do = 2.17, case = OptionGroup02.Option09 == 5, do = 2.17, case = OptionGroup02.Option09 == 6, do = 2.17, case = OptionGroup02.Option06 == 1, do = 1.04, case = OptionGroup02.Option06 == 2, do = 1.04, case = OptionGroup02.Option06 == 3, do = 1.04, case = OptionGroup02.Option06 == 4, do = 1.04, case = OptionGroup02.Option06 == 5, do = 2.17, case = OptionGroup02.Option06 == 6, do = 2.17, else = 0)");
            module.AddResultVariable(
                "Formula048",
                "Formula049 * Formula180");
            module.AddResultVariable(
                "Formula049",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 0.00173, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 0.00173, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 0.00173, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 0.00173, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 0.00217, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 0.00217, case = OptionGroup02.Option09 == 1, do = 0.00163, case = OptionGroup02.Option09 == 2, do = 0.00163, case = OptionGroup02.Option09 == 3, do = 0.00163, case = OptionGroup02.Option09 == 4, do = 0.00217, case = OptionGroup02.Option09 == 5, do = 0.00217, case = OptionGroup02.Option09 == 6, do = 0.00217, case = OptionGroup02.Option06 == 1, do = 0.00163, case = OptionGroup02.Option06 == 2, do = 0.00163, case = OptionGroup02.Option06 == 3, do = 0.00163, case = OptionGroup02.Option06 == 4, do = 0.00217, case = OptionGroup02.Option06 == 5, do = 0.00217, case = OptionGroup02.Option06 == 6, do = 0.00217, else = 0)");
            module.AddResultVariable(
                "Formula050",
                "Formula097 * Formula099 * Formula065");
            module.AddResultVariable(
                "Formula051",
                "0.18");
            module.AddResultVariable(
                "Formula052",
                "Formula053 + Formula054");
            module.AddResultVariable(
                "Formula053",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField07 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula054",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField06 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula055",
                "2");
            module.AddResultVariable(
                "Formula056",
                "17");
            module.AddResultVariable(
                "Formula057",
                "1.26");
            module.AddResultVariable(
                "Formula058",
                "11148 / Formula055");
            module.AddResultVariable(
                "Formula059",
                "1.057");
            module.AddResultVariable(
                "Formula060",
                "0.25");
            module.AddResultVariable(
                "Formula061",
                "13");
            module.AddResultVariable(
                "Formula062",
                "1");
            module.AddResultVariable(
                "Formula063",
                "4056 / Formula062");
            module.AddResultVariable(
                "Formula064",
                "1");
            module.AddResultVariable(
                "Formula065",
                "10");
            module.AddResultVariable(
                "Formula066",
                "550");
            module.AddResultVariable(
                "Formula067",
                "0.05");
            module.AddResultVariable(
                "Formula068",
                "Formula069 + Formula070");
            module.AddResultVariable(
                "Formula069",
                "(Formula085 * Formula086) / Lookup02.LookupField04 * (Formula032)");
            module.AddResultVariable(
                "Formula070",
                "(Formula029 / Lookup02.LookupField04 / Formula087) * (Formula088 * Formula086) * (Formula030)");
            module.AddResultVariable(
                "Formula071",
                "Formula072 + Formula073");
            module.AddResultVariable(
                "Formula072",
                "(Formula089 / Lookup02.LookupField04) * Formula091 * (Formula032)");
            module.AddResultVariable(
                "Formula073",
                "(Formula029 / Lookup02.LookupField04) / (Formula090) * Formula091 * (Formula030)");
            module.AddResultVariable(
                "Formula074",
                "Formula075 + Formula076");
            module.AddResultVariable(
                "Formula075",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula093 * Formula091, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula076",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField04 / Formula066 * Formula064 * Formula091, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula077",
                "Formula078 + Formula079");
            module.AddResultVariable(
                "Formula078",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField25 / Lookup02.LookupField04 * Formula032)");
            module.AddResultVariable(
                "Formula079",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Formula029 / Lookup02.LookupField04 / Lookup05.LookupField29 * Lookup05.LookupField28 * Formula030)");
            module.AddResultVariable(
                "Formula080",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula081 + Formula083, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula081 + Formula083, else = 0)");
            module.AddResultVariable(
                "Formula081",
                "Formula082");
            module.AddResultVariable(
                "Formula082",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 2.34, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 2.34, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 2.34, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 3.04, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 3.50, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 3.50, case = OptionGroup02.Option09 == 1, do = 1.12, case = OptionGroup02.Option09 == 2, do = 1.12, case = OptionGroup02.Option09 == 3, do = 1.12, case = OptionGroup02.Option09 == 4, do = 2.34, case = OptionGroup02.Option09 == 5, do = 2.34, case = OptionGroup02.Option09 == 6, do = 2.34, case = OptionGroup02.Option06 == 1, do = 1.12, case = OptionGroup02.Option06 == 2, do = 1.12, case = OptionGroup02.Option06 == 3, do = 1.12, case = OptionGroup02.Option06 == 4, do = 1.12, case = OptionGroup02.Option06 == 5, do = 2.34, case = OptionGroup02.Option06 == 6, do = 2.34, else = 0)");
            module.AddResultVariable(
                "Formula083",
                "Formula084 * Formula180");
            module.AddResultVariable(
                "Formula084",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 0.00186, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 0.00186, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 0.00186, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 0.00186, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 0.0023, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 0.0023, case = OptionGroup02.Option09 == 1, do = 0.00169, case = OptionGroup02.Option09 == 2, do = 0.00175, case = OptionGroup02.Option09 == 3, do = 0.00175, case = OptionGroup02.Option09 == 4, do = 0.00234, case = OptionGroup02.Option09 == 5, do = 0.00234, case = OptionGroup02.Option09 == 6, do = 0.00234, case = OptionGroup02.Option06 == 1, do = 0.00175, case = OptionGroup02.Option06 == 2, do = 0.00175, case = OptionGroup02.Option06 == 3, do = 0.00175, case = OptionGroup02.Option06 == 4, do = 0.00234, case = OptionGroup02.Option06 == 5, do = 0.00234, case = OptionGroup02.Option06 == 6, do = 0.00234, else = 0)");
            module.AddResultVariable(
                "Formula085",
                "0.18");
            module.AddResultVariable(
                "Formula086",
                "71.4463");
            module.AddResultVariable(
                "Formula087",
                "11148");
            module.AddResultVariable(
                "Formula088",
                "1.057");
            module.AddResultVariable(
                "Formula089",
                "0.25");
            module.AddResultVariable(
                "Formula090",
                "4056");
            module.AddResultVariable(
                "Formula091",
                "15.7210");
            module.AddResultVariable(
                "Formula092",
                "9");
            module.AddResultVariable(
                "Formula093",
                "0.05");
            module.AddResultVariable(
                "Formula094",
                "(Formula005 + Formula006) * 0.091168");
            module.AddResultVariable(
                "Formula095",
                "(Formula004 + Formula005 + Formula006) * 0.164069");
            module.AddResultVariable(
                "Formula096",
                "if(OptionGroup02.Option10 ==  \"Value173\", then = OptionGroup01.Option01 / Lookup07.LookupField41, else = (((Lookup02.LookupField03 * Lookup02.LookupField02) / 1000000)) * ((Lookup04.LookupField17 / 1000) * Formula180) / Formula098)");
            module.AddResultVariable(
                "Formula097",
                "if(OptionGroup02.Option02 !=  \"Value186\" and OptionGroup01.Option01 > Lookup08.LookupField45, then = Math.Ceil(Formula096) + 1, else = Math.Ceil(Formula096))");
            module.AddResultVariable(
                "Formula098",
                "20");
            module.AddResultVariable(
                "Formula099",
                "0.05");
            module.AddResultVariable(
                "Formula100",
                "if(Formula097 >= 20, then = Formula097 / 50, else = 0)");
            module.AddResultVariable(
                "Formula101",
                "0.38");
            module.AddResultVariable(
                "Formula102",
                "Formula110 + Formula114 + Formula116");
            module.AddResultVariable(
                "Formula103",
                "Formula123 + Formula126 + Formula132 + Formula137 + Formula134 + Formula186 + Formula138");
            module.AddResultVariable(
                "Formula104",
                "Formula155 + Formula158 + Formula164 + Formula167 + Formula192");
            module.AddResultVariable(
                "Formula105",
                "Formula178 + Formula179");
            module.AddResultVariable(
                "Formula106",
                "where(case = OptionGroup02.Option16 ==  \"Value207\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 <= 1000, do = 250, case = OptionGroup02.Option16 ==  \"Value207\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 > 1000, do = 250 + (OptionGroup01.Option01 / Lookup02.LookupField05 - 1000) * 0.05, case = OptionGroup02.Option16 ==  \"Value206\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 <= 1000, do = 125, case = OptionGroup02.Option16 ==  \"Value206\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 > 1000, do = 125 + (OptionGroup01.Option01 / Lookup02.LookupField05 - 1000) * 0.05, case = OptionGroup02.Option16 ==  \"Value186\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula107",
                "where(case = Formula182 < 17, do = 3.28 + (Formula182 - 1) * 2.25, case = Formula182 >= 17, do = Formula184 * 45, else = 0)");
            module.AddResultVariable(
                "Formula108",
                "Formula183 * Formula182");
            module.AddResultVariable(
                "Formula109",
                "where(case = OptionGroup02.Option10 !=  \"Value174\", do = 0, case = OptionGroup02.Option02 !=  \"Value186\", do = OptionGroup01.Option01 * Lookup08.LookupField46, else = 0)");
            module.AddResultVariable(
                "Formula110",
                "Formula112 + Formula113 + Formula111");
            module.AddResultVariable(
                "Formula111",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula115 * 0.0325, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula112",
                "Formula120 * Lookup04.LookupField19");
            module.AddResultVariable(
                "Formula113",
                "Formula185 * (1 + Formula121) * Lookup04.LookupField19");
            module.AddResultVariable(
                "Formula114",
                "Formula122 * Lookup03.LookupField13");
            module.AddResultVariable(
                "Formula115",
                "OptionGroup01.Option01 / Lookup02.LookupField05");
            module.AddResultVariable(
                "Formula116",
                "Formula117 + Formula118");
            module.AddResultVariable(
                "Formula117",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField34)");
            module.AddResultVariable(
                "Formula118",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 * Lookup05.LookupField33)");
            module.AddResultVariable(
                "Formula119",
                "Math.Ceil(OptionGroup02.Option13 / Lookup02.LookupField05)");
            module.AddResultVariable(
                "Formula120",
                "5 * Formula119");
            module.AddResultVariable(
                "Formula121",
                "0.01");
            module.AddResultVariable(
                "Formula122",
                "Formula185 * Lookup03.LookupField14");
            module.AddResultVariable(
                "Formula123",
                "Formula124 + Formula125");
            module.AddResultVariable(
                "Formula124",
                "Formula141 * Formula142 * Formula143 * Formula144");
            module.AddResultVariable(
                "Formula125",
                "((OptionGroup01.Option01 / Lookup02.LookupField05) / Formula145) * (Formula146 * Formula143)");
            module.AddResultVariable(
                "Formula126",
                "if(OptionGroup02.Option10 ==  \"Value211\", then = 0, else = Formula127 + Formula128 + Formula129)");
            module.AddResultVariable(
                "Formula127",
                "(Formula148 * Formula149 * Formula150 * Formula144)");
            module.AddResultVariable(
                "Formula128",
                "Formula185 / (Formula151) * (Formula153 * Formula150)");
            module.AddResultVariable(
                "Formula129",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula130 + Formula131, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula130",
                "(Formula148 * Formula149 * Formula150 * Formula144)");
            module.AddResultVariable(
                "Formula131",
                "Formula115 / (Formula152) * (Formula153 * Formula150)");
            module.AddResultVariable(
                "Formula132",
                "Formula133 + Formula147");
            module.AddResultVariable(
                "Formula133",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField26)");
            module.AddResultVariable(
                "Formula134",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula135 + Formula136, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula135 + Formula136, else = 0)");
            module.AddResultVariable(
                "Formula135",
                "Formula047");
            module.AddResultVariable(
                "Formula136",
                "Formula049 * OptionGroup01.Option01 * OptionGroup02.Option13");
            module.AddResultVariable(
                "Formula137",
                "Formula182 * Formula099 * Formula154");
            module.AddResultVariable(
                "Formula138",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula139 + Formula140, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula139",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField07 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula140",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField06 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula141",
                "0.05");
            module.AddResultVariable(
                "Formula142",
                "1");
            module.AddResultVariable(
                "Formula143",
                "13");
            module.AddResultVariable(
                "Formula144",
                "1.26");
            module.AddResultVariable(
                "Formula145",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = 2700, case = OptionGroup02.Option14 ==  \"Value158\", do = 1500, else = 0)");
            module.AddResultVariable(
                "Formula146",
                "1.01");
            module.AddResultVariable(
                "Formula147",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 / Lookup05.LookupField32 * Lookup05.LookupField30)");
            module.AddResultVariable(
                "Formula148",
                "0.05");
            module.AddResultVariable(
                "Formula149",
                "1");
            module.AddResultVariable(
                "Formula150",
                "13");
            module.AddResultVariable(
                "Formula151",
                "4056");
            module.AddResultVariable(
                "Formula152",
                "550");
            module.AddResultVariable(
                "Formula153",
                "1");
            module.AddResultVariable(
                "Formula154",
                "10");
            module.AddResultVariable(
                "Formula155",
                "Formula156 + Formula157");
            module.AddResultVariable(
                "Formula156",
                "(Formula170 * Formula171)");
            module.AddResultVariable(
                "Formula157",
                "Formula185 / (Formula172) * (Formula173 * Formula171)");
            module.AddResultVariable(
                "Formula158",
                "if(OptionGroup02.Option10 ==  \"Value211\", then = 0, else = Formula159 + Formula160 + Formula161)");
            module.AddResultVariable(
                "Formula159",
                "Formula174 * Formula175");
            module.AddResultVariable(
                "Formula160",
                "Formula185 / (Formula176) * Formula175");
            module.AddResultVariable(
                "Formula161",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula162 + Formula163, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula162",
                "Formula177 * Formula091");
            module.AddResultVariable(
                "Formula163",
                "Formula115 / Formula066 * Formula064 * Formula091");
            module.AddResultVariable(
                "Formula164",
                "Formula165 + Formula166");
            module.AddResultVariable(
                "Formula165",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField25)");
            module.AddResultVariable(
                "Formula166",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 / Lookup05.LookupField32 * Lookup05.LookupField28)");
            module.AddResultVariable(
                "Formula167",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula168 + Formula169, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula168 + Formula169, else = 0)");
            module.AddResultVariable(
                "Formula168",
                "Formula082");
            module.AddResultVariable(
                "Formula169",
                "Formula084 * OptionGroup01.Option01 * OptionGroup02.Option13");
            module.AddResultVariable(
                "Formula170",
                "0.05");
            module.AddResultVariable(
                "Formula171",
                "0.427");
            module.AddResultVariable(
                "Formula172",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = 2700, case = OptionGroup02.Option14 ==  \"Value158\", do = 1500, else = 0)");
            module.AddResultVariable(
                "Formula173",
                "1.01");
            module.AddResultVariable(
                "Formula174",
                "0.05");
            module.AddResultVariable(
                "Formula175",
                "15.721");
            module.AddResultVariable(
                "Formula176",
                "4056");
            module.AddResultVariable(
                "Formula177",
                "0.05");
            module.AddResultVariable(
                "Formula178",
                "(Formula103 + Formula104) * 0.291");
            module.AddResultVariable(
                "Formula179",
                "(Formula102 + Formula103 + Formula104) * 0.164069");
            module.AddResultVariable(
                "Formula180",
                "Formula029 * Formula030");
            module.AddResultVariable(
                "Formula181",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = (((Lookup02.LookupField03 * Lookup02.LookupField02) / 1000000)) * ((Lookup04.LookupField17 / 1000) * (OptionGroup01.Option01 * OptionGroup02.Option13 * OptionGroup02.Option07)) / Formula098, case = OptionGroup02.Option10 !=  \"Value154\", do = (((Lookup02.LookupField03 * Lookup02.LookupField02) / 1000000)) * ((Lookup04.LookupField17 / 1000) * (OptionGroup01.Option01 * OptionGroup02.Option13)) / Formula098)");
            module.AddResultVariable(
                "Formula182",
                "if(OptionGroup02.Option02 !=  \"Value186\" and OptionGroup01.Option01 > Lookup08.LookupField45, then = Math.Ceil(Formula181) + 1, else = Math.Ceil(Formula181))");
            module.AddResultVariable(
                "Formula183",
                "0.38");
            module.AddResultVariable(
                "Formula184",
                "if(Formula182 >= 17, then = Formula182 / 28, else = 0)");
            module.AddResultVariable(
                "Formula185",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 * OptionGroup02.Option07, case = OptionGroup02.Option10 !=  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05, else = 0)");
            module.AddResultVariable(
                "Formula186",
                "Formula187 + Formula188");
            module.AddResultVariable(
                "Formula187",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula189 * Formula190, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula188",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula191 * Formula190 * Formula180, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula189",
                "0.08");
            module.AddResultVariable(
                "Formula190",
                "13");
            module.AddResultVariable(
                "Formula191",
                "0.000232");
            module.AddResultVariable(
                "Formula192",
                "Formula193 + Formula196");
            module.AddResultVariable(
                "Formula193",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula194 * Formula195, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula194",
                "0.08");
            module.AddResultVariable(
                "Formula195",
                "2.249");
            module.AddResultVariable(
                "Formula196",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula197 * Formula195 * Formula180, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddResultVariable(
                "Formula197",
                "0.000232");
            module.AddResultVariable(
                "Formula198",
                "where(case = Formula002 < Formula003, do = Formula185 / 4, case = Formula002 > Formula003, do = Formula027, else = 0)");
            module.AddResultVariable(
                "Formula199",
                "where(case = Formula002 < Formula003, do = Formula182, case = Formula002 > Formula003, do = Formula097, else = 0)");
            module.AddResultVariable(
                "Formula200",
                "where(case = Formula002 < Formula003, do = Formula184, case = Formula002 > Formula003, do = Formula100, else = 0)");
            module.AddResultVariable(
                "Formula201",
                "where(case = Formula002 < Formula003, do = Formula002, case = Formula002 > Formula003, do = Formula003, else = 0)");
            module.AddResultVariable(
                "Formula202",
                "where(case = OptionGroup02.Option10 ==  \"Value154\" and (OptionGroup01.Option01 * OptionGroup02.Option07) < 250, do =  \"String02\", case = OptionGroup02.Option10 ==  \"Value154\" and (OptionGroup01.Option01 * OptionGroup02.Option07) >= 250, do =  \"String03\", case = OptionGroup01.Option01 < 250 and OptionGroup02.Option15 !=  \"Value080\" and OptionGroup02.Option15 !=  \"Value079\" and OptionGroup02.Option10 !=  \"Value173\" and OptionGroup02.Option10 !=  \"Value178\" and Lookup04.LookupField17 != 450, do =  \"String02\", case = OptionGroup02.Option10 ==  \"Value173\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value178\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value141\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value180\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value137\" and OptionGroup02.Option08 ==  \"Value051\" and OptionGroup01.Option01 <= 500, do =  \"String02\", case = Formula002 < Formula003, do =  \"String02\", case = Formula002 > Formula003, do =  \"String03\", else =  \"Value186\")");

            try
            {
                _executable = program.Compile("Main");
            }
            catch (CimbolException e)
            {
                Console.WriteLine("{0}", e.Message);
                throw;
            }

            _arguments = new IContainer[]
            {
                new StringContainer("0"), 
                new StringContainer("Value186"),
                new StringContainer("Value103"),
                new StringContainer("Value194"),
                new StringContainer("Value186"),
                new StringContainer("0"),
                new StringContainer("0"),
                new StringContainer("Value008"),
                new StringContainer("0"),
                new StringContainer("Value180"),
                new StringContainer("Value213"),
                new StringContainer("0"),
                new StringContainer("0"),
                new StringContainer("Value203"),
                new StringContainer("Value005"),
                new StringContainer("Value186"),
            };

            var testResult = _executable.Call(_arguments).Result;
            if (testResult.ErrorContainer != null)
            {
                var error = testResult.ErrorContainer.Value;
                Console.WriteLine("{0} {1}", error.RootPath, error.Message);
                throw new Exception(error.Message);
            }
        }

        [Benchmark]
        public async Task<EvaluationResult> Benchmark_FormulaAsync()
        {
            return await _executable.Call(_arguments);
        }

        private Task<ObjectContainer> Lookup01Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField01", new BooleanContainer(false) },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup02Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField02", new NumberContainer(210) },
                { "LookupField03", new NumberContainer(97.66m) },
                { "LookupField04", new NumberContainer(24) },
                { "LookupField05", new NumberContainer(6) },
                { "LookupField06", new NumberContainer(0) },
                { "LookupField07", new NumberContainer(0) },
                { "LookupField08", new BooleanContainer(true) },
                { "LookupField09", new StringContainer("0.0205086") },
                { "LookupField10", new StringContainer("0") },
                { "LookupField11", new StringContainer("0") },
                { "LookupField12", new StringContainer("0") },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup03Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField13", new NumberContainer(0.00328m) },
                { "LookupField14", new NumberContainer(4) },
                { "LookupField15", new BooleanContainer(true) },
                { "LookupField16", new StringContainer("0") },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup04Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField17", new NumberContainer(100) },
                { "LookupField18", new NumberContainer(0.03686m) },
                { "LookupField19", new NumberContainer(0.01888m) },
                { "LookupField20", new BooleanContainer(true) },
                { "LookupField21", new StringContainer("0") },
                { "LookupField22", new StringContainer("0") },
                { "LookupField23", new StringContainer("0") },
                { "LookupField24", new StringContainer("0") },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup05Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField25", new NumberContainer(1) },
                { "LookupField26", new NumberContainer(1) },
                { "LookupField27", new NumberContainer(1) },
                { "LookupField28", new NumberContainer(1) },
                { "LookupField29", new NumberContainer(1) },
                { "LookupField30", new NumberContainer(1) },
                { "LookupField31", new NumberContainer(1) },
                { "LookupField32", new NumberContainer(1) },
                { "LookupField33", new NumberContainer(1) },
                { "LookupField34", new NumberContainer(1) },
                { "LookupField35", new BooleanContainer(true) },
                { "LookupField36", new StringContainer("0") },
                { "LookupField37", new StringContainer("1") },
                { "LookupField38", new StringContainer("1") },
                { "LookupField39", new StringContainer("0") },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup06Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField40", new BooleanContainer(false) },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup07Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField41", new NumberContainer(150) },
                { "LookupField42", new NumberContainer(0.065m) },
                { "LookupField43", new NumberContainer(90) },
                { "LookupField44", new BooleanContainer(true) },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup08Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField45", new NumberContainer(0) },
                { "LookupField46", new NumberContainer(0) },
                { "LookupField47", new BooleanContainer(true) },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private Task<ObjectContainer> Lookup09Function(IType type)
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField48", new BooleanContainer(false) },
            };

            return Task.FromResult(new ObjectContainer(objectValueContents));
        }

        private ObjectContainer MakeMathModule()
        {
            var objectValueContents = new Dictionary<string, IContainer>(StringComparer.OrdinalIgnoreCase)
            {
                { "Ceil", new FunctionContainer((Func<IType, decimal, decimal>)CeilFunction) },
                { "Min", new FunctionContainer((Func<IType, decimal, decimal, decimal>)MinFunction) },
            };

            return new ObjectContainer(objectValueContents);
        }

        private decimal CeilFunction(IType type, decimal value)
        {
            return Math.Ceiling(value);
        }

        private decimal MinFunction(IType type, decimal leftValue, decimal rightValue)
        {
            return leftValue + rightValue;
        }
    }
}