using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Core.Models.Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível criar sua transação");
        }

    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível atualizar sua transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

        if (transaction is null)
            return new Response<Transaction?>(null, 404, "Transação não encontrada");

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();

        return new Response<Transaction?>(transaction, message: "Categoria excluida com sucesso");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível recuperar a transação");
        }

    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLatsDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível determinar a data de início ou término");
        }

        try
        {
            var query = context.Transactions
            .AsNoTracking().Where(x => x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate
                                  && x.UserId == request.UserId)
            .OrderBy(x => x.CreatedAt);


            var transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível recuperar as transações");
        }
        
    }
}
