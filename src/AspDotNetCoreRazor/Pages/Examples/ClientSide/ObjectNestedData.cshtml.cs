using Microsoft.AspNetCore.Mvc.RazorPages;
using WWWPGrids;

namespace AspDotNetCoreRazor.Pages.Examples.ClientSide
{
    public class ObjectNestedData : PageModel
    {
        private readonly ILogger<ObjectNestedData> _logger;
        public ObjectNestedData(ILogger<ObjectNestedData> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            SAPGridView oSGV = LoadGrid();
            TempData["SAPGridView"] = oSGV.GridBind("MyGrid1");
        }

        protected SAPGridView LoadGrid()
        {
            List<PersonsModel1> dt = MakeList();
            SAPGridView oSGV = new();

            oSGV.Grids["MyGrid1"] = new Grid()
            {
                ContainerId = "MyGridId",
                ContainerHeight = 400,
                Data = dt,
                Columns = new List<Column>() {
                    new Column { Data = "Id", Title = "id" },
                    new Column { Data = "FullName", Title = "Full Name" },
                    new Column { Data = "PersonsUniversity.Name", Title = "University Name" },
                }
            };
            return oSGV;
        }

        public List<PersonsModel1> MakeList()
        {
            List<PersonsModel1> oDT = new();

            for (int i = 0; i < 100; i++)
            {
                PersonsModel1 row = new();
                row.Id = i + 1000;
                row.FullName = "FullName " + i;
                row.PersonsUniversity = new PersonsUniversity()
                {
                    Id = i,
                    Name = "University" + i
                };
                oDT.Add(row);
            }
            return oDT;
        }
    }

    public class PersonsModel1
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public PersonsUniversity PersonsUniversity { get; set; }
    }
    public class PersonsUniversity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}