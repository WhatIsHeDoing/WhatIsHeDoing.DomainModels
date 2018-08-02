using Microsoft.AspNetCore.Mvc;

public class DefaultController : Controller
{
    [Route("")]
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public RedirectResult RedirectToSwaggerUi() => Redirect("/swagger/");
}
