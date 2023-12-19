using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RequestsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        return new string[] { "Request1", "Request2" };
    }

    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
        return "Request with ID: " + id;
    }

    [HttpPost]
    public ActionResult Post([FromBody] string value)
    {
        Console.WriteLine("Received POST request with value: " + value);

        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] string value)
    {
        Console.WriteLine($"Received PUT request for ID {id} with value: {value}");

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Console.WriteLine($"Received DELETE request for ID {id}");

        return Ok();
    }
}
