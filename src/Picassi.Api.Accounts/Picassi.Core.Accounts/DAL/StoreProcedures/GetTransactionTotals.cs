namespace Picassi.Core.Accounts.DAL.StoreProcedures
{
    public class GetTransactionTotals : IStoredProcedure
    {       
        public string GetStoredProcedure()
        {


            return @"

IF EXISTS (
        SELECT type_desc, type
        FROM sys.procedures WITH(NOLOCK)
        WHERE NAME = 'GetTransactionTotals'
            AND type = 'P'
      )
     DROP PROCEDURE accounts.GetTransactionTotals
GO

CREATE PROCEDURE [accounts].[GetTransactionTotals]

@StartDate AS Date,
@EndDate AS Date

AS
BEGIN


select			a.id as AccountId, 
				c.id as CategoryId, 
				c.name as CategoryName, 
				ISNULL(agg.total, 0) as total,
				ISNULL(agg.amount, 0) as amount
from			accounts.Accounts a
inner join		accounts.Categories c
on				a.id = a.id
left join 
(
	
	select		t.FromId as AccountId, c.id as CategoryId, Count(*) as total, SUM(-amount) as amount	
	from		accounts.Categories c
	left join	accounts.Transactions t
	on			t.CategoryId = c.Id
	where		t.Date >= @StartDate
	and			t.Date <= @EndDate	
	and			t.FromId is not null
	group by	c.id, t.FromId

	union

	select		t.ToId as AccountId, c.id as CategoryId, Count(*) as total, SUM(amount) as amount
	from		accounts.Categories c
	left join	accounts.Transactions t
	on			t.CategoryId = c.Id
	where		t.Date >= @StartDate
	and			t.Date <= @EndDate	
	and			t.ToId is not null
	group by	c.id, t.ToId

)	as agg
ON		c.id = agg.CategoryId
AND		a.id = agg.AccountId


END

";
        }

    }
}
