using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspDotNetCoreRazor.Pages;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;


    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public int Level1 { get; set; } = 10;

    public int RandomNumber { get; set; }

    public string RandomWinner { get; set; }

    public void OnGet()
    {
        Random random = new Random();
        RandomNumber = random.Next(1000);
        RandomWinner = $"Player {random.Next(1, 4)}";
    }

    public IActionResult OnPostRandomNumber1([FromBody] RandomNumberDTO randomNumber)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid model");
        }
        Random random = new Random();
        int number = random.Next(randomNumber.Min, randomNumber.Max);
        return new JsonResult(number);
    }

}

public class RandomNumberDTO
{
    public int Min { get; set; }
    public int Max { get; set; }
}