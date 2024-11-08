using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers;

[ApiController]
[Route("/[controller]")]
public class HelloWorldController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        return AnswerSuccess(new
        {
            AppName = ConfigApp.Get("app.name"),
            Version = ConfigApp.Get("app.version"),
            Message = "Hello World!",
        });
    }
}