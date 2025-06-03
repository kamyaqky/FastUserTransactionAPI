using Dapper;
using FastUserTransactionAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FastUserTransactionAPI.Services
{
    public class DataService
    {
        private readonly IDbConnection _db;

        public DataService(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<List<Models.TransactionDTO>> GetUserTransactions(string username)
        {
            string query = @"
            WITH TopAccounts AS (
                SELECT TOP 100 accountid
                FROM Accounts
                ORDER BY score DESC
            )
            SELECT TOP 10 t.transactionid, t.starttime, t.endtime
            FROM Users u
            JOIN TopAccounts ta ON u.accountid = ta.accountid
            JOIN Transactions t ON t.accountid = ta.accountid
            WHERE u.username = @username
            ORDER BY t.endtime DESC";

            return (await _db.QueryAsync<TransactionDTO>(query, new { username })).ToList();
        }

        public async Task<List<Models.TransactionDTO>> GetGroupTransactionsIfAdmin(string username)
        {
            string query = @"
            WITH TopAccounts AS (
                SELECT TOP 100 accountid, groupid
                FROM Accounts
                ORDER BY score DESC
            )
            SELECT TOP 20 t.transactionid, t.starttime, t.endtime
            FROM Users u
            JOIN TopAccounts ta ON u.accountid = ta.accountid
            JOIN Accounts a ON a.accountid = ta.accountid
            JOIN Transactions t ON t.groupid = ta.groupid AND t.accountid IN (
                SELECT accountid FROM TopAccounts WHERE groupid = ta.groupid
            )
            WHERE u.username = @username AND a.role = 'ADMIN'
            ORDER BY t.endtime DESC";

            return (await _db.QueryAsync<TransactionDTO>(query, new { username })).ToList();
        }
    }
}

