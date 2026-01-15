using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.CoreService.Application.Features.TodoItems.Create;
using TaskFlow.CoreService.Application.Features.TodoItems.Get;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdateDescription;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdatePriority;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdateStatus;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdateTitle;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

public sealed class TodoModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/todos");
        
        group.MapPost("/", CreateTodo)
            .WithName("CreateTodo")
            .WithSummary("Creates a new todo.")
            .Produces<CreateTodoResponse>(StatusCodes.Status201Created)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest);
        
        group.MapPatch("{todoId:guid}/title", UpdateTitle).WithName(nameof(UpdateTitle));
        group.MapPatch("{todoId:guid}/description", UpdateDescription).WithName(nameof(UpdateDescription));
        group.MapPatch("{todoId:guid}/priority", UpdatePriority).WithName(nameof(UpdatePriority));
        group.MapPatch("{todoId:guid}/status", UpdateStatus).WithName(nameof(UpdateStatus));

        group.MapGet("project/{projectId:guid}", GetTodosByProjectId);
    }
    
    /// <summary>
    /// Creates a new todo within a specific project.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/todos
    ///     {
    ///        "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "authorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "title": "Fix production bug",
    ///        "description": "High priority fix for the login issue",
    ///        "priority": 1,
    ///        "dueDate": "2026-12-31T23:59:59Z",
    ///        "estimatedCompletionTime": "02:00:00"
    ///     }
    /// </remarks>
    /// <param name="request">The todo creation details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="201">Returns the newly created todo IDs.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> CreateTodo(
        CreateTodoRequest request,
        ISender sender)
    {
        var command = new CreateTodoCommand(
            request.ProjectId,
            request.AuthorId,
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.EstimatedCompletionTime);
        
        var result = await sender.Send(command);

        return result.Match(
            response => Results.Created($"api/todos/{response.CreatedTodoId}", response),
            Results.BadRequest);
    }

    /// <summary>
    /// Updates the title in an existing todo.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/title
    ///     {
    ///        "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "newTitle": "Fix production bug"
    ///     }
    /// </remarks>
    /// <param name="todoId">Todo identifier that need changes the title.</param>
    /// <param name="request">The todo changing title details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo title successfully changed.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private async Task<IResult> UpdateTitle(Guid todoId, UpdateTodoTitleRequest request, ISender sender)
    {
        var command = new UpdateTodoTitleCommand(todoId, request.ProjectId, request.NewTitle);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            Results.BadRequest);
    }
    
    /// <summary>
    /// Updates the description in an existing todo.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/description
    ///     {
    ///        "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "newDescription": "High priority fix for the login issue"
    ///     }
    /// </remarks>
    /// <param name="todoId">Todo identifier that need changes the description.</param>
    /// <param name="request">The todo changing description details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo description successfully changed.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private async Task<IResult> UpdateDescription(Guid todoId, UpdateTodoDescriptionRequest request, ISender sender)
    {
        var command = new UpdateTodoDescriptionCommand(todoId, request.ProjectId, request.NewDescription);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            Results.BadRequest);
    }
    
    /// <summary>
    /// Updates the priority in an existing todo.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/priority
    ///     {
    ///        "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "newPriority": 4
    ///     }
    /// </remarks>
    /// <param name="todoId">Todo identifier that need changes the priority.</param>
    /// <param name="request">The todo changing priority details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo priority successfully changed.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private async Task<IResult> UpdatePriority(Guid todoId, UpdateTodoPriorityRequest request, ISender sender)
    {
        var command = new UpdateTodoPriorityCommand(todoId, request.ProjectId, request.NewPriority);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            Results.BadRequest);
    }
    
    /// <summary>
    /// Updates the status in an existing todo.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/status
    ///     {
    ///        "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "newStatus": 4
    ///     }
    /// </remarks>
    /// <param name="todoId">Todo identifier that need changes the status.</param>
    /// <param name="request">The todo changing status details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo status successfully changed.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private async Task<IResult> UpdateStatus(Guid todoId, UpdateTodoStatusRequest request, ISender sender)
    {
        var command = new UpdateTodoStatusCommand(todoId, request.ProjectId, request.NewStatus);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            Results.BadRequest);
    }
    
    
    private static async Task<IResult> GetTodosByProjectId(Guid projectId, ISender sender)
    {
        var query = new GetTodoByProjectIdQuery(projectId);
        
        var result = await sender.Send(query);
        
        return result.Match(
            _ => Results.Ok(result),
            Results.BadRequest);
    }
}