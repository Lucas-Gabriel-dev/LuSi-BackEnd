using LuSiBack.src.Context;
using LuSiBack.src.models;
using LuSiBack.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuSiBack.src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LusiController : ControllerBase
    {
        private readonly LusiContext _context;

        public LusiController(LusiContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> LookTask(int id)
        {
            try
            {
                var tasks = await _context.TaskUsers.FindAsync(id);

                if(tasks == null)
                {
                    return BadRequest(new 
                    {
                        msg = "This task doesn't exist!"
                    });
                }

                var options = _context.TaskOptions
                                      .OrderByDescending(x => x.Name)
                                      .Where(x => x.CurrentTaskId == tasks.Id)
                                      .ToList();

                List<AllOptions> allOptions = new List<AllOptions>();

                int taskFinished = 0;

                for (var i = 0; i < options.Count; i++)
                {
                    AllOptions option = new AllOptions(options[i].Id, options[i].Name, options[i].Complete);

                    if(options[i].Complete)
                    {
                        taskFinished += 1;
                    }

                    allOptions.Add(option);
                }

                int progressOfTask = 0;
                
                if(options.Count > 0){
                    progressOfTask = (taskFinished * 100) / options.Count;
                }

                return Ok(new 
                {
                    id = tasks.Id,
                    title = tasks.Title,
                    description = tasks.Description,
                    deadLine = tasks.DeadLine.ToString("dd/MM/yyyy"),
                    options = allOptions,
                    progress = progressOfTask
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(error);
            }
        }

        [HttpGet("AllTask")]
        [Authorize]
        public async Task<IActionResult> LookAllTasksUser()
        {
            try
            {
                int userId = Int32.Parse(User.Identity.Name);

                var verifyUser = _context.Users.Find(userId);
                
                if(verifyUser == null)
                {
                    return BadRequest(new
                    {
                        msg = "User not found"
                    });
                }

                var tasks = _context.TaskUsers
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Where(x => x.UserTaskId == userId)
                                    .ToList();

                if(tasks == null)
                {
                    return BadRequest(new
                    {
                        msg = "This doesn't have tasks"
                    });
                }

                List<AllTasks> allTasks = new List<AllTasks>();

                foreach (var item in tasks)
                {
                    AllTasks task = new AllTasks(item.Id, item.Title);

                    allTasks.Add(task);
                }

                // return Ok(new {
                //     user = verifyUser.Name,
                //     tasks = allTasks
                // });

                return Ok(allTasks);
            }
            catch (System.Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("AddTask")]
        [Authorize]
        public async Task<IActionResult> AddTask(TaskUser option)
        {
            try
            {
                if(
                    string.IsNullOrWhiteSpace(option.Title) || 
                    string.IsNullOrWhiteSpace(option.Description)  
                )
                {
                    return BadRequest(new 
                    {
                        msg = "Fill in all fields"
                    });
                }

                if(option.DeadLine < DateTime.Now)
                {
                    return BadRequest(new
                    {
                        msg = "The deadline date is invalid"
                    });
                }

                option.UserTaskId = Int32.Parse(User.Identity.Name);

                var user = await _context.Users.FindAsync(option.UserTaskId);

                if(user == null)
                {
                    return BadRequest(new 
                    {
                        msg = "User not found!"
                    });
                }

                List<string> taskOptions = new List<string>();

                
                foreach (var item in option.TaskOptions)
                {
                    if(!string.IsNullOrWhiteSpace(item.Name))
                    {
                        taskOptions.Add(item.Name);
                    }
                }
                option.Id = 0;
                option.UpdatedAt = null;
                option.User = null;
                option.TaskOptions = null;
                option.CreatedAt = DateTime.Now;

                await _context.TaskUsers.AddAsync(option);
                await _context.SaveChangesAsync();

                await AddTaskOption(option.Id, taskOptions);

                return Ok(new 
                {
                    idTaks = option.Id
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("AddTaskOption")]
        [Authorize]
        public async Task<IActionResult> AddTaskOption(int idTask, List<string> option)
        {
            try
            {
                if(idTask == 0)
                {
                    return BadRequest(new
                    {
                        msg = "Task not found"
                    });
                }

                var verifyTask = await _context.TaskUsers.FindAsync(idTask);

                var UserTaskId = Int32.Parse(User.Identity.Name);
                var verifyUser = await _context.TaskUsers.FindAsync(UserTaskId);

                if(verifyTask == null || verifyTask == null)
                {
                    return BadRequest(new
                    {
                        msg = "Task or User incorrect"
                    });
                }

                foreach (var item in option)
                {
                    if(!string.IsNullOrWhiteSpace(item))
                    {
                        TaskOption taskOption = new TaskOption();
                        taskOption.CurrentTaskId = idTask;
                        taskOption.Complete = false;
                        taskOption.Name = item;
                        
                        _context.TaskOptions.Add(taskOption);
                        _context.SaveChanges();
                    }
                }

                return await LookTask(idTask);
            }
            catch (System.Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("UserNotifications")]
        [Authorize]
        public async Task<IActionResult> UserNotifications(){
            try
            {
                var userId = Int32.Parse(User.Identity.Name);

                var searchTasks = _context.TaskUsers
                                          .OrderBy(x => x.DeadLine)
                                          .Where(x => x.UserTaskId == userId)
                                          .ToList();


                List<Notification> listNotification = new List<Notification>();

                foreach (var item in searchTasks)
                {
                    Notification userNotification = new Notification();

                    var diffDays = item.DeadLine - DateTime.Now;

                    if(diffDays.Days <= 3){
                        userNotification.Description = $"Faltam {diffDays.ToString("dd")}" +
                        $" dias para encerrar o prazo da tarefa {item.Title}";
                        
                        listNotification.Add(userNotification);
                    }
                }

                var verifyUser = _context.Users.Find(userId);
                
                if(verifyUser == null)
                {
                    return BadRequest(new
                    {
                        msg = "User not found"
                    });
                }


                return Ok(new  {
                    verifyUser.Name,
                    listNotification
                });
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpPatch("EditTask")]
        [Authorize]
        public async Task<IActionResult> EditTask(TaskUser taskUser)
        {
            try
            {
                if(taskUser == null)
                {
                    return BadRequest(new 
                    {
                        msg = "Task is null"
                    });
                }

                var findTask = await _context.TaskUsers.FindAsync(taskUser.Id);

                if(findTask.UserTaskId != Int32.Parse(User.Identity.Name))
                {
                    return BadRequest(new
                    {
                        msg = "User not logged!"
                    });
                }

                Console.WriteLine($"{findTask}");
                
                if(!string.IsNullOrWhiteSpace(taskUser.Title)) 
                {
                    findTask.Title = taskUser.Title;
                }

                if(!string.IsNullOrWhiteSpace(taskUser.Description))
                {
                    findTask.Description = taskUser.Description;             
                } 
                    
                findTask.UpdatedAt = DateTime.Now;

                _context.TaskUsers.Update(findTask);
                _context.SaveChanges();

                return Ok(new 
                {
                    title = findTask.Title,
                    description = findTask.Description,
                    UpdatedAt = findTask.UpdatedAt
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(error);
            }
        } 

        [HttpPatch("EditOptionsTask")]
        [Authorize]
        public async Task<IActionResult> EditOptionsTask(List<TaskOption> options)
        {
            try
            {
                if(options[0] == null)
                {
                    return BadRequest(new 
                    {
                        msg = "Options is null"
                    });
                }

                var task = await _context.TaskUsers.FindAsync(options[0].CurrentTaskId);

                if(task == null)
                {
                    return BadRequest(new {
                        msg = "The task doesn't exist!"
                    });
                }

                foreach (var item in options)
                {
                    var option = await _context.TaskOptions.FindAsync(item.Id);

                    if(option == null) 
                    {
                        return BadRequest(new{
                            msg = $"Id {item.Id} option doesn't exist"
                        });
                    } 
                }

                foreach (var item in options)
                {
                    var option = await _context.TaskOptions.FindAsync(item.Id);
                    option.Name = item.Name;
                    option.Complete = item.Complete;

                    _context.TaskOptions.Update(option);
                    _context.SaveChanges();
                }

                task.UpdatedAt = DateTime.Now;

                _context.TaskUsers.Update(task);
                _context.SaveChanges();

                return await LookTask(task.Id);
            }
            catch (System.Exception error)
            {
                return BadRequest(error);
            }
        }

        [HttpDelete("DeleteTask/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.TaskUsers.FindAsync(id);

                if(task == null)
                {
                    return BadRequest(new 
                    {
                        msg = "Task doesn't exist"
                    });
                }

                _context.TaskUsers.Remove(task);
                _context.SaveChanges();

                var taskOption = _context.TaskOptions.Where(x => x.CurrentTaskId == id);
                
                foreach (var item in taskOption)
                {
                    _context.TaskOptions.Remove(item);
                    _context.SaveChanges();
                }          

                return Ok(new 
                {
                    msg = $"Task ID: {id} deleted"
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(error);
            }   
        }
    }
}