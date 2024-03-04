using Microsoft.AspNetCore.Mvc.RazorPages;
using WWWPGrids;
using WWWPGrids.Charts;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide
{
    public class ChartSample : PageModel
    {
        private readonly ILogger<ChartSample> _logger;
        public ChartSample(ILogger<ChartSample> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            SAPGridView oSGV = AddGrid();
            oSGV = AddGridChart(oSGV);
            TempData["SAPGridView"] = oSGV.GridBind("MyGrid1");
        }

        protected SAPGridView AddGrid()
        {
            List<ChartSampleModel> dt = new ChartSampleData().GetData();
            SAPGridView oSGV = new();

            oSGV.Grids["MyGrid1"] = new Grid()
            {
                ContainerId = "MyGridId",
                ContainerHeight = 400,
                Data = dt,
                GridTitle = "Test Report 1",
                Options = new Option() { DropDownFilterButton = true, TitleRowInExelExport = false },
                Columns = new List<Column>() {
                    new() { Data = "Id", Title = "Id" },
                    new() { Data = "Province", Title = "Province" },
                    new() { Data = "Population2005", Title = "Population2005" },
                    new() { Data = "Population2013", Title = "Population2013" },
                    new() { Data = "Population2015", Title = "Population2015" },
                    new() { Data = "Population2013ComparedTo2005", Title = "Population2013ComparedTo2005" },
                    new()
                    {
                        Data = "Population2015ComparedTo2005", Title = "Population2015ComparedTo2005",
                        Functions =
                        {
                            new Calc
                            {
                                Section = Function.SectionValue.Tfoot,
                                Operator = Calc.OperatorValue.VerticalSum
                            },
                            new Calc
                            {
                                Section = Function.SectionValue.Tfoot,
                                Formula = "Population2015ComparedTo2005 / " + dt.Count.ToString()
                            },
                            new Separator
                            {
                                Section = Function.SectionValue.Tfoot,
                                DecimalPlaces = 2
                            }
                        }
                    }
                }
            };
            return oSGV;
        }

        public SAPGridView AddGridChart(SAPGridView oSGV)
        {
            oSGV.Grids["MyGrid1"].Charts.Add(GetPieChart());
            oSGV.Grids["MyGrid1"].Charts.Add(GetColumnChart(oSGV.Grids["MyGrid1"]));
            return oSGV;
        }

        public PieChart GetPieChart()
        {
            return new PieChart()
            {
                ChartContainerId = "pieChartContainer",
                Key = "Province",
                Value = "Population2013ComparedTo2005",
                Title = new ChartTitle
                {
                    Text = "PieChart Title 1",
                    Align = TitleAlign.Center
                }
            };
        }

        public ColumnChart GetColumnChart(Grid grid)
        {
            return new ColumnChart()
            {
                ChartContainerId = "columnChartContainer",

                Title = new ChartTitle
                {
                    Text = "ColumnChart Title 1",
                    Align = TitleAlign.Center
                },
                XAxis = new ColumnChartXAxis()
                {
                    Title = new ChartTitle()
                    {
                        Text = "XAxis Title 1",
                        Align = TitleAlign.Center
                    },
                    Accessibility = new Accessibility()
                    {
                        Description = "Accessibility description"
                    },
                    Categories = "Province"
                },
                YAxis = new ColumnChartYAxis()
                {
                    Title = new ChartTitle
                    {
                        Text = "YAxis Title 1",
                        Align = TitleAlign.Center
                    },
                    Min = 0
                },
                Series = new List<string>() { "Population2005", "Population2013", "Population2015" }
            };
        }
    }

    public class ChartSampleData
    {
        public List<ChartSampleModel> GetData()
        {
            var d = new List<ChartSampleModel>
            {
                new() { Id = 1, Province = "Tehran", Population2005 = 11228625, Population2013 = 12183391, Population2015 = 13267637 },
                new() { Id = 2, Province = "KhorasanRazavi", Population2005 = 5515980, Population2013 = 5994402, Population2015 = 6434501 },
                new() { Id = 3, Province = "Isfahan", Population2005 = 4399327, Population2013 = 4879312, Population2015 = 5120850 },
                new() { Id = 4, Province = "Fars", Population2005 = 4220721, Population2013 = 4596658, Population2015 = 4851274 },
                new() { Id = 5, Province = "Khuzestan", Population2005 = 4192598, Population2013 = 4531720, Population2015 = 4710509 },
                new() { Id = 6, Province = "AzerbaijanSharghi", Population2005 = 3527267, Population2013 = 3724620, Population2015 = 3909652 },
                new() { Id = 7, Province = "Mazandaran", Population2005 = 2893087, Population2013 = 3073943, Population2015 = 3283582 },
                new() { Id = 8, Province = "AzerbaijanGharbi", Population2005 = 2831779, Population2013 = 3080576, Population2015 = 3265219 },
                new() { Id = 9, Province = "Kerman", Population2005 = 2584834, Population2013 = 2938988, Population2015 = 3164718 },
                new() { Id = 10, Province = "SistanVaBaluchistan", Population2005 = 2349049, Population2013 = 2534327, Population2015 = 2775014 },
                new() { Id = 11, Province = "Alborz", Population2005 = 2053233, Population2013 = 2412513, Population2015 = 2712400 },
                new() { Id = 12, Province = "Gilan", Population2005 = 2381063, Population2013 = 2480874, Population2015 = 2530696 },
                new() { Id = 13, Province = "Kermanshah", Population2005 = 1842457, Population2013 = 1945227, Population2015 = 1952434 },
                new() { Id = 14, Province = "Golestan", Population2005 = 1593055, Population2013 = 1777014, Population2015 = 1868619 },
                new() { Id = 15, Province = "Hormozgan", Population2005 = 1365377, Population2013 = 1578183, Population2015 = 1776415 },
                new() { Id = 16, Province = "Lorestan", Population2005 = 1689650, Population2013 = 1754243, Population2015 = 1760649 },
                new() { Id = 17, Province = "Hamadan", Population2005 = 1674595, Population2013 = 1738214, Population2015 = 1758268 },
                new() { Id = 18, Province = "Kurdistan", Population2005 = 1416334, Population2013 = 1493645, Population2015 = 1603011 },
                new() { Id = 19, Province = "Markazi", Population2005 = 1326826, Population2013 = 1413959, Population2015 = 1429475 },
                new() { Id = 20, Province = "Qom", Population2005 = 1036714, Population2013 = 1151672, Population2015 = 1292283 },
                new() { Id = 21, Province = "Qazvin", Population2005 = 1127734, Population2013 = 1201565, Population2015 = 1273761 },
                new() { Id = 22, Province = "Ardabil", Population2005 = 1209968, Population2013 = 1248488, Population2015 = 1270420 },
                new() { Id = 23, Province = "Bushehr", Population2005 = 866490, Population2013 = 1032949, Population2015 = 1163400 },
                new() { Id = 24, Province = "Yazd", Population2005 = 958323, Population2013 = 1074428, Population2015 = 1138533 },
                new() { Id = 25, Province = "Zanjan", Population2005 = 942818, Population2013 = 1015734, Population2015 = 1057461 },
                new() { Id = 26, Province = "ChaharmahalVaBakhtiari", Population2005 = 843784, Population2013 = 895263, Population2015 = 947763 },
                new() { Id = 27, Province = "KhorasanShomali", Population2005 = 791930, Population2013 = 867727, Population2015 = 863092 },
                new() { Id = 28, Province = "KhorasanJonobi", Population2005 = 600568, Population2013 = 662534, Population2015 = 768898 },
                new() { Id = 29, Province = "KohgiluyehVaBoyerAhmad", Population2005 = 621428, Population2013 = 658629, Population2015 = 713052 },
                new() { Id = 30, Province = "Semnan", Population2005 = 570835, Population2013 = 631218, Population2015 = 702360 },
                new() { Id = 31, Province = "Ilam", Population2005 = 560464, Population2013 = 557599, Population2015 = 580158 }
            };
            foreach (var item in d)
            {
                double population2013ComparedTo2005 = ((item.Population2013 - item.Population2005) / item.Population2005) * 100;
                double population2015ComparedTo20055 = ((item.Population2015 - item.Population2005) / item.Population2005) * 100;
                item.Population2013ComparedTo2005 = double.Parse(population2013ComparedTo2005.ToString("N2"));
                item.Population2015ComparedTo2005 = double.Parse(population2015ComparedTo20055.ToString("N2"));
            }
            return d;
        }
    }
    public class ChartSampleModel
    {
        public int Id { get; set; }
        public string Province { get; set; }
        public double Population2005 { get; set; }
        public double Population2013 { get; set; }
        public double Population2015 { get; set; }
        public double Population2013ComparedTo2005 { get; set; }
        public double Population2015ComparedTo2005 { get; set; }
    }
}