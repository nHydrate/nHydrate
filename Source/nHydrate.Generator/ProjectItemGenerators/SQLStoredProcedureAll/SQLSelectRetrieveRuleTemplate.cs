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
using System.Collections;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLSelectRetrieveRuleTemplate : ISQLGenerate
	{
		private ModelRoot _model = null;
    private CustomRetrieveRule _currentRule;
    private Table ParentTable = null;

		#region Constructors
    public SQLSelectRetrieveRuleTemplate(ModelRoot model, CustomRetrieveRule currentRule)
    {
      _model = model;
      _currentRule = currentRule;
      this.ParentTable = (Table)currentRule.ParentTableRef.Object;
		}
		#endregion 

		#region GenerateContent
		public void GenerateContent(StringBuilder sb)
		{
			if (_model.Database.AllowZeroTouch) return;
      try
      {
        if (_currentRule.UseSearchObject)
        {
					this.AppendANDTemplate(sb);
					this.AppendORTemplate(sb);
        }
        else
        {
          this.AppendNormalTemplate(sb);
        }        
      }
      catch(Exception ex)
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
      foreach(Reference reference in this.ParentTable.GeneratedColumns)
      {
        Column dc = (Column)reference.Object;
        ii++;
        output.Append(dc.DatabaseName.ToLower());
        if(ii != this.ParentTable.GeneratedColumns.Count)
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
      foreach(Reference reference in _currentRule.Parameters)
      {
        Parameter parameter = (Parameter)reference.Object;
        ii++;
				output.Append("\t@" + ValidationHelper.MakeDatabaseScriptIdentifier(parameter.DatabaseName) + " " + 
					parameter.DataType.ToString().ToLower());

				if (ModelHelper.VariableLengthType(parameter.DataType))
          output.Append("(" + parameter.Length + ") ");
        output.Append((parameter.IsOutputParameter ? " out " : " = default"));

        if(ii != _currentRule.Parameters.Count)
          output.Append(",");
        output.AppendLine();
      }
      return output.ToString();
    }

    public ArrayList GetValidSearchColumns()
    {
      try
      {
        ArrayList validColumns = new ArrayList();
        foreach (Reference reference in this.ParentTable.GeneratedColumns)
        {
          Column dc = (Column)reference.Object;
          if (!(dc.DataType == System.Data.SqlDbType.Binary ||
            dc.DataType == System.Data.SqlDbType.Image ||
            dc.DataType == System.Data.SqlDbType.NText ||
            dc.DataType == System.Data.SqlDbType.Text ||
            dc.DataType == System.Data.SqlDbType.Timestamp ||
            dc.DataType == System.Data.SqlDbType.Udt ||
            dc.DataType == System.Data.SqlDbType.VarBinary ||
            dc.DataType == System.Data.SqlDbType.Variant ||
          dc.DataType == System.Data.SqlDbType.Money))
          {
            validColumns.Add(dc);
          }
        }
        return validColumns;

      }
      catch (Exception ex)
      {
        throw new Exception(this.ParentTable.DatabaseName + ": Failed on generation of select or template", ex);
      }
    }

    #endregion

    #region AppendNormalTemplate

    private void AppendNormalTemplate(StringBuilder sb)
		{
      try
      {
        string SPName = string.Format("gen_{0}CustomSelectBy{1}", this.ParentTable.PascalName, _currentRule.PascalName);

				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + SPName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        sb.AppendLine("	drop procedure [dbo].[" + SPName + "]" );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("SET QUOTED_IDENTIFIER ON " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("CREATE PROCEDURE [dbo].[" + SPName + "]" );

        //Add the parameters
        if (_currentRule.Parameters.Count > 0)
        {
          sb.AppendLine("(" );
          sb.AppendLine(this.BuildParameterList() );
          sb.AppendLine(")" );
          sb.AppendLine();
        }
        sb.AppendLine("AS" );

        sb.AppendLine("SET NOCOUNT ON;" );
        sb.AppendLine();

        string sql = _currentRule.SQL.Replace("%1%", Globals.BuildSelectList(this.ParentTable, _model));
        sql = sql.Replace("%COLUMNS%", Globals.BuildSelectList(this.ParentTable, _model));
        sql = sql.Replace("%TABLE%", this.ParentTable.DatabaseName);
        sb.AppendLine(sql );
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
					sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", SPName, _model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO" );
					sb.AppendLine();
				}


      }
      catch(Exception ex)
      {
        throw;
      }

    }

    #endregion

    #region AppendANDTemplate

    private void AppendANDTemplate(StringBuilder sb)
    {
      try
      {
        string SPName = string.Format("gen_{0}CustomSelectBy{1}And", Globals.GetPascalName(_model, this.ParentTable), _currentRule.PascalName);

        ArrayList validColumns = GetValidSearchColumns();
        sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + SPName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" );
        sb.AppendLine("	drop procedure [dbo].[" + SPName + "]" );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("SET QUOTED_IDENTIFIER ON " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("CREATE PROCEDURE [dbo].[" + SPName + "]" );
        sb.AppendLine("(" );

        //The specified parameters
        foreach (Reference reference in _currentRule.Parameters)
        {
          Parameter parameter = (Parameter)reference.Object;
					sb.AppendFormat("	@{0} " + parameter.DataType.ToString() + (ModelHelper.VariableLengthType(parameter.DataType) ? "(" + parameter.Length + ")" : "") + " = null, " + System.Environment.NewLine, parameter.DatabaseName);
        }

        //The search object parameters
        foreach (Column dc in validColumns)
        {
					sb.AppendFormat("	@{0} varchar(100) = null, " + System.Environment.NewLine, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
        }
        sb.AppendLine("	@max_row_count int" );
        sb.AppendLine(")" );
        sb.AppendLine("AS" );
        sb.AppendLine();
        sb.AppendLine("IF (@max_row_count > 0)" );
        sb.AppendLine("BEGIN" );
        sb.AppendLine("	SET ROWCOUNT @max_row_count" );
        sb.AppendLine("END" );
        sb.AppendLine();

        //Build the where clause
        StringBuilder tempSB = new StringBuilder();
        int index = 1;
        foreach (Column dc in validColumns)
        {
					tempSB.AppendFormat("(@{1} is null or [{0}].[{1}] LIKE @{1})", this.ParentTable.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
          if (index < validColumns.Count)
          {
            tempSB.AppendLine().Append("and");
          }
          tempSB.AppendLine();
          index++;
        }

        string sql = _currentRule.SQL;
        sql = sql.Replace("%COLUMNS%", Globals.BuildSelectList(this.ParentTable, _model));
        sql = sql.Replace("%TABLE%", this.ParentTable.DatabaseName);
        sql = sql.Replace("%SEARCHWHERE%", tempSB.ToString());
        sb.Append(sql);

        sb.AppendLine();
        sb.AppendLine("GO" );
        sb.AppendLine("SET QUOTED_IDENTIFIER OFF " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        if (_model.Database.GrantExecUser != string.Empty)
        {
          sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", SPName, _model.Database.GrantExecUser).AppendLine();
          sb.AppendLine("GO" );
          sb.AppendLine();
        }

      }
      catch (Exception ex)
      {
        throw;
      }

    }

    #endregion

    #region AppendORTemplate

		private void AppendORTemplate(StringBuilder sb)
    {
      try
      {
        string SPName = string.Format("gen_{0}CustomSelectBy{1}Or", Globals.GetPascalName(_model, this.ParentTable), _currentRule.PascalName);

        ArrayList validColumns = GetValidSearchColumns();
        sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + SPName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" );
        sb.AppendLine("	drop procedure [dbo].[" + SPName + "]" );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("SET QUOTED_IDENTIFIER ON " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        sb.AppendLine("CREATE PROCEDURE [dbo].[" + SPName + "]" );
        sb.AppendLine("(" );
        //The specified parameters
        foreach (Reference reference in _currentRule.Parameters)
        {
          Parameter parameter = (Parameter)reference.Object;
          sb.AppendFormat("	@{0} " + parameter.DataType.ToString() + (ModelHelper.VariableLengthType(parameter.DataType) ? "(" + parameter.Length + ")" : "") + " = null, " + System.Environment.NewLine, parameter.DatabaseName);
        }

        //The search object parameters
        foreach (Column dc in validColumns)
        {
					sb.AppendFormat("	@{0} varchar(100) = null, " + System.Environment.NewLine, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
        }
        sb.AppendLine("	@max_row_count int" );
        sb.AppendLine(")" );
        sb.AppendLine("AS" );
        sb.AppendLine();
        sb.AppendLine("IF (@max_row_count > 0)" );
        sb.AppendLine("BEGIN" );
        sb.AppendLine("	SET ROWCOUNT @max_row_count" );
        sb.AppendLine("END" );
        sb.AppendLine();

        //Build the where clause
        StringBuilder tempSB = new StringBuilder();
        int index = 1;
        foreach (Column dc in validColumns)
        {
					tempSB.AppendFormat("	([{0}].[{1}] LIKE @{1})", this.ParentTable.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
          if (index < validColumns.Count)
          {
            tempSB.AppendLine().Append("or");
          }
          tempSB.AppendLine();
          index++;
        }

        string sql = _currentRule.SQL;
        sql = sql.Replace("%COLUMNS%", Globals.BuildSelectList(this.ParentTable, _model));
        sql = sql.Replace("%TABLE%", this.ParentTable.DatabaseName);
        sql = sql.Replace("%SEARCHWHERE%", tempSB.ToString());
        sb.Append(sql);
        
        sb.AppendLine();
        sb.AppendLine("GO" );
        sb.AppendLine("SET QUOTED_IDENTIFIER OFF " );
        sb.AppendLine("GO" );
        sb.AppendLine("SET ANSI_NULLS ON " );
        sb.AppendLine("GO" );
        sb.AppendLine();
        if (_model.Database.GrantExecUser != string.Empty)
        {
          sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", SPName, _model.Database.GrantExecUser).AppendLine();
          sb.AppendLine("GO" );
          sb.AppendLine();
        }

      }
      catch (Exception ex)
      {
        throw;
      }

    }

    #endregion

  }
}
