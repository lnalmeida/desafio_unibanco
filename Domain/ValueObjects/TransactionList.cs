using System.Text.Json.Nodes;
using DesafioUnibanco.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DesafioUnibanco.Domain.ValueObjects;

public class TransactionList
{
    private readonly Transaction _item;
    private readonly List<Transaction> _transactionList; 
    
    public TransactionList()
    {
        _transactionList = new List<Transaction>();
    }
    
    public void Add(Transaction transaction)
    {
        _transactionList.Add(transaction);
    }
    
    public void ClearTransactionsList()
    {
        _transactionList.Clear();
    }
    
    public object GetEstatistics(int seconds)
    {
        var lastTransactions = _transactionList.Where(t => 
            t.GetCreatedAt() > DateTime.Now.AddSeconds(-seconds));
        
        if (!lastTransactions.Any())
        {
            return null;
        }
        
        var count = lastTransactions.Count();
        var sum = lastTransactions.Sum(t => t.GetValor());
        var average = lastTransactions.Average(t => t.GetValor());
        var max = lastTransactions.Max(t => t.GetValor());
        var min = lastTransactions.Min(t => t.GetValor());
        
        var statistics = new 
        {
            Count = count,
            Sum = sum,
            Average = average,
            Min = min,
            Max = max
        };
        
        return new JsonResult(statistics).Value;
    }
    
    
}