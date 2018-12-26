using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace InstallerClass
{
	/// <summary>
	/// Summary description for CustomActionDB.
	/// </summary>
	[RunInstaller(true)]
	public class CustomActionDB : System.Configuration.Install.Installer
	{
		private System.Data.SqlClient.SqlConnection sqlConnection1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CustomActionDB()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
			// 
			// sqlConnection1
			// 
			this.sqlConnection1.ConnectionString = "workstation id=7EA2B2F6068D473;packet size=4096;integrated security=SSPI;data sou" +
				"rce=\".\";persist security info=False;initial catalog=master";

		}
		#endregion

		/// <summary>
		/// ����������  ������ �� �����.
		/// </summary>
		/// <param name="name">�������� �����.</param>
		/// <returns></returns>
		private string GetSql(string name)
		{
			try
			{
				// �������� ������� ������ ������ Assembly
				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
				// �������� ������ ������ Stream � �������� ������� ������.
				System.IO.Stream str = asm.GetManifestResourceStream(asm.GetName().Name + "." + name);
				// ��������� � ���������� ����������.
				System.IO.StreamReader reader = new System.IO.StreamReader(str);
				return reader.ReadToEnd();
			}
			catch(Exception ex)
			{
				// ����������� ��������� ����������.
				System.Windows.Forms.MessageBox.Show("� ������ GetSql �������� ������: "+ex.Message);
				throw ex;
			}		
		}
		/// <summary>
		/// ���������� SQL - �������.
		/// </summary>
		/// <param name="databaseName">�������� ���� ������.</param>
		/// <param name="sql">����� �������.</param>
		private void ExecuteSql(string databaseName, string sql)
		{
			// ������� ������ ������ SqlCommand.
			System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand(sql, sqlConnection1);
			// ��������� ����������.
			comm.Connection.Open();
			// �������� ������������ ���� ������.
			comm.Connection.ChangeDatabase(databaseName);
			try
			{
				// ��������� �������.
				comm.ExecuteNonQuery();
			}
			finally
			{
				// ��������� ����������.
				comm.Connection.Close();
			}
		}
		/// <summary>
		/// �������� ������� � ���� ������.
		/// </summary>
		/// <param name="databaseName">�������� ���� ������.</param>
		protected void AddDBTable(string databaseName)
		{			
			try
			{
				// ������� ����� ���� ������.
				this.ExecuteSql("master", "CREATE DATABASE " + databaseName);
				// ������� ������� � ����� ���� ������.
				this.ExecuteSql(databaseName, this.GetSql("sqlScript.txt"));
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("� ������ AddDBTable �������� ������: "+ex.Message);
			}
		}
		/// <summary>
		/// ����������� ����� Install
		/// </summary>
		/// <param name="stateSaver">�������� �� ���������.</param>
		public override void Install(IDictionary stateSaver)
		{		
			base.Install (stateSaver);
			// � �������� �������� �� �������� ��������� �������� �������������.
			this.AddDBTable(this.Context.Parameters["dbname"]);
		}

	
	}
}
