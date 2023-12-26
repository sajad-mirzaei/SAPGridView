namespace WWWPGrids.Models;

public class DatatablesFiltersModel
{
    public int Draw { get; set; }
    public List<Dictionary<string, string>> Columns { get; set; }
    public List<Dictionary<string, string>> Order { get; set; }
    public int Start { get; set; } = 0;
    public int Length { get; set; } = 10;
    public Dictionary<string, string> Search { get; set; }
    public string CustomData { get; set; }
    public long? _ { get; set; }
}
public class DatatablesFiltersModel<T>
{
    public int Draw { get; set; }
    public List<Dictionary<string, string>> Columns { get; set; }
    public List<Dictionary<string, string>> Order { get; set; }
    public int Start { get; set; } = 0;
    public int Length { get; set; } = 10;
    public Dictionary<string, string> Search { get; set; }
    public T CustomData { get; set; }
    public long? _ { get; set; }
}
