using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebStorage
{
	public class AcceptHeaderAttribute : ProducesAttribute, IActionConstraint
	{
		public string Value { get; set; }

		public AcceptHeaderAttribute(string value) : base(value)
		{
			Value = value;
		}

		public bool Accept(ActionConstraintContext context)
		{
			if (context.RouteContext.HttpContext.Request.Headers.TryGetValue("Accept", out var value))
			{
				return value[0] == Value;
			}

			return false;
		}

		public new int Order => 0;
	}
}