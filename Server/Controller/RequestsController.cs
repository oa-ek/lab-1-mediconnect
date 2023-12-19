using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class RequestsController : ControllerBase
{
    // GET: api/Requests
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        // Обработка GET-запроса
        return new string[] { "Request1", "Request2" };
    }

    // GET: api/Requests/5
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
        // Обработка GET-запроса с указанным id
        return "Request with ID: " + id;
    }

    // POST: api/Requests
    [HttpPost]
    public ActionResult Post([FromBody] string value)
    {
        // Обработка POST-запроса с переданным значением
        // Например, сохранение значения в базе данных
        Console.WriteLine("Received POST request with value: " + value);

        return Ok();
    }

    // PUT: api/Requests/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] string value)
    {
        // Обработка PUT-запроса с указанным id и переданным значением
        // Например, обновление значения в базе данных
        Console.WriteLine($"Received PUT request for ID {id} with value: {value}");

        return Ok();
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        // Обработка DELETE-запроса с указанным id
        // Например, удаление значения из базы данных
        Console.WriteLine($"Received DELETE request for ID {id}");

        return Ok();
    }
}
