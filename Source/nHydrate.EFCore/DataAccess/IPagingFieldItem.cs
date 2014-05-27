using System;
using System.Collections.Generic;
using System.Text;

namespace Widgetsphere.EFCore.DataAccess
{
	public interface IPagingFieldItem
	{
		bool Ascending { get; set; }
		Enum GetField();
	}
}
