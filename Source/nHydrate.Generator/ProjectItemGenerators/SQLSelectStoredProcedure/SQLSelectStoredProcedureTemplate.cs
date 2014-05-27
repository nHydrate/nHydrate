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

namespace Widgetsphere.Generator.ProjectItemGenerators.DatabaseSchema
{
  class SQLSelectStoredProcedureTemplate : BaseDbScriptTemplate
  {
    private StringBuilder sb = new StringBuilder();
    private CustomStoredProcedure _currentStoredProcedure;

    #region Constructors
    public SQLSelectStoredProcedureTemplate(ModelRoot model, CustomStoredProcedure currentStoredProcedure)
    {
      _model = model;
      _currentStoredProcedure = currentStoredProcedure;
    }
    #endregion

    #region BaseClassTemplate overrides
    public override string FileContent
    {
      get
      {
        GenerateContent();
        return sb.ToString();
      }
    }

    public override string FileName
    {
      get
      {
        return string.Format("{0}Select.sql", _currentStoredProcedure.PascalName);
      }
    }
    #endregion

    #region GenerateContent
    private void GenerateContent()
		{
			if (_model.Database.AllowZeroTouch) return;
      try
      {
        this.AppendFullTemplate();
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
      foreach (Reference reference in _currentStoredProcedure.GeneratedColumns)
      {
        Column dc = (Column)reference.Object;
        ii++;
        output.Append(dc.DatabaseName.ToLower());
        if (ii != _currentStoredProcedure.GeneratedColumns.Count)
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
      foreach (Reference reference in _currentStoredProcedure.Parameters)
      {
        Parameter parameter = (Parameter)reference.Object;
        ii++;
				output.Append("\t@" + ValidationHelper.MakeDatabaseScriptIdentifier(parameter.DatabaseName) + " " + 
					parameter.DataType.ToString().ToLower() + 
					(parameter.GetPredefinedSize() == -1 ? "(" + parameter.Length + ") " : "") + (parameter.IsOutputParameter ? " out " : " = default"));

        if (ii != _currentStoredProcedure.Parameters.Count)
          output.Append(",");
        output.AppendLine();
      }
      return output.ToString();
    }

    public string StoredProcedureName
    {
      get { return "gen_" + Globals.GetPascalName(_model, _currentStoredProcedure) + "Select"; }
    }

    #endregion

    public void AppendFullTemplate()
    {
      try
      {
				sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
				sb.AppendLine("--Data Schema For Version " + _model.Version);
				ValidationHelper.AppendCopyrightInSQL(sb, _model);
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

        if(_currentStoredProcedure.Parameters.Count > 0)
        {
          sb.AppendLine("(" );
          sb.Append(this.BuildParameterList());
          sb.AppendLine(")" );
        }

        sb.AppendLine("AS" );
        sb.AppendLine("SET NOCOUNT ON;" );
        sb.AppendLine();
        sb.Append(_currentStoredProcedure.SQL);
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
          sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", this.StoredProcedureName, _model.Database.GrantExecUser).AppendLine();
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
