using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockTest.Server.Data;
using MockTest.Server.IRepository;
using MockTest.Shared.Domain;

namespace MockTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var Tasks = await _unitOfWork.Tasks.GetAll();
            return Ok(Tasks);
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskItem(int id)
        {
            var Tasks=await _unitOfWork.Tasks.Get(q=>q.Id == id);

            if(Tasks == null)
            {
                return NotFound();
            }

            return Ok(Tasks);
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Tasks.Update(taskItem);

            try
            {
                await _unitOfWork.Save(HttpContext);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            await _unitOfWork.Tasks.Insert(taskItem);
            await _unitOfWork.Save(HttpContext);

            return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _unitOfWork.Tasks.Get(q=>q.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            await _unitOfWork.Tasks.Delete(id);
            await _unitOfWork.Save(HttpContext);

            return NoContent();
        }

        private async Task<bool> TaskItemExists(int id)
        {
            var taskitem=await _unitOfWork.Tasks.Get(q=>q.Id==id);
            return taskitem != null;
        }
    }
}
