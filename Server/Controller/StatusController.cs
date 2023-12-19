using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private Dictionary<int, string> _appointmentStatuses;

    public StatusController()
    {
        _appointmentStatuses = new Dictionary<int, string>
        {
            { 1, "Scheduled" },
            { 2, "Confirmed" },
            { 3, "Cancelled" }
        };
    }

    [HttpGet]
    public ActionResult<IEnumerable<KeyValuePair<int, string>>> Get()
    {
        return Ok(_appointmentStatuses);
    }

    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
        if (_appointmentStatuses.ContainsKey(id))
        {
            return Ok(_appointmentStatuses[id]);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public ActionResult Post([FromBody] KeyValuePair<int, string> newStatus)
    {
        _appointmentStatuses.Add(newStatus.Key, newStatus.Value);

        return CreatedAtAction(nameof(Get), new { id = newStatus.Key }, newStatus);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] string updatedStatus)
    {
        if (_appointmentStatuses.ContainsKey(id))
        {
            _appointmentStatuses[id] = updatedStatus;

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (_appointmentStatuses.ContainsKey(id))
        {
            _appointmentStatuses.Remove(id);

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
