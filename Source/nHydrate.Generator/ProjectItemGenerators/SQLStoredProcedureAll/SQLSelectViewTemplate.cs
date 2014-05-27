#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Text;

using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLSelectViewTemplate : ISQLGenerate
  {
		private ModelRoot _model;
    private CustomView _currentView;

    #region Constructors
    public SQLSelectViewTemplate(ModelRoot model, CustomView currentView)
    {
      _model = model;
      _currentView = currentView;
    }
    #endregion

    #region GenerateContent
    public void GenerateContent(StringBuilder sb)
    {
			if (_model.Database.AllowZeroTouch) return;
      try
      {        
        this.AppendFullTemplate(sb);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    #endregion

    #region string methods

    protected string BuildSelectList()
    {
      StringBuilder output = new StringBuilder();
      int ii = 0;
      foreach (Reference reference in _currentView.GeneratedColumns)
      {
        Column dc = (Column)reference.Object;
        ii++;
        output.Append(dc.DatabaseName.ToLower());
        if (ii != _currentView.GeneratedColumns.Count)
        {
          output.Append("," + Environment.NewLine + "\t");
        }
      }
      return output.ToString();
    }

    protected string BuildParameterList()
    {
      StringBuilder output = new StringBuilder();
      int ii = 0;
      foreach (Reference reference in _currentView.Parameters)
      {
        Parameter parameter = (Parameter)reference.Object;
        ii++;
        output.Append("\t@" + ValidationHelper.MakeDatabaseScriptIdentifier(parameter.DatabaseName) + " " + 
					parameter.DataType.ToString().ToLower());

        if (ModelHelper.VariableLengthType(parameter.DataType))
          output.Append("(" + parameter.Length + ") ");
        output.Append((parameter.IsOutputParameter ? " out " : " = default"));

        if (ii != _currentView.Parameters.Count)
          output.Append(",");
        output.AppendLine();
      }
      return output.ToString();
    }

    public string StoredProcedureName
    {
      get { return "gen_" + Globals.GetPascalName(_model, _currentView) + "Select"; }
    }

    #endregion

    private void AppendFullTemplate(StringBuilder sb)
    {
      try
      {
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + this.StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        sb.AppendLine("	drop procedure [dbo].[" + this.StoredProcedureName + "]" );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("SET QUOTED_IDENTIFIER ON " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("CREATE PROCEDURE [dbo].[" + this.StoredProcedureName + "]" );

        if(_currentView.Parameters.Count > 0)
        {
          sb.AppendLine("(" );
          sb.Append(this.BuildParameterList());
          sb.AppendLine(")" );
        }

        sb.AppendLine("AS" );
        sb.AppendLine("SET NOCOUNT ON;" );
        sb.AppendLine();

        sb.AppendLine("SELECT " );

        int index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.Append("CONVERT(" + column.DatabaseType + ", [view_" + _currentView.DatabaseName + "].[" + column.DatabaseName + "]) AS [" + column.DatabaseName + "]");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.AppendLine();
          index++;
        }
        sb.AppendLine("FROM" );
        sb.AppendLine("[view_" + _currentView.DatabaseName + "]" );

        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("SET QUOTED_IDENTIFIER OFF " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        if (_model.Database.GrantExecUser != string.Empty)
        {
          sb.AppendFormat("GRANT EXECUTE ON [dbo].[{0}] TO [{1}]", this.StoredProcedureName, _model.Database.GrantExecUser).AppendLine();
          sb.AppendLine("GO" );
          sb.AppendLine();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

  }
}
