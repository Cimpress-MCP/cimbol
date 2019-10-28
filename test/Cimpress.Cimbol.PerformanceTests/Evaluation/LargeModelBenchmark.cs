using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Evaluation
{
    public class LargeModelBenchmark
    {
        private ILocalValue[] _arguments;

        private Executable _executable;

        [ParamsSource(nameof(CompilationProfiles))]
        public CompilationProfile CompilationProfile { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var program = new Program();
            var argument01 = program.AddArgument("Argument01");
            var argument02 = program.AddArgument("Argument02");
            var argument03 = program.AddArgument("Argument03");
            var argument04 = program.AddArgument("Argument04");
            var argument05 = program.AddArgument("Argument05");
            var argument06 = program.AddArgument("Argument06");
            var argument07 = program.AddArgument("Argument07");
            var argument08 = program.AddArgument("Argument08");
            var argument09 = program.AddArgument("Argument09");
            var argument10 = program.AddArgument("Argument10");
            var argument11 = program.AddArgument("Argument11");
            var argument12 = program.AddArgument("Argument12");
            var argument13 = program.AddArgument("Argument13");
            var argument14 = program.AddArgument("Argument14");
            var argument15 = program.AddArgument("Argument15");
            var argument16 = program.AddArgument("Argument16");

            var constant01 = program.AddConstant("Constant01", new FunctionValue((Func<PromiseValue>)Lookup01Function));
            var constant02 = program.AddConstant("Constant02", new FunctionValue((Func<PromiseValue>)Lookup02Function));
            var constant03 = program.AddConstant("Constant03", new FunctionValue((Func<PromiseValue>)Lookup03Function));
            var constant04 = program.AddConstant("Constant04", new FunctionValue((Func<PromiseValue>)Lookup04Function));
            var constant05 = program.AddConstant("Constant05", new FunctionValue((Func<PromiseValue>)Lookup05Function));
            var constant06 = program.AddConstant("Constant06", new FunctionValue((Func<PromiseValue>)Lookup06Function));
            var constant07 = program.AddConstant("Constant07", new FunctionValue((Func<PromiseValue>)Lookup07Function));
            var constant08 = program.AddConstant("Constant08", new FunctionValue((Func<PromiseValue>)Lookup08Function));
            var constant09 = program.AddConstant("Constant09", new FunctionValue((Func<PromiseValue>)Lookup09Function));
            var constant10 = program.AddConstant("Math", MakeMathModule());

            var module01 = program.AddModule("Lookup01");
            module01.AddImport("Lookup01Function", constant01);
            var lookup01 = module01.AddFormula("Lookup01", "await Lookup01Function()");

            var module02 = program.AddModule("Lookup02");
            module02.AddImport("Lookup02Function", constant02);
            var lookup02 = module02.AddFormula("Lookup02", "await Lookup02Function()");

            var module03 = program.AddModule("Lookup03");
            module03.AddImport("Lookup03Function", constant03);
            var lookup03 = module03.AddFormula("Lookup03", "await Lookup03Function()");

            var module04 = program.AddModule("Lookup04");
            module04.AddImport("Lookup04Function", constant04);
            var lookup04 = module04.AddFormula("Lookup04", "await Lookup04Function()");

            var module05 = program.AddModule("Lookup05");
            module05.AddImport("Lookup05Function", constant05);
            var lookup05 = module05.AddFormula("Lookup05", "await Lookup05Function()");

            var module06 = program.AddModule("Lookup06");
            module06.AddImport("Lookup06Function", constant06);
            var lookup06 = module06.AddFormula("Lookup06", "await Lookup06Function()");

            var module07 = program.AddModule("Lookup07");
            module07.AddImport("Lookup07Function", constant07);
            var lookup07 = module07.AddFormula("Lookup07", "await Lookup07Function()");

            var module08 = program.AddModule("Lookup08");
            module08.AddImport("Lookup08Function", constant08);
            var lookup08 = module08.AddFormula("Lookup08", "await Lookup08Function()");

            var module09 = program.AddModule("Lookup09");
            module09.AddImport("Lookup09Function", constant09);
            var lookup09 = module09.AddFormula("Lookup09", "await Lookup09Function()");

            var module10 = program.AddModule("OptionGroup01");
            module10.AddImport("Argument01", argument01);
            module10.AddFormula("Option01", "Argument01");

            var module11 = program.AddModule("OptionGroup02");
            module11.AddImport("Argument02", argument02);
            module11.AddImport("Argument03", argument03);
            module11.AddImport("Argument04", argument04);
            module11.AddImport("Argument05", argument05);
            module11.AddImport("Argument06", argument06);
            module11.AddImport("Argument07", argument07);
            module11.AddImport("Argument08", argument08);
            module11.AddImport("Argument09", argument09);
            module11.AddImport("Argument10", argument10);
            module11.AddImport("Argument11", argument11);
            module11.AddImport("Argument12", argument12);
            module11.AddImport("Argument13", argument13);
            module11.AddImport("Argument14", argument14);
            module11.AddImport("Argument15", argument15);
            module11.AddImport("Argument16", argument16);
            module11.AddFormula("Option02", "Argument02");
            module11.AddFormula("Option03", "Argument03");
            module11.AddFormula("Option04", "Argument04");
            module11.AddFormula("Option05", "Argument05");
            module11.AddFormula("Option06", "Argument06");
            module11.AddFormula("Option07", "Argument07");
            module11.AddFormula("Option08", "Argument08");
            module11.AddFormula("Option09", "Argument09");
            module11.AddFormula("Option10", "Argument10");
            module11.AddFormula("Option11", "Argument11");
            module11.AddFormula("Option12", "Argument12");
            module11.AddFormula("Option13", "Argument13");
            module11.AddFormula("Option14", "Argument14");
            module11.AddFormula("Option15", "Argument15");
            module11.AddFormula("Option16", "Argument16");

            var module = program.AddModule("Main");
            module.AddImport("Lookup01", lookup01);
            module.AddImport("Lookup02", lookup02);
            module.AddImport("Lookup03", lookup03);
            module.AddImport("Lookup04", lookup04);
            module.AddImport("Lookup05", lookup05);
            module.AddImport("Lookup06", lookup06);
            module.AddImport("Lookup07", lookup07);
            module.AddImport("Lookup08", lookup08);
            module.AddImport("Lookup09", lookup09);
            module.AddImport("OptionGroup01", module10);
            module.AddImport("OptionGroup02", module11);
            module.AddImport("Math", constant10);
            module.AddFormula(
                "Formula001",
                "where(case = OptionGroup02.Option10 == \"Value159\" and Lookup01.LookupField01, do = Lookup01.LookupField02 * OptionGroup02.Option13, case =  OptionGroup02.Option10 == \"Value159\" and Lookup01.LookupField01 == false, do = Math.Min(Formula002, Formula003) * 2.5, else = Math.Min(Formula002, Formula003))");
            module.AddFormula(
                "Formula002",
                "Formula102 + Formula103 + Formula104 + Formula105 + Formula106 + Formula107 + Formula108 + Formula109");
            module.AddFormula(
                "Formula003",
                "where(case = OptionGroup02.Option10 ==  \"Value173\", do = Formula004 + Formula033 + Formula042 + Formula050 + Formula068 + Formula077 + Formula007 + Formula094 + Formula012 + Formula011, case = OptionGroup02.Option10 ==  \"Value178\", do = Formula004 + Formula033 + Formula042 + Formula050 + Formula068 + Formula077 + Formula007 + Formula094 + Formula012 + Formula011, case = OptionGroup02.Option10 !=  \"Value178\", do = Formula004 + Formula005 + Formula006 + Formula009 + Formula008 + Formula012 + Formula011 + Formula010, case = OptionGroup02.Option10 !=  \"Value173\", do = Formula004 + Formula005 + Formula006 + Formula009 + Formula008 + Formula012 + Formula011 + Formula010, else = 0)");
            module.AddFormula(
                "Formula004",
                "Formula013 + Formula018 + Formula022 + Formula017");
            module.AddFormula(
                "Formula005",
                "Formula033 + Formula036 + Formula039 + Formula042 + Formula050 + Formula052 + Formula045 + Formula186");
            module.AddFormula(
                "Formula006",
                "Formula068 + Formula071 + Formula074 + Formula077 + Formula080 + Formula192");
            module.AddFormula(
                "Formula007",
                "where(case = OptionGroup02.Option10 ==  \"Value173\" and OptionGroup01.Option01 <= 1000, do = Lookup07.LookupField43, case = OptionGroup02.Option10 ==  \"Value173\" and OptionGroup01.Option01 > 1000, do = Lookup07.LookupField43 + (OptionGroup01.Option01 - 1000) * Lookup07.LookupField42, case = OptionGroup02.Option10 ==  \"Value178\" and OptionGroup01.Option01 <= 1000, do = 50, case = OptionGroup02.Option10 ==  \"Value178\" and OptionGroup01.Option01 > 1000, do = 50 + (OptionGroup01.Option01 - 1000) * 0.025, else = 0)");
            module.AddFormula(
                "Formula008",
                "where(case = OptionGroup02.Option16 ==  \"Value207\" and Formula029 <= 1000, do = 250 / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value207\" and Formula029 > 1000, do = (250 + (Formula029 - 1000) * 0.05) / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value206\" and Formula029 <= 1000, do = 125 / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value206\" and Formula029 > 1000, do = (125 + (Formula029 - 1000) * 0.05) / Lookup02.LookupField04 * Formula030, case = OptionGroup02.Option16 ==  \"Value186\", do = 0, else = 0)");
            module.AddFormula(
                "Formula009",
                "Formula094 + Formula095");
            module.AddFormula(
                "Formula010",
                "OptionGroup01.Option01 * Lookup08.LookupField46");
            module.AddFormula(
                "Formula011",
                "Formula101 * Formula097");
            module.AddFormula(
                "Formula012",
                "where(case = Formula097 <= 18, do = 3.28 + (Formula097 - 1) * 2.25, case = Formula097 > 18, do = Formula100 * 45, else = 0)");
            module.AddFormula(
                "Formula013",
                "Formula014 + Formula015 + Formula016");
            module.AddFormula(
                "Formula014",
                "(Formula028 * Lookup04.LookupField18) / Lookup02.LookupField04 * (Formula032)");
            module.AddFormula(
                "Formula015",
                "(Formula029 / Lookup02.LookupField04) * (1 + Formula023) * Lookup04.LookupField18 * (Formula030)");
            module.AddFormula(
                "Formula016",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula019 * 0.13, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula017",
                "Formula020 + Formula021");
            module.AddFormula(
                "Formula018",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = Formula013 * 0.05351, case = OptionGroup02.Option14 ==  \"Value158\", do = Formula013 * 0.05851, else = Formula013 * 0)");
            module.AddFormula(
                "Formula019",
                "OptionGroup01.Option01 / Lookup02.LookupField04");
            module.AddFormula(
                "Formula020",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField27 / Lookup02.LookupField04 * Formula032)");
            module.AddFormula(
                "Formula021",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Formula029 / Lookup02.LookupField04 * Lookup05.LookupField31 * Formula030)");
            module.AddFormula(
                "Formula022",
                "(Formula024 * Formula026 * (1 + Formula025)) / Lookup02.LookupField04 * (Formula032)");
            module.AddFormula(
                "Formula023",
                "0.057");
            module.AddFormula(
                "Formula024",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = 5, case = OptionGroup02.Option14 ==  \"Value158\", do = 9, else = 0)");
            module.AddFormula(
                "Formula025",
                "0.1");
            module.AddFormula(
                "Formula026",
                "2.3");
            module.AddFormula(
                "Formula027",
                "where(case = OptionGroup02.Option10 ==  \"String01\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField04 * OptionGroup02.Option07, case = OptionGroup02.Option10 !=  \"String01\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField04, else = 0)");
            module.AddFormula(
                "Formula028",
                "112");
            module.AddFormula(
                "Formula029",
                "where(case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 == 400 and OptionGroup01.Option01 * OptionGroup02.Option07 > 2500, do = 2500, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * OptionGroup02.Option07 <= 999, do = 500, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * 25 >= 1000 and OptionGroup01.Option01 * OptionGroup02.Option07 < 2000, do = 1000, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * 25 >= 2000 and OptionGroup01.Option01 * OptionGroup02.Option07 < 2500, do = 2000, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 * 25 >= 2500 and OptionGroup01.Option01 * OptionGroup02.Option07 < 5000, do = 2500, case = OptionGroup02.Option10 ==  \"Value154\" and Lookup04.LookupField17 < 400 and OptionGroup01.Option01 * OptionGroup02.Option07 >= 5000, do = 5000, case = Lookup04.LookupField17 == 400 and OptionGroup01.Option01 <= 250 and OptionGroup02.Option10 !=  \"Value154\", do = 250, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 <= 999, do = 500, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 >= 1000 and OptionGroup01.Option01 < 2000, do = 1000, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 >= 2000 and OptionGroup01.Option01 < 2500, do = 2000, case = Lookup04.LookupField17 <= 400 and OptionGroup01.Option01 >= 2500 and OptionGroup01.Option01 < 5000, do = 2500, case = Lookup04.LookupField17 <= 350 and OptionGroup01.Option01 >= 5000, do = 5000, case = Lookup04.LookupField17 == 400 and OptionGroup01.Option01 >= 5000, do = 2500, case = OptionGroup02.Option10 ==  \"Value179\" and Lookup04.LookupField17 == 450 and OptionGroup01.Option01 >= 2500, do = 2500, case = Lookup04.LookupField17 == 450, do = 250, else = 0)");
            module.AddFormula(
                "Formula030",
                "Math.Ceil(Formula031)");
            module.AddFormula(
                "Formula031",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option07 * OptionGroup02.Option13 / Formula029, case = OptionGroup02.Option10 ==  \"Value137\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Formula029, case = OptionGroup02.Option10 !=  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Formula029, else = 0)");
            module.AddFormula(
                "Formula032",
                "where(case = Formula030 > Lookup02.LookupField04, do = Lookup02.LookupField04, case = Formula030 <= Lookup02.LookupField04, do = Formula030, else = 0)");
            module.AddFormula(
                "Formula033",
                "Formula034 + Formula035");
            module.AddFormula(
                "Formula034",
                "(Formula051 * Formula055 * Formula056 * Formula057) / Lookup02.LookupField04 * (Formula032)");
            module.AddFormula(
                "Formula035",
                "((Formula029 / Lookup02.LookupField04) / Formula058) * (Formula059 * Formula056) * (Formula030)");
            module.AddFormula(
                "Formula036",
                "Formula037 + Formula038");
            module.AddFormula(
                "Formula037",
                "(Formula060 * Formula062 * Formula061 * Formula057) / Lookup02.LookupField04 * (Formula032)");
            module.AddFormula(
                "Formula038",
                "((Formula029 / Lookup02.LookupField04) / Formula063) * (Formula064 * Formula061) * (Formula030)");
            module.AddFormula(
                "Formula039",
                "Formula040 + Formula041");
            module.AddFormula(
                "Formula040",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula067 * Formula062 * Formula061 * Formula057, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula041",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField04 / Formula066 * Formula064 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula042",
                "Formula043 + Formula044");
            module.AddFormula(
                "Formula043",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField26 / Lookup02.LookupField04 * Formula032)");
            module.AddFormula(
                "Formula044",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Formula029 / Lookup02.LookupField04 / Lookup05.LookupField29 * Lookup05.LookupField30 * Formula030)");
            module.AddFormula(
                "Formula045",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula046 + Formula048, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula046 + Formula048, else = 0)");
            module.AddFormula(
                "Formula046",
                "Formula047");
            module.AddFormula(
                "Formula047",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 2.17, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 2.17, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 2.17, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 2.82, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 3.25, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 3.25, case = OptionGroup02.Option09 == 1, do = 1.04, case = OptionGroup02.Option09 == 2, do = 1.04, case = OptionGroup02.Option09 == 3, do = 1.04, case = OptionGroup02.Option09 == 4, do = 2.17, case = OptionGroup02.Option09 == 5, do = 2.17, case = OptionGroup02.Option09 == 6, do = 2.17, case = OptionGroup02.Option06 == 1, do = 1.04, case = OptionGroup02.Option06 == 2, do = 1.04, case = OptionGroup02.Option06 == 3, do = 1.04, case = OptionGroup02.Option06 == 4, do = 1.04, case = OptionGroup02.Option06 == 5, do = 2.17, case = OptionGroup02.Option06 == 6, do = 2.17, else = 0)");
            module.AddFormula(
                "Formula048",
                "Formula049 * Formula180");
            module.AddFormula(
                "Formula049",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 0.00173, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 0.00173, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 0.00173, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 0.00173, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 0.00217, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 0.00217, case = OptionGroup02.Option09 == 1, do = 0.00163, case = OptionGroup02.Option09 == 2, do = 0.00163, case = OptionGroup02.Option09 == 3, do = 0.00163, case = OptionGroup02.Option09 == 4, do = 0.00217, case = OptionGroup02.Option09 == 5, do = 0.00217, case = OptionGroup02.Option09 == 6, do = 0.00217, case = OptionGroup02.Option06 == 1, do = 0.00163, case = OptionGroup02.Option06 == 2, do = 0.00163, case = OptionGroup02.Option06 == 3, do = 0.00163, case = OptionGroup02.Option06 == 4, do = 0.00217, case = OptionGroup02.Option06 == 5, do = 0.00217, case = OptionGroup02.Option06 == 6, do = 0.00217, else = 0)");
            module.AddFormula(
                "Formula050",
                "Formula097 * Formula099 * Formula065");
            module.AddFormula(
                "Formula051",
                "0.18");
            module.AddFormula(
                "Formula052",
                "Formula053 + Formula054");
            module.AddFormula(
                "Formula053",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField07 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula054",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField06 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula055",
                "2");
            module.AddFormula(
                "Formula056",
                "17");
            module.AddFormula(
                "Formula057",
                "1.26");
            module.AddFormula(
                "Formula058",
                "11148 / Formula055");
            module.AddFormula(
                "Formula059",
                "1.057");
            module.AddFormula(
                "Formula060",
                "0.25");
            module.AddFormula(
                "Formula061",
                "13");
            module.AddFormula(
                "Formula062",
                "1");
            module.AddFormula(
                "Formula063",
                "4056 / Formula062");
            module.AddFormula(
                "Formula064",
                "1");
            module.AddFormula(
                "Formula065",
                "10");
            module.AddFormula(
                "Formula066",
                "550");
            module.AddFormula(
                "Formula067",
                "0.05");
            module.AddFormula(
                "Formula068",
                "Formula069 + Formula070");
            module.AddFormula(
                "Formula069",
                "(Formula085 * Formula086) / Lookup02.LookupField04 * (Formula032)");
            module.AddFormula(
                "Formula070",
                "(Formula029 / Lookup02.LookupField04 / Formula087) * (Formula088 * Formula086) * (Formula030)");
            module.AddFormula(
                "Formula071",
                "Formula072 + Formula073");
            module.AddFormula(
                "Formula072",
                "(Formula089 / Lookup02.LookupField04) * Formula091 * (Formula032)");
            module.AddFormula(
                "Formula073",
                "(Formula029 / Lookup02.LookupField04) / (Formula090) * Formula091 * (Formula030)");
            module.AddFormula(
                "Formula074",
                "Formula075 + Formula076");
            module.AddFormula(
                "Formula075",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula093 * Formula091, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula076",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField04 / Formula066 * Formula064 * Formula091, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula077",
                "Formula078 + Formula079");
            module.AddFormula(
                "Formula078",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField25 / Lookup02.LookupField04 * Formula032)");
            module.AddFormula(
                "Formula079",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Formula029 / Lookup02.LookupField04 / Lookup05.LookupField29 * Lookup05.LookupField28 * Formula030)");
            module.AddFormula(
                "Formula080",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula081 + Formula083, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula081 + Formula083, else = 0)");
            module.AddFormula(
                "Formula081",
                "Formula082");
            module.AddFormula(
                "Formula082",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 2.34, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 2.34, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 2.34, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 3.04, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 3.50, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 3.50, case = OptionGroup02.Option09 == 1, do = 1.12, case = OptionGroup02.Option09 == 2, do = 1.12, case = OptionGroup02.Option09 == 3, do = 1.12, case = OptionGroup02.Option09 == 4, do = 2.34, case = OptionGroup02.Option09 == 5, do = 2.34, case = OptionGroup02.Option09 == 6, do = 2.34, case = OptionGroup02.Option06 == 1, do = 1.12, case = OptionGroup02.Option06 == 2, do = 1.12, case = OptionGroup02.Option06 == 3, do = 1.12, case = OptionGroup02.Option06 == 4, do = 1.12, case = OptionGroup02.Option06 == 5, do = 2.34, case = OptionGroup02.Option06 == 6, do = 2.34, else = 0)");
            module.AddFormula(
                "Formula083",
                "Formula084 * Formula180");
            module.AddFormula(
                "Formula084",
                "where(case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 1, do = 0.00186, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 2, do = 0.00186, case = OptionGroup02.Option06 == 1 and OptionGroup02.Option09 == 3, do = 0.00186, case = OptionGroup02.Option06 == 2 and OptionGroup02.Option09 == 2, do = 0.00186, case = OptionGroup02.Option06 == 3 and OptionGroup02.Option09 == 3, do = 0.0023, case = OptionGroup02.Option06 == 4 and OptionGroup02.Option09 == 4, do = 0.0023, case = OptionGroup02.Option09 == 1, do = 0.00169, case = OptionGroup02.Option09 == 2, do = 0.00175, case = OptionGroup02.Option09 == 3, do = 0.00175, case = OptionGroup02.Option09 == 4, do = 0.00234, case = OptionGroup02.Option09 == 5, do = 0.00234, case = OptionGroup02.Option09 == 6, do = 0.00234, case = OptionGroup02.Option06 == 1, do = 0.00175, case = OptionGroup02.Option06 == 2, do = 0.00175, case = OptionGroup02.Option06 == 3, do = 0.00175, case = OptionGroup02.Option06 == 4, do = 0.00234, case = OptionGroup02.Option06 == 5, do = 0.00234, case = OptionGroup02.Option06 == 6, do = 0.00234, else = 0)");
            module.AddFormula(
                "Formula085",
                "0.18");
            module.AddFormula(
                "Formula086",
                "71.4463");
            module.AddFormula(
                "Formula087",
                "11148");
            module.AddFormula(
                "Formula088",
                "1.057");
            module.AddFormula(
                "Formula089",
                "0.25");
            module.AddFormula(
                "Formula090",
                "4056");
            module.AddFormula(
                "Formula091",
                "15.7210");
            module.AddFormula(
                "Formula092",
                "9");
            module.AddFormula(
                "Formula093",
                "0.05");
            module.AddFormula(
                "Formula094",
                "(Formula005 + Formula006) * 0.091168");
            module.AddFormula(
                "Formula095",
                "(Formula004 + Formula005 + Formula006) * 0.164069");
            module.AddFormula(
                "Formula096",
                "if(OptionGroup02.Option10 ==  \"Value173\", then = OptionGroup01.Option01 / Lookup07.LookupField41, else = (((Lookup02.LookupField03 * Lookup02.LookupField02) / 1000000)) * ((Lookup04.LookupField17 / 1000) * Formula180) / Formula098)");
            module.AddFormula(
                "Formula097",
                "if(OptionGroup02.Option02 !=  \"Value186\" and OptionGroup01.Option01 > Lookup08.LookupField45, then = Math.Ceil(Formula096) + 1, else = Math.Ceil(Formula096))");
            module.AddFormula(
                "Formula098",
                "20");
            module.AddFormula(
                "Formula099",
                "0.05");
            module.AddFormula(
                "Formula100",
                "if(Formula097 >= 20, then = Formula097 / 50, else = 0)");
            module.AddFormula(
                "Formula101",
                "0.38");
            module.AddFormula(
                "Formula102",
                "Formula110 + Formula114 + Formula116");
            module.AddFormula(
                "Formula103",
                "Formula123 + Formula126 + Formula132 + Formula137 + Formula134 + Formula186 + Formula138");
            module.AddFormula(
                "Formula104",
                "Formula155 + Formula158 + Formula164 + Formula167 + Formula192");
            module.AddFormula(
                "Formula105",
                "Formula178 + Formula179");
            module.AddFormula(
                "Formula106",
                "where(case = OptionGroup02.Option16 ==  \"Value207\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 <= 1000, do = 250, case = OptionGroup02.Option16 ==  \"Value207\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 > 1000, do = 250 + (OptionGroup01.Option01 / Lookup02.LookupField05 - 1000) * 0.05, case = OptionGroup02.Option16 ==  \"Value206\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 <= 1000, do = 125, case = OptionGroup02.Option16 ==  \"Value206\" and OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 > 1000, do = 125 + (OptionGroup01.Option01 / Lookup02.LookupField05 - 1000) * 0.05, case = OptionGroup02.Option16 ==  \"Value186\", do = 0, else = 0)");
            module.AddFormula(
                "Formula107",
                "where(case = Formula182 < 17, do = 3.28 + (Formula182 - 1) * 2.25, case = Formula182 >= 17, do = Formula184 * 45, else = 0)");
            module.AddFormula(
                "Formula108",
                "Formula183 * Formula182");
            module.AddFormula(
                "Formula109",
                "where(case = OptionGroup02.Option10 !=  \"Value174\", do = 0, case = OptionGroup02.Option02 !=  \"Value186\", do = OptionGroup01.Option01 * Lookup08.LookupField46, else = 0)");
            module.AddFormula(
                "Formula110",
                "Formula112 + Formula113 + Formula111");
            module.AddFormula(
                "Formula111",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula115 * 0.0325, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula112",
                "Formula120 * Lookup04.LookupField19");
            module.AddFormula(
                "Formula113",
                "Formula185 * (1 + Formula121) * Lookup04.LookupField19");
            module.AddFormula(
                "Formula114",
                "Formula122 * Lookup03.LookupField13");
            module.AddFormula(
                "Formula115",
                "OptionGroup01.Option01 / Lookup02.LookupField05");
            module.AddFormula(
                "Formula116",
                "Formula117 + Formula118");
            module.AddFormula(
                "Formula117",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField34)");
            module.AddFormula(
                "Formula118",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 * Lookup05.LookupField33)");
            module.AddFormula(
                "Formula119",
                "Math.Ceil(OptionGroup02.Option13 / Lookup02.LookupField05)");
            module.AddFormula(
                "Formula120",
                "5 * Formula119");
            module.AddFormula(
                "Formula121",
                "0.01");
            module.AddFormula(
                "Formula122",
                "Formula185 * Lookup03.LookupField14");
            module.AddFormula(
                "Formula123",
                "Formula124 + Formula125");
            module.AddFormula(
                "Formula124",
                "Formula141 * Formula142 * Formula143 * Formula144");
            module.AddFormula(
                "Formula125",
                "((OptionGroup01.Option01 / Lookup02.LookupField05) / Formula145) * (Formula146 * Formula143)");
            module.AddFormula(
                "Formula126",
                "if(OptionGroup02.Option10 ==  \"Value211\", then = 0, else = Formula127 + Formula128 + Formula129)");
            module.AddFormula(
                "Formula127",
                "(Formula148 * Formula149 * Formula150 * Formula144)");
            module.AddFormula(
                "Formula128",
                "Formula185 / (Formula151) * (Formula153 * Formula150)");
            module.AddFormula(
                "Formula129",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula130 + Formula131, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula130",
                "(Formula148 * Formula149 * Formula150 * Formula144)");
            module.AddFormula(
                "Formula131",
                "Formula115 / (Formula152) * (Formula153 * Formula150)");
            module.AddFormula(
                "Formula132",
                "Formula133 + Formula147");
            module.AddFormula(
                "Formula133",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField26)");
            module.AddFormula(
                "Formula134",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula135 + Formula136, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula135 + Formula136, else = 0)");
            module.AddFormula(
                "Formula135",
                "Formula047");
            module.AddFormula(
                "Formula136",
                "Formula049 * OptionGroup01.Option01 * OptionGroup02.Option13");
            module.AddFormula(
                "Formula137",
                "Formula182 * Formula099 * Formula154");
            module.AddFormula(
                "Formula138",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula139 + Formula140, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula139",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField07 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula140",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 / Lookup02.LookupField06 * Formula061, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula141",
                "0.05");
            module.AddFormula(
                "Formula142",
                "1");
            module.AddFormula(
                "Formula143",
                "13");
            module.AddFormula(
                "Formula144",
                "1.26");
            module.AddFormula(
                "Formula145",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = 2700, case = OptionGroup02.Option14 ==  \"Value158\", do = 1500, else = 0)");
            module.AddFormula(
                "Formula146",
                "1.01");
            module.AddFormula(
                "Formula147",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 / Lookup05.LookupField32 * Lookup05.LookupField30)");
            module.AddFormula(
                "Formula148",
                "0.05");
            module.AddFormula(
                "Formula149",
                "1");
            module.AddFormula(
                "Formula150",
                "13");
            module.AddFormula(
                "Formula151",
                "4056");
            module.AddFormula(
                "Formula152",
                "550");
            module.AddFormula(
                "Formula153",
                "1");
            module.AddFormula(
                "Formula154",
                "10");
            module.AddFormula(
                "Formula155",
                "Formula156 + Formula157");
            module.AddFormula(
                "Formula156",
                "(Formula170 * Formula171)");
            module.AddFormula(
                "Formula157",
                "Formula185 / (Formula172) * (Formula173 * Formula171)");
            module.AddFormula(
                "Formula158",
                "if(OptionGroup02.Option10 ==  \"Value211\", then = 0, else = Formula159 + Formula160 + Formula161)");
            module.AddFormula(
                "Formula159",
                "Formula174 * Formula175");
            module.AddFormula(
                "Formula160",
                "Formula185 / (Formula176) * Formula175");
            module.AddFormula(
                "Formula161",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = Formula162 + Formula163, case = OptionGroup02.Option10 !=  \"Value154\", do = 0, else = 0)");
            module.AddFormula(
                "Formula162",
                "Formula177 * Formula091");
            module.AddFormula(
                "Formula163",
                "Formula115 / Formula066 * Formula064 * Formula091");
            module.AddFormula(
                "Formula164",
                "Formula165 + Formula166");
            module.AddFormula(
                "Formula165",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = Lookup05.LookupField25)");
            module.AddFormula(
                "Formula166",
                "if(OptionGroup02.Option05 ==  \"Value186\", then = 0, else = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 / Lookup05.LookupField32 * Lookup05.LookupField28)");
            module.AddFormula(
                "Formula167",
                "where(case = OptionGroup02.Option10 ==  \"Value174\", do = Formula168 + Formula169, case = OptionGroup02.Option10 ==  \"Value165\", do = Formula168 + Formula169, else = 0)");
            module.AddFormula(
                "Formula168",
                "Formula082");
            module.AddFormula(
                "Formula169",
                "Formula084 * OptionGroup01.Option01 * OptionGroup02.Option13");
            module.AddFormula(
                "Formula170",
                "0.05");
            module.AddFormula(
                "Formula171",
                "0.427");
            module.AddFormula(
                "Formula172",
                "where(case = OptionGroup02.Option14 ==  \"Value203\", do = 2700, case = OptionGroup02.Option14 ==  \"Value158\", do = 1500, else = 0)");
            module.AddFormula(
                "Formula173",
                "1.01");
            module.AddFormula(
                "Formula174",
                "0.05");
            module.AddFormula(
                "Formula175",
                "15.721");
            module.AddFormula(
                "Formula176",
                "4056");
            module.AddFormula(
                "Formula177",
                "0.05");
            module.AddFormula(
                "Formula178",
                "(Formula103 + Formula104) * 0.291");
            module.AddFormula(
                "Formula179",
                "(Formula102 + Formula103 + Formula104) * 0.164069");
            module.AddFormula(
                "Formula180",
                "Formula029 * Formula030");
            module.AddFormula(
                "Formula181",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = (((Lookup02.LookupField03 * Lookup02.LookupField02) / 1000000)) * ((Lookup04.LookupField17 / 1000) * (OptionGroup01.Option01 * OptionGroup02.Option13 * OptionGroup02.Option07)) / Formula098, case = OptionGroup02.Option10 !=  \"Value154\", do = (((Lookup02.LookupField03 * Lookup02.LookupField02) / 1000000)) * ((Lookup04.LookupField17 / 1000) * (OptionGroup01.Option01 * OptionGroup02.Option13)) / Formula098)");
            module.AddFormula(
                "Formula182",
                "if(OptionGroup02.Option02 !=  \"Value186\" and OptionGroup01.Option01 > Lookup08.LookupField45, then = Math.Ceil(Formula181) + 1, else = Math.Ceil(Formula181))");
            module.AddFormula(
                "Formula183",
                "0.38");
            module.AddFormula(
                "Formula184",
                "if(Formula182 >= 17, then = Formula182 / 28, else = 0)");
            module.AddFormula(
                "Formula185",
                "where(case = OptionGroup02.Option10 ==  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05 * OptionGroup02.Option07, case = OptionGroup02.Option10 !=  \"Value154\", do = OptionGroup01.Option01 * OptionGroup02.Option13 / Lookup02.LookupField05, else = 0)");
            module.AddFormula(
                "Formula186",
                "Formula187 + Formula188");
            module.AddFormula(
                "Formula187",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula189 * Formula190, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddFormula(
                "Formula188",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula191 * Formula190 * Formula180, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddFormula(
                "Formula189",
                "0.08");
            module.AddFormula(
                "Formula190",
                "13");
            module.AddFormula(
                "Formula191",
                "0.000232");
            module.AddFormula(
                "Formula192",
                "Formula193 + Formula196");
            module.AddFormula(
                "Formula193",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula194 * Formula195, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddFormula(
                "Formula194",
                "0.08");
            module.AddFormula(
                "Formula195",
                "2.249");
            module.AddFormula(
                "Formula196",
                "where(case = OptionGroup02.Option11 ==  \"Value213\", do = Formula197 * Formula195 * Formula180, case = OptionGroup02.Option11 ==  \"Value185\", do = 0, else = 0)");
            module.AddFormula(
                "Formula197",
                "0.000232");
            module.AddFormula(
                "Formula198",
                "where(case = Formula002 < Formula003, do = Formula185 / 4, case = Formula002 > Formula003, do = Formula027, else = 0)");
            module.AddFormula(
                "Formula199",
                "where(case = Formula002 < Formula003, do = Formula182, case = Formula002 > Formula003, do = Formula097, else = 0)");
            module.AddFormula(
                "Formula200",
                "where(case = Formula002 < Formula003, do = Formula184, case = Formula002 > Formula003, do = Formula100, else = 0)");
            module.AddFormula(
                "Formula201",
                "where(case = Formula002 < Formula003, do = Formula002, case = Formula002 > Formula003, do = Formula003, else = 0)");
            module.AddFormula(
                "Formula202",
                "where(case = OptionGroup02.Option10 ==  \"Value154\" and (OptionGroup01.Option01 * OptionGroup02.Option07) < 250, do =  \"String02\", case = OptionGroup02.Option10 ==  \"Value154\" and (OptionGroup01.Option01 * OptionGroup02.Option07) >= 250, do =  \"String03\", case = OptionGroup01.Option01 < 250 and OptionGroup02.Option15 !=  \"Value080\" and OptionGroup02.Option15 !=  \"Value079\" and OptionGroup02.Option10 !=  \"Value173\" and OptionGroup02.Option10 !=  \"Value178\" and Lookup04.LookupField17 != 450, do =  \"String02\", case = OptionGroup02.Option10 ==  \"Value173\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value178\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value141\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value180\", do =  \"String03\", case = OptionGroup02.Option10 ==  \"Value137\" and OptionGroup02.Option08 ==  \"Value051\" and OptionGroup01.Option01 <= 500, do =  \"String02\", case = Formula002 < Formula003, do =  \"String02\", case = Formula002 > Formula003, do =  \"String03\", else =  \"Value186\")");

            try
            {
                _executable = program.Compile(CompilationProfile);
            }
            catch (CimbolCompilationException e)
            {
                Console.WriteLine("{0} {1} {2}", e.Formula, e.Start, e.End);
                throw;
            }

            _arguments = new ILocalValue[]
            {
                new StringValue("0"), 
                new StringValue("Value186"),
                new StringValue("Value103"),
                new StringValue("Value194"),
                new StringValue("Value186"),
                new StringValue("0"),
                new StringValue("0"),
                new StringValue("Value008"),
                new StringValue("0"),
                new StringValue("Value180"),
                new StringValue("Value213"),
                new StringValue("0"),
                new StringValue("0"),
                new StringValue("Value203"),
                new StringValue("Value005"),
                new StringValue("Value186"),
            };

            var testResult = _executable.Call(_arguments).Result;
            if (testResult.Errors.Count > 0)
            {
                var firstError = testResult.Errors.First();
                Console.WriteLine("{0} {1} {2}", firstError.Module, firstError.Formula, firstError.Message);
                throw new Exception(firstError.Message);
            }
        }

        [Benchmark]
        public async Task<EvaluationResult> Benchmark_FormulaAsync()
        {
            var result = await _executable.Call(_arguments);

            if (result.Errors.Count > 0)
            {
                var firstError = result.Errors.First();
                Console.WriteLine("{0} {1} {2}", firstError.Module, firstError.Formula, firstError.Message);
                throw new Exception(firstError.Message);
            }

            return result;
        }

        public CompilationProfile[] CompilationProfiles()
        {
            return new[] { CompilationProfile.Minimal, CompilationProfile.Trace, CompilationProfile.Verbose };
        }

        private PromiseValue Lookup01Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField01", new BooleanValue(false) },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup02Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField02", new NumberValue(210) },
                { "LookupField03", new NumberValue(97.66m) },
                { "LookupField04", new NumberValue(24) },
                { "LookupField05", new NumberValue(6) },
                { "LookupField06", new NumberValue(0) },
                { "LookupField07", new NumberValue(0) },
                { "LookupField08", new BooleanValue(true) },
                { "LookupField09", new StringValue("0.0205086") },
                { "LookupField10", new StringValue("0") },
                { "LookupField11", new StringValue("0") },
                { "LookupField12", new StringValue("0") },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup03Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField13", new NumberValue(0.00328m) },
                { "LookupField14", new NumberValue(4) },
                { "LookupField15", new BooleanValue(true) },
                { "LookupField16", new StringValue("0") },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup04Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField17", new NumberValue(100) },
                { "LookupField18", new NumberValue(0.03686m) },
                { "LookupField19", new NumberValue(0.01888m) },
                { "LookupField20", new BooleanValue(true) },
                { "LookupField21", new StringValue("0") },
                { "LookupField22", new StringValue("0") },
                { "LookupField23", new StringValue("0") },
                { "LookupField24", new StringValue("0") },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup05Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField25", new NumberValue(1) },
                { "LookupField26", new NumberValue(1) },
                { "LookupField27", new NumberValue(1) },
                { "LookupField28", new NumberValue(1) },
                { "LookupField29", new NumberValue(1) },
                { "LookupField30", new NumberValue(1) },
                { "LookupField31", new NumberValue(1) },
                { "LookupField32", new NumberValue(1) },
                { "LookupField33", new NumberValue(1) },
                { "LookupField34", new NumberValue(1) },
                { "LookupField35", new BooleanValue(true) },
                { "LookupField36", new StringValue("0") },
                { "LookupField37", new StringValue("1") },
                { "LookupField38", new StringValue("1") },
                { "LookupField39", new StringValue("0") },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup06Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField40", new BooleanValue(false) },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup07Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField41", new NumberValue(150) },
                { "LookupField42", new NumberValue(0.065m) },
                { "LookupField43", new NumberValue(90) },
                { "LookupField44", new BooleanValue(true) },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup08Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField45", new NumberValue(0) },
                { "LookupField46", new NumberValue(0) },
                { "LookupField47", new BooleanValue(true) },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private PromiseValue Lookup09Function()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "LookupField48", new BooleanValue(false) },
            };

            return new PromiseValue(Task.FromResult((ILocalValue)new ObjectValue(objectValueContents)));
        }

        private ObjectValue MakeMathModule()
        {
            var objectValueContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Ceil", new FunctionValue((Func<NumberValue, NumberValue>)CeilFunction) },
                { "Min", new FunctionValue((Func<NumberValue, NumberValue, NumberValue>)MinFunction) },
            };

            return new ObjectValue(objectValueContents);
        }

        private NumberValue CeilFunction(NumberValue value)
        {
            return new NumberValue(Math.Ceiling(value.Value));
        }

        private NumberValue MinFunction(NumberValue leftValue, NumberValue rightValue)
        {
            return new NumberValue(leftValue.Value + rightValue.Value);
        }
    }
}