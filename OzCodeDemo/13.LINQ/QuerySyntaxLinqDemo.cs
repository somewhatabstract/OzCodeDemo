using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using OzCodeDemo.DemoClasses.Customers;

namespace OzCodeDemo._13.LINQ
{
	[Export(typeof(IOzCodeDemo))]
	[ExportMetadata("Demo", "QuerySyntaxLinq")]
	public class QuerySyntaxLinqDemo : IOzCodeDemo
	{
		public void Start()
		{
			Debugger.Break();

			var customers = CustomersRepository.LoadCustomersFromDb();

			var customersEligible = customers.Where(c => c.Address.State != null).ToList();

			var salesReps = new[]
			{
				new {Name = "Bob", States = new [] {"PA", "AK", "WI", "IN", "NM", "NJ" }, Address = "Bob's House"},
				new {Name = "Danny", States = new [] {"MI", "HI", "NY", "TX", "AL","MD"}, Address = "Bob's House"},
				new {Name = "Albert", States = new [] {"KS", "UT", "DE", "LA" }, Address = "Not Bob's House"},
				new {Name = "Samantha", States = new [] { "MN", "OH", "IL", "CA", "ND", "SD", "VA", "SC"}, Address = "Bob's House"},
			};

			var customersWithSalesRep1 =
				from rep in salesReps
				from state in rep.States
				join customer in customersEligible on state equals customer.Address.State
				select new { Customer = customer, SalesRep = rep };

			Debug.Assert( customersWithSalesRep1.Count() == customersEligible.Count, "Every customer should have a sales rep" );


			var customersWithSalesRep2 =
				from rep in salesReps
				from state in rep.States
				join customer in customersEligible on state equals customer.Address.State into customersWithRep
				from customerWithRep in customersWithRep.DefaultIfEmpty()
				where customerWithRep != null
				select new {Customer = customerWithRep, SalesRep = rep};

			Debug.Assert( customersWithSalesRep2.Count() == customersEligible.Count, "Every customer should have a sales rep");
		}
	}
}