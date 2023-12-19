using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private Dictionary<int, string> appointmentStatuses;

    public StatusController()
    {
        // Пример: Инициализация словаря статусов записей
        appointmentStatuses = new Dictionary<int, string>
        {
            { 1, "Scheduled" },
            { 2, "Confirmed" },
            { 3, "Cancelled" }
        };
    }

    // GET: api/Status
    [HttpGet]
    public ActionResult<IEnumerable<KeyValuePair<int, string>>> Get()
    {
        // Возвращаем список статусов записей
        return Ok(appointmentStatuses);
    }

    // GET: api/Status/5
    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
        if (appointmentStatuses.ContainsKey(id))
        {
            // Возвращаем статус с указанным Id
            return Ok(appointmentStatuses[id]);
        }
        else
        {
            return NotFound(); // В случае, если статус с указанным Id не найден
        }
    }

    // POST: api/Status
    [HttpPost]
    public ActionResult Post([FromBody] KeyValuePair<int, string> newStatus)
    {
        // Добавляем новый статус записи
        appointmentStatuses.Add(newStatus.Key, newStatus.Value);

        // Возвращаем созданный статус
        return CreatedAtAction(nameof(Get), new { id = newStatus.Key }, newStatus);
    }

    // PUT: api/Status/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] string updatedStatus)
    {
        if (appointmentStatuses.ContainsKey(id))
        {
            // Обновляем существующий статус
            appointmentStatuses[id] = updatedStatus;

            return Ok();
        }
        else
        {
            return NotFound(); // В случае, если статус с указанным Id не найден
        }
    }

    // DELETE: api/Status/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (appointmentStatuses.ContainsKey(id))
        {
            // Удаляем статус с указанным Id
            appointmentStatuses.Remove(id);

            return Ok();
        }
        else
        {
            return NotFound(); // В случае, если статус с указанным Id не найден
        }
    }
}
