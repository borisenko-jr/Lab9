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
		/// Считывание  текста из файла.
		/// </summary>
		/// <param name="name">Название файла.</param>
		/// <returns></returns>
		private string GetSql(string name)
		{
			try
			{
				// Получаем текущий объект класса Assembly
				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
				// Получаем объект класса Stream к ресурсам текущей сборки.
				System.IO.Stream str = asm.GetManifestResourceStream(asm.GetName().Name + "." + name);
				// Считываем и возвращаем содержимое.
				System.IO.StreamReader reader = new System.IO.StreamReader(str);
				return reader.ReadToEnd();
			}
			catch(Exception ex)
			{
				// Отлавливаем возникшие исключения.
				System.Windows.Forms.MessageBox.Show("В методе GetSql возникла ошибка: "+ex.Message);
				throw ex;
			}		
		}
		/// <summary>
		/// Выполнение SQL - запроса.
		/// </summary>
		/// <param name="databaseName">Название базы данных.</param>
		/// <param name="sql">Текст команды.</param>
		private void ExecuteSql(string databaseName, string sql)
		{
			// Создаем объект класса SqlCommand.
			System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand(sql, sqlConnection1);
			// Открываем соединение.
			comm.Connection.Open();
			// Изменяем используемую базу данных.
			comm.Connection.ChangeDatabase(databaseName);
			try
			{
				// Выполняем команду.
				comm.ExecuteNonQuery();
			}
			finally
			{
				// Закрываем соединение.
				comm.Connection.Close();
			}
		}
		/// <summary>
		/// Создание таблицы в базе данных.
		/// </summary>
		/// <param name="databaseName">Название базы данных.</param>
		protected void AddDBTable(string databaseName)
		{			
			try
			{
				// Создаем новую базу данных.
				this.ExecuteSql("master", "CREATE DATABASE " + databaseName);
				// Создаем таблицу в новой базе данных.
				this.ExecuteSql(databaseName, this.GetSql("sqlScript.txt"));
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("В методе AddDBTable возникла ошибка: "+ex.Message);
			}
		}
		/// <summary>
		/// Перегружаем метод Install
		/// </summary>
		/// <param name="stateSaver">Параметр по умолчанию.</param>
		public override void Install(IDictionary stateSaver)
		{		
			base.Install (stateSaver);
			// В качестве названия БД передаем введенное значение пользователем.
			this.AddDBTable(this.Context.Parameters["dbname"]);
		}

	
	}
}
