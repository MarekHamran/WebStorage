using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebStorage
{
	/// <summary>
	/// Allows to select controller method e.g.
	/// One method may accept json, another xml
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ProducesAttribute" />
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint" />
	public class AcceptHeaderAttribute : ProducesAttribute, IActionConstraint
	{
		/// <summary>
		/// Gets or sets the value header
		/// </summary>
		/// <value>
		/// The value of header
		/// </value>
		public string Value { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AcceptHeaderAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public AcceptHeaderAttribute(string value) : base(value)
		{
			Value = value;
		}

		/// <summary>
		/// Determines whether an action is a valid candidate for selection.
		/// </summary>
		/// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.ActionConstraints.ActionConstraintContext" />.</param>
		/// <returns>
		/// True if the action is valid for selection, otherwise false.
		/// </returns>
		public bool Accept(ActionConstraintContext context)
		{
			if (context.RouteContext.HttpContext.Request.Headers.TryGetValue("Accept", out var value))
			{
				return value[0] == Value;
			}

			return false;
		}

		/// <summary>
		/// Gets the order.
		/// </summary>
		/// <value>
		/// The order.
		/// </value>
		public new int Order => 0;
	}
}