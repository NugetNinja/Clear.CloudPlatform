﻿using Clear.CloudPlatform.Application.Common.Exceptions;
using Clear.CloudPlatform.Application.Common.Interfaces;
using Clear.CloudPlatform.Domain.Entities;
using MediatR;

namespace Clear.CloudPlatform.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommand : IRequest
{
    public int Id { get; set; }

    public string? Title { get; set; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoList), request.Id);
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
