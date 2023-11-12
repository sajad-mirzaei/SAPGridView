namespace WWWPGrids.Models;

public class DatatablesViewModel<T>
{
    public List<DatatablesColumnModel> Columns { get; set; } = new();
    public IEnumerable<T> DataView { get; set; }
    public string ApiUrl { get; set; }
    public object[,] DefaultOrder { get; set; } = new object[,] { { 0, "asc" } };
}