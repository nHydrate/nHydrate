using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
	partial class Module
	{
		protected override void OnDeleting()
		{
			//Remove from relation mapped collection
			var count1 = this.nHydrateModel.RelationModules.Remove(x => x.ModuleId == this.Id);
			var count2 = this.nHydrateModel.IndexModules.Remove(x => x.ModuleId == this.Id);

			base.OnDeleting();
		}

	}
}

