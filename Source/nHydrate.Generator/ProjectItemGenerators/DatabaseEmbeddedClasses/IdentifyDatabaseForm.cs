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
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Acme.PROJECTNAME.Install
{
	/// <summary>
	/// Summary description for IdentifyDatabaseForm.
	/// </summary>
	public class IdentifyDatabaseForm : System.Windows.Forms.Form
	{
		private string mConnectionString = string.Empty;
		private string mDatabaseName = string.Empty;
		private bool _createdDb = false;


		#region designer generated code
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.ComboBox comboBoxConnectionDatabaseName;
		private System.Windows.Forms.TextBox textBoxConnectionPassword;
		private System.Windows.Forms.TextBox textBoxConnectionUserName;
		private System.Windows.Forms.RadioButton radioButtonConnectionUserPassword;
		private System.Windows.Forms.RadioButton radioButtonConnectionIntegratedSecurity;
		private System.Windows.Forms.Button buttonConnectionRefresh;
		private System.Windows.Forms.ComboBox comboBoxConnectionServerName;
		private System.Windows.Forms.Label labelConnectionPassword;
		private System.Windows.Forms.Label labelConnectionUserName;
		private System.Windows.Forms.Label labelConnectionSelectDatabase;
		private System.Windows.Forms.Label labelConnectionLogon;
		private System.Windows.Forms.Label labelConnectionServerName;
		private System.Windows.Forms.Label labelConnectionInstruction;
		private System.Windows.Forms.Button buttonConnectionTestConnection;
		private System.Windows.Forms.TabControl tabControlChooseDatabase;
		private System.Windows.Forms.TabPage tabPageConnection;
		private System.Windows.Forms.TabPage tabPageCreation;
		private System.Windows.Forms.TextBox textBoxCreationDatabaseName;
		private System.Windows.Forms.TextBox textBoxCreationPassword;
		private System.Windows.Forms.TextBox textBoxCreationUserName;
		private System.Windows.Forms.RadioButton radioButtonCreationUserPassword;
		private System.Windows.Forms.RadioButton radioButtonCreationIntegratedSecurity;
		private System.Windows.Forms.Button buttonCreationRefresh;
		private System.Windows.Forms.ComboBox comboBoxCreationServerName;
		private System.Windows.Forms.Label labelCreationPassword;
		private System.Windows.Forms.Label labelCreationUserName;
		private System.Windows.Forms.Label labelCreationDatabaseName;
		private System.Windows.Forms.Label labelCreationLogon;
		private System.Windows.Forms.Label labelCreationServerName;
		private System.Windows.Forms.Label labelCreationInstruction;
		private CheckBox _cbCreateDatabase;
		private CheckBox chkSaveSettings;
		private System.Windows.Forms.Button cmdCancel;

		private void InitializeComponent()
		{
			this.comboBoxConnectionDatabaseName = new System.Windows.Forms.ComboBox();
			this.textBoxConnectionPassword = new System.Windows.Forms.TextBox();
			this.textBoxConnectionUserName = new System.Windows.Forms.TextBox();
			this.radioButtonConnectionIntegratedSecurity = new System.Windows.Forms.RadioButton();
			this.buttonConnectionRefresh = new System.Windows.Forms.Button();
			this.comboBoxConnectionServerName = new System.Windows.Forms.ComboBox();
			this.labelConnectionPassword = new System.Windows.Forms.Label();
			this.labelConnectionUserName = new System.Windows.Forms.Label();
			this.labelConnectionSelectDatabase = new System.Windows.Forms.Label();
			this.labelConnectionLogon = new System.Windows.Forms.Label();
			this.labelConnectionServerName = new System.Windows.Forms.Label();
			this.labelConnectionInstruction = new System.Windows.Forms.Label();
			this.buttonConnectionTestConnection = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.tabControlChooseDatabase = new System.Windows.Forms.TabControl();
			this.tabPageConnection = new System.Windows.Forms.TabPage();
			this.radioButtonConnectionUserPassword = new System.Windows.Forms.RadioButton();
			this.tabPageCreation = new System.Windows.Forms.TabPage();
			this._cbCreateDatabase = new System.Windows.Forms.CheckBox();
			this.textBoxCreationDatabaseName = new System.Windows.Forms.TextBox();
			this.textBoxCreationPassword = new System.Windows.Forms.TextBox();
			this.textBoxCreationUserName = new System.Windows.Forms.TextBox();
			this.radioButtonCreationUserPassword = new System.Windows.Forms.RadioButton();
			this.radioButtonCreationIntegratedSecurity = new System.Windows.Forms.RadioButton();
			this.buttonCreationRefresh = new System.Windows.Forms.Button();
			this.comboBoxCreationServerName = new System.Windows.Forms.ComboBox();
			this.labelCreationPassword = new System.Windows.Forms.Label();
			this.labelCreationUserName = new System.Windows.Forms.Label();
			this.labelCreationDatabaseName = new System.Windows.Forms.Label();
			this.labelCreationLogon = new System.Windows.Forms.Label();
			this.labelCreationServerName = new System.Windows.Forms.Label();
			this.labelCreationInstruction = new System.Windows.Forms.Label();
			this.chkSaveSettings = new System.Windows.Forms.CheckBox();
			this.tabControlChooseDatabase.SuspendLayout();
			this.tabPageConnection.SuspendLayout();
			this.tabPageCreation.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBoxConnectionDatabaseName
			// 
			this.comboBoxConnectionDatabaseName.Location = new System.Drawing.Point(32, 240);
			this.comboBoxConnectionDatabaseName.Name = "comboBoxConnectionDatabaseName";
			this.comboBoxConnectionDatabaseName.Size = new System.Drawing.Size(221, 21);
			this.comboBoxConnectionDatabaseName.Sorted = true;
			this.comboBoxConnectionDatabaseName.TabIndex = 6;
			this.comboBoxConnectionDatabaseName.DropDown += new System.EventHandler(this.comboBoxConnectionDatabaseName_DropDown);
			// 
			// textBoxConnectionPassword
			// 
			this.textBoxConnectionPassword.Enabled = false;
			this.textBoxConnectionPassword.Location = new System.Drawing.Point(112, 184);
			this.textBoxConnectionPassword.Name = "textBoxConnectionPassword";
			this.textBoxConnectionPassword.PasswordChar = '*';
			this.textBoxConnectionPassword.Size = new System.Drawing.Size(185, 20);
			this.textBoxConnectionPassword.TabIndex = 5;
			// 
			// textBoxConnectionUserName
			// 
			this.textBoxConnectionUserName.Enabled = false;
			this.textBoxConnectionUserName.Location = new System.Drawing.Point(112, 160);
			this.textBoxConnectionUserName.Name = "textBoxConnectionUserName";
			this.textBoxConnectionUserName.Size = new System.Drawing.Size(185, 20);
			this.textBoxConnectionUserName.TabIndex = 4;
			// 
			// radioButtonConnectionIntegratedSecurity
			// 
			this.radioButtonConnectionIntegratedSecurity.Checked = true;
			this.radioButtonConnectionIntegratedSecurity.Location = new System.Drawing.Point(32, 112);
			this.radioButtonConnectionIntegratedSecurity.Name = "radioButtonConnectionIntegratedSecurity";
			this.radioButtonConnectionIntegratedSecurity.Size = new System.Drawing.Size(280, 16);
			this.radioButtonConnectionIntegratedSecurity.TabIndex = 2;
			this.radioButtonConnectionIntegratedSecurity.TabStop = true;
			this.radioButtonConnectionIntegratedSecurity.Text = "Use Windows Authentication";
			this.radioButtonConnectionIntegratedSecurity.CheckedChanged += new System.EventHandler(this.radioButtonConnectionIntegratedSecurity_CheckedChanged);
			// 
			// buttonConnectionRefresh
			// 
			this.buttonConnectionRefresh.Location = new System.Drawing.Point(256, 48);
			this.buttonConnectionRefresh.Name = "buttonConnectionRefresh";
			this.buttonConnectionRefresh.Size = new System.Drawing.Size(56, 23);
			this.buttonConnectionRefresh.TabIndex = 1;
			this.buttonConnectionRefresh.Text = "&Refresh";
			// 
			// comboBoxConnectionServerName
			// 
			this.comboBoxConnectionServerName.Location = new System.Drawing.Point(32, 48);
			this.comboBoxConnectionServerName.Name = "comboBoxConnectionServerName";
			this.comboBoxConnectionServerName.Size = new System.Drawing.Size(221, 21);
			this.comboBoxConnectionServerName.TabIndex = 0;
			this.comboBoxConnectionServerName.DropDown += new System.EventHandler(this.comboBoxConnectionServerName_DropDown);
			// 
			// labelConnectionPassword
			// 
			this.labelConnectionPassword.Location = new System.Drawing.Point(40, 184);
			this.labelConnectionPassword.Name = "labelConnectionPassword";
			this.labelConnectionPassword.Size = new System.Drawing.Size(64, 16);
			this.labelConnectionPassword.TabIndex = 9;
			this.labelConnectionPassword.Text = "Password:";
			// 
			// labelConnectionUserName
			// 
			this.labelConnectionUserName.Location = new System.Drawing.Point(40, 160);
			this.labelConnectionUserName.Name = "labelConnectionUserName";
			this.labelConnectionUserName.Size = new System.Drawing.Size(64, 16);
			this.labelConnectionUserName.TabIndex = 8;
			this.labelConnectionUserName.Text = "User Name: ";
			// 
			// labelConnectionSelectDatabase
			// 
			this.labelConnectionSelectDatabase.Location = new System.Drawing.Point(16, 224);
			this.labelConnectionSelectDatabase.Name = "labelConnectionSelectDatabase";
			this.labelConnectionSelectDatabase.Size = new System.Drawing.Size(300, 16);
			this.labelConnectionSelectDatabase.TabIndex = 7;
			this.labelConnectionSelectDatabase.Text = "3. Identify the database on the server";
			// 
			// labelConnectionLogon
			// 
			this.labelConnectionLogon.Location = new System.Drawing.Point(16, 88);
			this.labelConnectionLogon.Name = "labelConnectionLogon";
			this.labelConnectionLogon.Size = new System.Drawing.Size(300, 16);
			this.labelConnectionLogon.TabIndex = 6;
			this.labelConnectionLogon.Text = "2. Enter information to logon to server";
			// 
			// labelConnectionServerName
			// 
			this.labelConnectionServerName.Location = new System.Drawing.Point(16, 32);
			this.labelConnectionServerName.Name = "labelConnectionServerName";
			this.labelConnectionServerName.Size = new System.Drawing.Size(300, 16);
			this.labelConnectionServerName.TabIndex = 5;
			this.labelConnectionServerName.Text = "1. Select or enter a server name";
			// 
			// labelConnectionInstruction
			// 
			this.labelConnectionInstruction.Location = new System.Drawing.Point(8, 8);
			this.labelConnectionInstruction.Name = "labelConnectionInstruction";
			this.labelConnectionInstruction.Size = new System.Drawing.Size(312, 16);
			this.labelConnectionInstruction.TabIndex = 4;
			this.labelConnectionInstruction.Text = "Select the following to connect to SQL server data:";
			// 
			// buttonConnectionTestConnection
			// 
			this.buttonConnectionTestConnection.Location = new System.Drawing.Point(200, 272);
			this.buttonConnectionTestConnection.Name = "buttonConnectionTestConnection";
			this.buttonConnectionTestConnection.Size = new System.Drawing.Size(98, 23);
			this.buttonConnectionTestConnection.TabIndex = 7;
			this.buttonConnectionTestConnection.Text = "&Test Connection";
			this.buttonConnectionTestConnection.Click += new System.EventHandler(this.buttonConnectionTestConnection_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(180, 349);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(82, 24);
			this.cmdOK.TabIndex = 16;
			this.cmdOK.Text = "&OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(268, 349);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(80, 24);
			this.cmdCancel.TabIndex = 17;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// tabControlChooseDatabase
			// 
			this.tabControlChooseDatabase.Controls.Add(this.tabPageConnection);
			this.tabControlChooseDatabase.Controls.Add(this.tabPageCreation);
			this.tabControlChooseDatabase.Location = new System.Drawing.Point(8, 8);
			this.tabControlChooseDatabase.Name = "tabControlChooseDatabase";
			this.tabControlChooseDatabase.SelectedIndex = 0;
			this.tabControlChooseDatabase.Size = new System.Drawing.Size(344, 336);
			this.tabControlChooseDatabase.TabIndex = 17;
			// 
			// tabPageConnection
			// 
			this.tabPageConnection.Controls.Add(this.radioButtonConnectionUserPassword);
			this.tabPageConnection.Controls.Add(this.comboBoxConnectionServerName);
			this.tabPageConnection.Controls.Add(this.labelConnectionServerName);
			this.tabPageConnection.Controls.Add(this.labelConnectionUserName);
			this.tabPageConnection.Controls.Add(this.buttonConnectionTestConnection);
			this.tabPageConnection.Controls.Add(this.radioButtonConnectionIntegratedSecurity);
			this.tabPageConnection.Controls.Add(this.labelConnectionLogon);
			this.tabPageConnection.Controls.Add(this.labelConnectionInstruction);
			this.tabPageConnection.Controls.Add(this.comboBoxConnectionDatabaseName);
			this.tabPageConnection.Controls.Add(this.textBoxConnectionPassword);
			this.tabPageConnection.Controls.Add(this.buttonConnectionRefresh);
			this.tabPageConnection.Controls.Add(this.textBoxConnectionUserName);
			this.tabPageConnection.Controls.Add(this.labelConnectionPassword);
			this.tabPageConnection.Controls.Add(this.labelConnectionSelectDatabase);
			this.tabPageConnection.Location = new System.Drawing.Point(4, 22);
			this.tabPageConnection.Name = "tabPageConnection";
			this.tabPageConnection.Size = new System.Drawing.Size(336, 310);
			this.tabPageConnection.TabIndex = 0;
			this.tabPageConnection.Text = "Upgrade Database";
			this.tabPageConnection.UseVisualStyleBackColor = true;
			// 
			// radioButtonConnectionUserPassword
			// 
			this.radioButtonConnectionUserPassword.Location = new System.Drawing.Point(32, 134);
			this.radioButtonConnectionUserPassword.Name = "radioButtonConnectionUserPassword";
			this.radioButtonConnectionUserPassword.Size = new System.Drawing.Size(280, 16);
			this.radioButtonConnectionUserPassword.TabIndex = 0;
			this.radioButtonConnectionUserPassword.Text = "Use Sql Server Authentication";
			this.radioButtonConnectionUserPassword.CheckedChanged += new System.EventHandler(this.radioButtonConnectionUsePasswordIntegratedSecurity_CheckedChanged);
			// 
			// tabPageCreation
			// 
			this.tabPageCreation.Controls.Add(this._cbCreateDatabase);
			this.tabPageCreation.Controls.Add(this.textBoxCreationDatabaseName);
			this.tabPageCreation.Controls.Add(this.textBoxCreationPassword);
			this.tabPageCreation.Controls.Add(this.textBoxCreationUserName);
			this.tabPageCreation.Controls.Add(this.radioButtonCreationUserPassword);
			this.tabPageCreation.Controls.Add(this.radioButtonCreationIntegratedSecurity);
			this.tabPageCreation.Controls.Add(this.buttonCreationRefresh);
			this.tabPageCreation.Controls.Add(this.comboBoxCreationServerName);
			this.tabPageCreation.Controls.Add(this.labelCreationPassword);
			this.tabPageCreation.Controls.Add(this.labelCreationUserName);
			this.tabPageCreation.Controls.Add(this.labelCreationDatabaseName);
			this.tabPageCreation.Controls.Add(this.labelCreationLogon);
			this.tabPageCreation.Controls.Add(this.labelCreationServerName);
			this.tabPageCreation.Controls.Add(this.labelCreationInstruction);
			this.tabPageCreation.Location = new System.Drawing.Point(4, 22);
			this.tabPageCreation.Name = "tabPageCreation";
			this.tabPageCreation.Size = new System.Drawing.Size(336, 310);
			this.tabPageCreation.TabIndex = 1;
			this.tabPageCreation.Text = "Create New Database";
			this.tabPageCreation.UseVisualStyleBackColor = true;
			// 
			// _cbCreateDatabase
			// 
			this._cbCreateDatabase.AutoSize = true;
			this._cbCreateDatabase.Checked = true;
			this._cbCreateDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
			this._cbCreateDatabase.Location = new System.Drawing.Point(19, 270);
			this._cbCreateDatabase.Name = "_cbCreateDatabase";
			this._cbCreateDatabase.Size = new System.Drawing.Size(106, 17);
			this._cbCreateDatabase.TabIndex = 25;
			this._cbCreateDatabase.Text = "Create Database";
			this._cbCreateDatabase.UseVisualStyleBackColor = true;
			// 
			// textBoxCreationDatabaseName
			// 
			this.textBoxCreationDatabaseName.Location = new System.Drawing.Point(32, 112);
			this.textBoxCreationDatabaseName.Name = "textBoxCreationDatabaseName";
			this.textBoxCreationDatabaseName.Size = new System.Drawing.Size(229, 20);
			this.textBoxCreationDatabaseName.TabIndex = 10;
			// 
			// textBoxCreationPassword
			// 
			this.textBoxCreationPassword.Enabled = false;
			this.textBoxCreationPassword.Location = new System.Drawing.Point(120, 240);
			this.textBoxCreationPassword.Name = "textBoxCreationPassword";
			this.textBoxCreationPassword.PasswordChar = '*';
			this.textBoxCreationPassword.Size = new System.Drawing.Size(193, 20);
			this.textBoxCreationPassword.TabIndex = 14;
			// 
			// textBoxCreationUserName
			// 
			this.textBoxCreationUserName.Enabled = false;
			this.textBoxCreationUserName.Location = new System.Drawing.Point(120, 216);
			this.textBoxCreationUserName.Name = "textBoxCreationUserName";
			this.textBoxCreationUserName.Size = new System.Drawing.Size(193, 20);
			this.textBoxCreationUserName.TabIndex = 13;
			// 
			// radioButtonCreationUserPassword
			// 
			this.radioButtonCreationUserPassword.Location = new System.Drawing.Point(32, 192);
			this.radioButtonCreationUserPassword.Name = "radioButtonCreationUserPassword";
			this.radioButtonCreationUserPassword.Size = new System.Drawing.Size(280, 16);
			this.radioButtonCreationUserPassword.TabIndex = 12;
			this.radioButtonCreationUserPassword.Text = "Use a specific user name and password";
			this.radioButtonCreationUserPassword.CheckedChanged += new System.EventHandler(this.radioButtonCreationUsePassword_CheckedChanged);
			// 
			// radioButtonCreationIntegratedSecurity
			// 
			this.radioButtonCreationIntegratedSecurity.Checked = true;
			this.radioButtonCreationIntegratedSecurity.Location = new System.Drawing.Point(32, 168);
			this.radioButtonCreationIntegratedSecurity.Name = "radioButtonCreationIntegratedSecurity";
			this.radioButtonCreationIntegratedSecurity.Size = new System.Drawing.Size(280, 16);
			this.radioButtonCreationIntegratedSecurity.TabIndex = 11;
			this.radioButtonCreationIntegratedSecurity.TabStop = true;
			this.radioButtonCreationIntegratedSecurity.Text = "Use Windows NT Integrated security";
			this.radioButtonCreationIntegratedSecurity.CheckedChanged += new System.EventHandler(this.radioButtonCreationIntegratedSecurity_CheckedChanged);
			// 
			// buttonCreationRefresh
			// 
			this.buttonCreationRefresh.Location = new System.Drawing.Point(272, 48);
			this.buttonCreationRefresh.Name = "buttonCreationRefresh";
			this.buttonCreationRefresh.Size = new System.Drawing.Size(56, 23);
			this.buttonCreationRefresh.TabIndex = 9;
			this.buttonCreationRefresh.Text = "&Refresh";
			// 
			// comboBoxCreationServerName
			// 
			this.comboBoxCreationServerName.Location = new System.Drawing.Point(32, 48);
			this.comboBoxCreationServerName.Name = "comboBoxCreationServerName";
			this.comboBoxCreationServerName.Size = new System.Drawing.Size(229, 21);
			this.comboBoxCreationServerName.TabIndex = 8;
			this.comboBoxCreationServerName.DropDown += new System.EventHandler(this.comboBoxCreationServerName_DropDown);
			// 
			// labelCreationPassword
			// 
			this.labelCreationPassword.Location = new System.Drawing.Point(48, 240);
			this.labelCreationPassword.Name = "labelCreationPassword";
			this.labelCreationPassword.Size = new System.Drawing.Size(64, 16);
			this.labelCreationPassword.TabIndex = 24;
			this.labelCreationPassword.Text = "Password:";
			// 
			// labelCreationUserName
			// 
			this.labelCreationUserName.Location = new System.Drawing.Point(48, 216);
			this.labelCreationUserName.Name = "labelCreationUserName";
			this.labelCreationUserName.Size = new System.Drawing.Size(64, 16);
			this.labelCreationUserName.TabIndex = 23;
			this.labelCreationUserName.Text = "User Name: ";
			// 
			// labelCreationDatabaseName
			// 
			this.labelCreationDatabaseName.Location = new System.Drawing.Point(16, 88);
			this.labelCreationDatabaseName.Name = "labelCreationDatabaseName";
			this.labelCreationDatabaseName.Size = new System.Drawing.Size(300, 16);
			this.labelCreationDatabaseName.TabIndex = 22;
			this.labelCreationDatabaseName.Text = "2. New Database Name";
			// 
			// labelCreationLogon
			// 
			this.labelCreationLogon.Location = new System.Drawing.Point(16, 144);
			this.labelCreationLogon.Name = "labelCreationLogon";
			this.labelCreationLogon.Size = new System.Drawing.Size(300, 16);
			this.labelCreationLogon.TabIndex = 21;
			this.labelCreationLogon.Text = "3. Enter information to logon to server";
			// 
			// labelCreationServerName
			// 
			this.labelCreationServerName.Location = new System.Drawing.Point(16, 32);
			this.labelCreationServerName.Name = "labelCreationServerName";
			this.labelCreationServerName.Size = new System.Drawing.Size(300, 16);
			this.labelCreationServerName.TabIndex = 20;
			this.labelCreationServerName.Text = "1. Select or enter a server name";
			// 
			// labelCreationInstruction
			// 
			this.labelCreationInstruction.Location = new System.Drawing.Point(8, 8);
			this.labelCreationInstruction.Name = "labelCreationInstruction";
			this.labelCreationInstruction.Size = new System.Drawing.Size(312, 16);
			this.labelCreationInstruction.TabIndex = 19;
			this.labelCreationInstruction.Text = "Select the following to connect to SQL server data:";
			// 
			// chkSaveSettings
			// 
			this.chkSaveSettings.AutoSize = true;
			this.chkSaveSettings.Location = new System.Drawing.Point(12, 354);
			this.chkSaveSettings.Name = "chkSaveSettings";
			this.chkSaveSettings.Size = new System.Drawing.Size(92, 17);
			this.chkSaveSettings.TabIndex = 15;
			this.chkSaveSettings.Text = "Save Settings";
			this.chkSaveSettings.UseVisualStyleBackColor = true;
			// 
			// IdentifyDatabaseForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(344, 368);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(358, 383);
			this.Controls.Add(this.chkSaveSettings);
			this.Controls.Add(this.tabControlChooseDatabase);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "IdentifyDatabaseForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Identify Database";
			this.tabControlChooseDatabase.ResumeLayout(false);
			this.tabPageConnection.ResumeLayout(false);
			this.tabPageConnection.PerformLayout();
			this.tabPageCreation.ResumeLayout(false);
			this.tabPageCreation.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		public IdentifyDatabaseForm()
		{
			InitializeComponent();

			FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			fi = new FileInfo(Path.Combine(fi.DirectoryName, "installsettings.xml"));
			if (fi.Exists)
			{
				XmlDocument document = new XmlDocument();
				document.Load(fi.FullName);
				chkSaveSettings.Checked = true;

				comboBoxConnectionServerName.Text = XmlHelper.GetNodeValue(document.DocumentElement, "server", comboBoxConnectionServerName.Text);
				radioButtonConnectionIntegratedSecurity.Checked = XmlHelper.GetNodeValue(document.DocumentElement, "useintegratedsecurity", radioButtonConnectionIntegratedSecurity.Checked);
				radioButtonConnectionUserPassword.Checked = !radioButtonConnectionIntegratedSecurity.Checked;
				textBoxConnectionUserName.Text = XmlHelper.GetNodeValue(document.DocumentElement, "username", textBoxConnectionUserName.Text);
				textBoxConnectionPassword.Text = XmlHelper.GetNodeValue(document.DocumentElement, "password", textBoxConnectionPassword.Text);
				comboBoxConnectionDatabaseName.Text = XmlHelper.GetNodeValue(document.DocumentElement, "database", comboBoxConnectionDatabaseName.Text);

				comboBoxCreationServerName.Text = XmlHelper.GetNodeValue(document.DocumentElement, "server", comboBoxCreationServerName.Text);
				textBoxCreationDatabaseName.Text = XmlHelper.GetNodeValue(document.DocumentElement, "database", textBoxCreationDatabaseName.Text);
				radioButtonCreationIntegratedSecurity.Checked = XmlHelper.GetNodeValue(document.DocumentElement, "useintegratedsecurity", radioButtonCreationIntegratedSecurity.Checked);
				radioButtonCreationUserPassword.Checked = !radioButtonCreationIntegratedSecurity.Checked;
				textBoxCreationUserName.Text = XmlHelper.GetNodeValue(document.DocumentElement, "username", textBoxCreationUserName.Text);
				textBoxCreationPassword.Text = XmlHelper.GetNodeValue(document.DocumentElement, "password", textBoxCreationPassword.Text);

			}

		}

		private void SaveSettings()
		{
			FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			string fileName = Path.Combine(fi.DirectoryName, "installsettings.xml");
			if (File.Exists(fileName)) File.Delete(fileName);
			System.Threading.Thread.Sleep(500);

			if (chkSaveSettings.Checked)
			{
				XmlDocument document = new XmlDocument();
				document.LoadXml("<a></a>");

				if (tabControlChooseDatabase.SelectedIndex == 0)
				{
					XmlHelper.AddElement(document.DocumentElement, "server", comboBoxConnectionServerName.Text);
					XmlHelper.AddElement(document.DocumentElement, "useintegratedsecurity", radioButtonConnectionIntegratedSecurity.Checked.ToString().ToLower());
					XmlHelper.AddElement(document.DocumentElement, "username", textBoxConnectionUserName.Text);
					XmlHelper.AddElement(document.DocumentElement, "password", textBoxConnectionPassword.Text);
					XmlHelper.AddElement(document.DocumentElement, "database", comboBoxConnectionDatabaseName.Text);
				}
				else
				{
					XmlHelper.AddElement(document.DocumentElement, "server", comboBoxCreationServerName.Text);
					XmlHelper.AddElement(document.DocumentElement, "database", textBoxCreationDatabaseName.Text);
					XmlHelper.AddElement(document.DocumentElement, "useintegratedsecurity", radioButtonCreationIntegratedSecurity.Checked.ToString().ToLower());
					XmlHelper.AddElement(document.DocumentElement, "username", textBoxCreationUserName.Text);
					XmlHelper.AddElement(document.DocumentElement, "password", textBoxCreationPassword.Text);
				}

				document.Save(fileName);
			}

		}
		#endregion

		#region Tab1 Connection

		private void comboBoxConnectionServerName_DropDown(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				if (comboBoxConnectionServerName.Items.Count == 0)
				{
					comboBoxConnectionServerName.DataSource = SqlServers.GetServers();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}

		}

		private void buttonConnectionRefresh_Click(object sender, System.EventArgs e)
		{
			comboBoxConnectionServerName.DataSource = SqlServers.GetServers();
		}

		private void buttonConnectionTestConnection_Click(object sender, System.EventArgs e)
		{
			string connectString = SqlServers.BuildConnectionString(radioButtonConnectionIntegratedSecurity.Checked, comboBoxConnectionDatabaseName.Text, comboBoxConnectionServerName.Text, textBoxConnectionUserName.Text, textBoxConnectionPassword.Text);
			bool valid = SqlServers.TestConnectionString(connectString);
			if (valid)
			{
				MessageBox.Show("Connection Succeeded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("The information does not describe a valid connection string.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void radioButtonConnectionUsePasswordIntegratedSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxConnectionPassword.Enabled = true;
			textBoxConnectionUserName.Enabled = true;
		}

		private void radioButtonConnectionIntegratedSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxConnectionPassword.Enabled = false;
			textBoxConnectionUserName.Enabled = false;
		}

		private void comboBoxConnectionDatabaseName_DropDown(object sender, System.EventArgs e)
		{
			comboBoxConnectionDatabaseName.DataSource = GetDatabaseNames();
		}
		#endregion

		#region OK/Cancel Handlers
		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.SaveSettings();
			if (tabControlChooseDatabase.SelectedTab == this.tabPageConnection)
			{
				string connectString = SqlServers.BuildConnectionString(radioButtonConnectionIntegratedSecurity.Checked, comboBoxConnectionDatabaseName.Text, comboBoxConnectionServerName.Text, textBoxConnectionUserName.Text, textBoxConnectionPassword.Text);
				bool valid = SqlServers.TestConnectionString(connectString);
				if (valid)
				{
					mConnectionString = connectString;
					mDatabaseName = comboBoxConnectionServerName.Text + "." + comboBoxConnectionDatabaseName.Text;
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				else
				{
					MessageBox.Show("The information does not describe a valid connection string.");
				}
			}
			else
			{
				bool error = false;
				if (_cbCreateDatabase.Checked)
				{
					error = CreateDatabase();
				}

				if (!error)
				{
					string outputConnectString = SqlServers.BuildConnectionString(radioButtonCreationIntegratedSecurity.Checked, textBoxCreationDatabaseName.Text, comboBoxCreationServerName.Text, textBoxCreationUserName.Text, textBoxCreationPassword.Text);
					if (SqlServers.TestConnectionString(outputConnectString))
					{
						mConnectionString = outputConnectString;
						mDatabaseName = comboBoxCreationServerName.Text + "." + textBoxCreationDatabaseName.Text;
						_createdDb = true;
					}
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}

		}

		private bool CreateDatabase()
		{
			bool error = false;
			string connectString = SqlServers.BuildConnectionString(radioButtonCreationIntegratedSecurity.Checked, string.Empty, comboBoxCreationServerName.Text, textBoxCreationUserName.Text, textBoxCreationPassword.Text);
			if (SqlServers.TestConnectionString(connectString) && SqlServers.HasCreatePermissions(connectString))
			{
				try
				{
					SqlServers.CreateDatabase(connectString, textBoxCreationDatabaseName.Text);
				}
				catch (Exception ex)
				{
					error = true;
					System.Diagnostics.Debug.WriteLine(ex.ToString());
					MessageBox.Show("Could not create database." + Environment.NewLine + ex.Message);
				}
			}
			else
			{
				error = true;
				MessageBox.Show("The account does not have permissions to create a database on this server.");
			}
			return error;
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region Tab2 Creation
		private void comboBoxCreationServerName_DropDown(object sender, System.EventArgs e)
		{
			if (comboBoxCreationServerName.Items.Count == 0)
			{
				comboBoxCreationServerName.DataSource = SqlServers.GetServers();
			}
		}

		private void buttonCreationRefresh_Click(object sender, System.EventArgs e)
		{
			comboBoxCreationServerName.DataSource = SqlServers.GetServers();
		}

		private void radioButtonCreationUsePassword_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxCreationPassword.Enabled = true;
			textBoxCreationUserName.Enabled = true;
		}

		private void radioButtonCreationIntegratedSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxCreationPassword.Enabled = false;
			textBoxCreationUserName.Enabled = false;
		}


		#endregion

		#region Public Properties

		public string ConnectionString
		{
			get { return mConnectionString; }
		}

		public string DatabaseName
		{
			get { return mDatabaseName; }
		}

		public bool CreatedDatabase
		{
			get
			{
				return _createdDb;
			}
		}
		#endregion

		private string[] GetDatabaseNames()
		{
			string connectString = SqlServers.BuildConnectionString(radioButtonConnectionIntegratedSecurity.Checked, comboBoxConnectionDatabaseName.Text, comboBoxConnectionServerName.Text, textBoxConnectionUserName.Text, textBoxConnectionPassword.Text);
			return SqlServers.GetDatabaseNames(connectString);
		}

	}

	#region XmlHelper

	public class XmlHelper
	{
		private XmlHelper()
		{
		}

		public static XPathNavigator CreateXPathNavigator(XmlReader reader)
		{
			XPathDocument document = new XPathDocument(reader);
			return document.CreateNavigator();
		}

		public static XPathNodeIterator GetIterator(XPathNavigator navigator, string xPath)
		{
			return (XPathNodeIterator)navigator.Evaluate(xPath);
		}

		#region GetXmlReader

		public static XmlReader GetXmlReader(FileInfo fileInfo)
		{
			XmlTextReader textReader = new XmlTextReader(fileInfo.FullName);
			return textReader;
		}

		#endregion

		#region GetNode

		public static System.Xml.XmlNode GetNode(System.Xml.XmlNode xmlNode, string XPath)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = xmlNode.SelectSingleNode(XPath);
				return node;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static System.Xml.XmlNode GetNode(System.Xml.XmlNode xmlNode, string XPath, XmlNamespaceManager nsManager)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = xmlNode.SelectSingleNode(XPath, nsManager);
				return node;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region GetNodeValue

		public static string GetNodeValue(System.Xml.XmlDocument document, string XPath, string defaultValue)
		{
			try
			{
				return GetNodeValue(document.DocumentElement, XPath, defaultValue);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string GetNodeValue(System.Xml.XmlNode element, string XPath, string defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return node.InnerText;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static int GetNodeValue(System.Xml.XmlNode element, string XPath, int defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return int.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static int? GetNodeValue(System.Xml.XmlNode element, string XPath, int? defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return int.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static Single GetNodeValue(System.Xml.XmlNode element, string XPath, Single defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return Single.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static Single? GetNodeValue(System.Xml.XmlNode element, string XPath, Single? defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return Single.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static double GetNodeValue(System.Xml.XmlNode element, string XPath, double defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return double.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static double? GetNodeValue(System.Xml.XmlNode element, string XPath, double? defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return double.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static bool GetNodeValue(System.Xml.XmlNode element, string XPath, bool defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return bool.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static bool? GetNodeValue(System.Xml.XmlNode element, string XPath, bool? defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return bool.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static DateTime GetNodeValue(System.Xml.XmlNode element, string XPath, DateTime defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return DateTime.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static DateTime? GetNodeValue(System.Xml.XmlNode element, string XPath, DateTime? defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return DateTime.Parse(node.InnerText);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static string GetNodeValue(System.Xml.XmlDocument document, string XPath, XmlNamespaceManager nsManager, string defaultValue)
		{
			try
			{
				return GetNodeValue(document.DocumentElement, XPath, nsManager, defaultValue);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string GetNodeValue(System.Xml.XmlNode element, string XPath, XmlNamespaceManager nsManager, string defaultValue)
		{
			try
			{
				System.Xml.XmlNode node = null;
				node = element.SelectSingleNode(XPath, nsManager);
				if (node == null)
					return defaultValue;
				else
					return node.InnerText;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#endregion

		#region GetNodeXML

		public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue, bool useOuter)
		{
			try
			{
				XmlNode node = null;
				node = document.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else if (useOuter)
					return node.OuterXml;
				else
					return node.InnerXml;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue)
		{
			return GetNodeXML(document, XPath, defaultValue, false);
		}

		#endregion

		#region GetAttributeValue

		public static string GetAttribute(XmlNode node, string attributeName)
		{
			return GetAttribute(node, attributeName, "");
		}

		public static string GetAttribute(XmlNode node, string attributeName, string defaultValue)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
				attr = node.Attributes[attributeName.ToLower()];

			if (attr == null)
				return defaultValue;
			else
				return attr.Value;
		}

		public static Guid GetAttribute(XmlNode node, string attributeName, Guid defaultValue)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
				attr = node.Attributes[attributeName.ToLower()];

			if (attr == null)
				return defaultValue;
			else
				return new Guid(attr.Value);
		}

		public static double GetAttribute(XmlNode node, string attributeName, double defaultValue)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
				attr = node.Attributes[attributeName.ToLower()];

			if (attr == null)
				return defaultValue;
			else
				return double.Parse(attr.Value);
		}

		public static int GetAttribute(XmlNode node, string attributeName, int defaultValue)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
				attr = node.Attributes[attributeName.ToLower()];

			if (attr == null)
				return defaultValue;
			else
				return int.Parse(attr.Value);
		}

		public static long GetAttribute(XmlNode node, string attributeName, long defaultValue)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
				attr = node.Attributes[attributeName.ToLower()];

			if (attr == null)
				return defaultValue;
			else
				return long.Parse(attr.Value);
		}

		public static bool GetAttribute(XmlNode node, string attributeName, bool defaultValue)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			if (attr == null)
				attr = node.Attributes[attributeName.ToLower()];

			if (attr == null)
				return defaultValue;
			else
				return bool.Parse(attr.Value);
		}

		#endregion

		#region AddElement

		public static XmlNode AddElement(XmlElement element, string name, string value)
		{
			XmlDocument document = null;
			XmlElement elemNew = null;

			document = element.OwnerDocument;
			elemNew = document.CreateElement(name);
			if (value.GetType() == typeof(string))
			{
				if (value != "")
					elemNew.InnerText = value;
			}
			else
				elemNew.InnerText = value;

			return element.AppendChild(elemNew);
		}

		public static XmlNode AddElement(XmlDocument document, string name, string value)
		{
			XmlElement elemNew = document.CreateElement(name);
			if (value.GetType() == typeof(string))
			{
				if (value != "")
					elemNew.InnerText = value;
			}
			else
				elemNew.Value = value;

			return document.AppendChild(elemNew);
		}

		public static XmlNode AddElement(XmlElement element, string name)
		{
			XmlDocument document = null;
			XmlElement elemNew = null;
			document = element.OwnerDocument;
			elemNew = document.CreateElement(name);
			return element.AppendChild(elemNew);
		}

		public static XmlNode AddElement(XmlDocument XMLDocument, string name)
		{
			XmlElement elemNew = null;
			elemNew = XMLDocument.CreateElement(name);
			return XMLDocument.AppendChild(elemNew);
		}

		public static XmlAttribute AddAttribute(XmlElement XmlElement, string name, string value)
		{
			XmlDocument docOwner = null;
			XmlAttribute attrNew = null;

			docOwner = XmlElement.OwnerDocument;
			attrNew = docOwner.CreateAttribute(name);
			attrNew.InnerText = value;
			XmlElement.Attributes.Append(attrNew);
			return attrNew;
		}

		#endregion

		#region RemoveElement

		public static void RemoveElement(XmlDocument document, string XPath)
		{
			XmlNode parentNode = null;
			XmlNodeList nodes = null;

			nodes = document.SelectNodes(XPath);
			foreach (XmlElement node in nodes)
			{
				parentNode = node.ParentNode;
				node.RemoveAll();
				parentNode.RemoveChild(node);
			}
		}

		public static void RemoveElement(XmlElement element)
		{
			XmlNode parentNode = element.ParentNode;
			parentNode.RemoveChild(element);
		}

		public static void RemoveAttribute(XmlElement element, string attributeName)
		{
			XmlAttribute attrDelete = null;
			attrDelete = (XmlAttribute)element.Attributes.GetNamedItem(attributeName);
			element.Attributes.Remove(attrDelete);
		}

		#endregion

		#region UpdateElement

		public static void UpdateElement(XmlElement element, string newValue)
		{
			element.InnerText = newValue;
		}

		public static void UpdateElement(ref XmlDocument XMLDocument, string Xpath, string newValue)
		{
			XMLDocument.SelectSingleNode(Xpath).InnerText = newValue;
		}

		public static void UpdateAttribute(XmlElement XmlElement, string attributeName, string newValue)
		{
			XmlAttribute attrTemp = null;
			attrTemp = (XmlAttribute)XmlElement.Attributes.GetNamedItem(attributeName);
			attrTemp.InnerText = newValue;
		}

		#endregion

		#region GetElement

		public static XmlElement GetElement(XmlElement parentElement, string tagName)
		{
			XmlNodeList list = parentElement.GetElementsByTagName(tagName);
			if (list.Count > 0)
				return (XmlElement)list[0];
			else
				return null;
		}

		#endregion
	}

	#endregion

}
