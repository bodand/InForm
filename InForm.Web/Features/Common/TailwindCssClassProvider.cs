using Microsoft.AspNetCore.Components.Forms;

namespace InForm.Web.Features.Common
{
	public class TailwindCssClassProvider : FieldCssClassProvider
	{
		private const string ValidBorderColor = "border-slate-400";
		private const string InvalidBorderColor = "border-rose-500";

		public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
		{
			if (editContext.IsValid(fieldIdentifier)) return ValidBorderColor;
			return InvalidBorderColor;
		}
	}
}
