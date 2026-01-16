using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.CoreService.Application.Features.TodoItems.Create;
using TaskFlow.CoreService.Application.Features.TodoItems.Delete;
using TaskFlow.CoreService.Application.Features.TodoItems.Get;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdateDescription;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdatePriority;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdateStatus;
using TaskFlow.CoreService.Application.Features.TodoItems.UpdateTitle;
using TaskFlow.CoreService.Presentation.Extensions;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

/// <summary>
/// Registers all todo-related API endpoints.
/// </summary>
/// <remarks>
/// <para>
/// Endpoints include:
/// </para>
/// <code>
/// - GET /api/todos/{todoId}
/// - GET /api/todos?projectId={projectId}
/// - POST /api/todos
/// - PATCH /api/todos/{todoId}/title
/// - PATCH /api/todos/{todoId}/description
/// - PATCH /api/todos/{todoId}/priority
/// - PATCH /api/todos/{todoId}/status
/// - DELETE /api/todos/{todoId}?projectId={projectId}
/// </code>
/// </remarks>
public sealed class TodoModule : ICarterModule
{
    /// <summary>
    /// Adds all routes for managing todos to the provided endpoint builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/todos/")
            .WithTags("Todos");
        
        group.MapGet("{todoId:guid}", GetTodoById)
            .WithName(nameof(GetTodoById))
            .Produces<GetTodoResponse>()
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapGet("", GetTodosByProjectId)            
            .Produces<List<GetTodoResponse>>()
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapPost("", CreateTodo)
            .Produces<CreateTodoResponse>(StatusCodes.Status201Created)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapPatch("{todoId:guid}/title", UpdateTitle)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapPatch("{todoId:guid}/description", UpdateDescription)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapPatch("{todoId:guid}/priority", UpdatePriority)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapPatch("{todoId:guid}/status", UpdateStatus)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
        
        group.MapDelete("{todoId:guid}", DeleteTodo)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<List<Error>>(StatusCodes.Status400BadRequest)
            .Produces<List<Error>>(StatusCodes.Status404NotFound);
    }
    
    /// <summary>
    /// Gets the existing todo by the todo identifier.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// GET /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1
    /// </code>
    /// </remarks>
    /// <param name="todoId">Todo identifier.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="200">Returns todo if it exists.</response>
    /// <response code="404">If the todo with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> GetTodoById(Guid todoId, ISender sender)
    {
        var query = new GetTodoByIdQuery(todoId);
        
        var result = await sender.Send(query);
        
        return result.Match(
            Results.Ok,
            _ => result.ToHttpResult()
            );
    }
    
    /// <summary>
    /// Gets the existing todos by project identifier.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// GET /api/todos?projectId=3fa85f64-5717-4562-b3fc-2c963f66afaf
    /// </code>
    /// </remarks>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="200">Returns all todos for the specified project. Returns an empty array if none exist.</response>
    /// <response code="404">If the project with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> GetTodosByProjectId(Guid projectId, ISender sender)
    {
        var query = new GetTodosByProjectIdQuery(projectId);
        
        var result = await sender.Send(query);
        
        return result.Match(
            Results.Ok,
            _ => result.ToHttpResult()
            );
    }
    
    /// <summary>
    /// Creates a new todo within a specific project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// POST /api/todos
    /// {
    ///    "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///    "authorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///    "title": "Fix production bug",
    ///    "description": "High priority fix for the login issue",
    ///    "priority": 1,
    ///    "dueDate": "2026-12-31T23:59:59Z",
    ///    "estimatedCompletionTime": "02:00:00"
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">The todo creation details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="201">Returns the newly created todo IDs.</response>
    /// <response code="404">If the project or author with the specified identifier was not found.</response>
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
            response => Results.CreatedAtRoute(
                nameof(GetTodoById),
                new {todoId = response.CreatedTodoId},
                response),
            _ => result.ToHttpResult()
            );
    }

    /// <summary>
    /// Updates the title in an existing todo.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/title
    /// {
    ///    "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///    "newTitle": "Fix production bug"
    /// }
    /// </code>
    /// </remarks>
    /// <param name="todoId">Todo identifier that needs changes the title.</param>
    /// <param name="request">The todo changing title details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo title successfully changed.</response>
    /// <response code="404">If the todo or project with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> UpdateTitle(Guid todoId, UpdateTodoTitleRequest request, ISender sender)
    {
        var command = new UpdateTodoTitleCommand(todoId, request.ProjectId, request.NewTitle);
        
        var result = await sender.Send(command);
        
        return result.Match(
             Results.NoContent,
            _ => result.ToHttpResult()
            );
    }
    
    /// <summary>
    /// Updates the description in an existing todo.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/description
    /// {
    ///    "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///    "newDescription": "High priority fix for the login issue"
    /// }
    /// </code>
    /// </remarks>
    /// <param name="todoId">Todo identifier that needs changes the description.</param>
    /// <param name="request">The todo changing description details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo description successfully changed.</response>
    /// <response code="404">If the todo or project with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> UpdateDescription(Guid todoId, UpdateTodoDescriptionRequest request, ISender sender)
    {
        var command = new UpdateTodoDescriptionCommand(todoId, request.ProjectId, request.NewDescription);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            _ => result.ToHttpResult());
    }
    
    /// <summary>
    /// Updates the priority in an existing todo.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/priority
    /// {
    ///    "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///    "newPriority": 4
    /// }
    /// </code>
    /// </remarks>
    /// <param name="todoId">Todo identifier that needs changes the priority.</param>
    /// <param name="request">The todo changing priority details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo priority successfully changed.</response>
    /// <response code="404">If the todo or project with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> UpdatePriority(Guid todoId, UpdateTodoPriorityRequest request, ISender sender)
    {
        var command = new UpdateTodoPriorityCommand(todoId, request.ProjectId, request.NewPriority);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            _ => result.ToHttpResult());
    }
    
    /// <summary>
    /// Updates the status in an existing todo.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// PATCH /api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1/status
    /// {
    ///    "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///    "newStatus": 4
    /// }
    /// </code>
    /// </remarks>
    /// <param name="todoId">Todo identifier that needs changes the status.</param>
    /// <param name="request">The todo changing status details.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo status successfully changed.</response>
    /// <response code="404">If the todo or project with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> UpdateStatus(Guid todoId, UpdateTodoStatusRequest request, ISender sender)
    {
        var command = new UpdateTodoStatusCommand(todoId, request.ProjectId, request.NewStatus);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            _ => result.ToHttpResult());
    }
    
    /// <summary>
    /// Deletes existing todo.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sample request:
    /// </para>
    /// <code>
    /// DELETE api/todos/3b8c6b74-db2c-42b8-9aed-a8697434b8f1?projectId=3fa85f64-5717-4562-b3fc-2c963f66afaf
    /// </code>
    /// </remarks>
    /// <param name="todoId">Todo identifier that needs to delete.</param>
    /// <param name="projectId">The deleting todo project identifier.</param>
    /// <param name="sender">MediatR sender instance.</param>
    /// <response code="204">If the todo successfully deleted.</response>
    /// <response code="404">If the todo or project with the specified identifier was not found.</response>
    /// <response code="400">If the request data is invalid or business rules are violated.</response>
    private static async Task<IResult> DeleteTodo(Guid todoId, Guid projectId, ISender sender)
    {
        var command = new DeleteTodoCommand(todoId, projectId);
        
        var result = await sender.Send(command);
        
        return result.Match(
            Results.NoContent,
            _ => result.ToHttpResult());
    }
}