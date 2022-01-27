using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Miralissa.Server.Classes;
using Miralissa.Shared;

namespace Miralissa.Server.Repository.PersonalData
{
	//TODO Ну это никуда не годится
	public static class PersonalDataRawRepository
	{
		private static string _connectionString = "Data Source=****;Initial Catalog=****;Integrated Security=True; MultipleActiveResultSets=true";

		private static SqlConnection CreateConnection(bool needOpen = true)
		{
			SqlConnection connection = new SqlConnection(_connectionString);

			if (needOpen)
				connection.Open();

			return connection;
		}

        public static IEnumerable<PDelement> SearchPersonalData(string surname, string name, string patronymic)
        {
            foreach (PDelement data in SearchPersonalDataTns(surname, name, patronymic))
                yield return data;

            foreach (PDelement data in SearchPersonalDataFlash(surname, name, patronymic))
                yield return data;

            foreach (PDelement data in SearchPersonalDataBillingInd(surname, name, patronymic))
                yield return data;

            foreach (PDelement data in SearchPersonalDataHandy(surname, name, patronymic))
                yield return data;
        }

        internal static IEnumerable<PDelement> SearchPersonalDataTns(string surname, string name, string patronymic)
        {
            if (!string.IsNullOrEmpty(surname))
            {
                surname = surname.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }
            else
            {
                surname = "NO_SURNAME";
            }

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            if (!string.IsNullOrEmpty(patronymic))
            {
                patronymic = patronymic.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(string.Empty, connection))
            using (SqlDataIterator reader = new SqlDataIterator(cmd))
            {
                cmd.CommandText = @"SELECT [Surname],
                                   [Name],
                            	   [Patronymic],
                            	   [DateBirth],
                            	   [PlaceBirth],
                            	   [Address] 
                            FROM [dbo].[TNS] AS [Pers]" +
                                    $"INNER JOIN CONTAINSTABLE([dbo].[TNS], [Surname], '\"{surname}\"') AS [FTS] ON [FTS].[KEY] = [Pers].[ID]" +
                                    "WHERE CONTAINS([Name], 'FORMSOF(THESAURUS, {0})')";

                cmd.CommandText = string.Format(cmd.CommandText, "\"" + name + "\"");

                if (!string.IsNullOrWhiteSpace(patronymic))
                {
                    cmd.CommandText += string.Format(" AND CONTAINS([Patronymic], 'FORMSOF(THESAURUS, \"{0}\")')", patronymic);
                }

                reader.InitReader();
                while (reader.Read())
                {
                    yield return new PDelement
                    {
                        Source = "ТНС Энерго",
                        Surname = reader.GetData<string>(),
                        Name = reader.GetData<string>(),
                        Patronymic = reader.GetData<string>(),
                        DateBirth = reader.GetData<DateTime?>(),
                        PlaceBirth = reader.GetData<string>(),
                        Address = reader.GetData<string>(),
                    };
                }
            }
        }

        internal static IEnumerable<PDelement> SearchPersonalDataFlash(string surname, string name, string patronymic)
        {
            if (!string.IsNullOrEmpty(surname))
            {
                surname = surname.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }
            else
            {
                surname = "NO_SURNAME";
            }

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            if (!string.IsNullOrEmpty(patronymic))
            {
                patronymic = patronymic.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(string.Empty, connection))
            using (SqlDataIterator reader = new SqlDataIterator(cmd))
            {
                cmd.CommandText = @"SELECT [Surname],
                                   [Name],
                                   [Patronymic],
                                   [DateBirth],
                                   [PlaceBirth],
                                   [INN],
                                   [SNILS],
                                   [Address],
                                   [PassSeria],
                                   [PassNumber],
                                   [PassIssueDate],
                                   [PassIssueAddr]
                            FROM [dbo].[FlashData] AS [Pers]" +
                                    string.Format("INNER JOIN CONTAINSTABLE([dbo].[FlashData], [Surname], '\"{0}\"') AS [FTS] ON [FTS].[KEY] = [Pers].[ID]", surname);

                if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(patronymic))
                {
                    cmd.CommandText += "WHERE ";
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    cmd.CommandText += string.Format(" CONTAINS([Name], 'FORMSOF(THESAURUS, \"{0}\")')", name);
                }

                if (!string.IsNullOrWhiteSpace(patronymic))
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        cmd.CommandText += " AND ";
                    }
                    cmd.CommandText += string.Format("CONTAINS([Patronymic], 'FORMSOF(THESAURUS, \"{0}\")')", patronymic);
                }

                reader.InitReader();
                while (reader.Read())
                {
                    yield return new PDelement
                    {
                        Source = "Флешка",
                        Surname = reader.GetData<string>(),
                        Name = reader.GetData<string>(),
                        Patronymic = reader.GetData<string>(),
                        DateBirth = reader.GetData<DateTime?>(),
                        PlaceBirth = reader.GetData<string>(),
                        INN = reader.GetData<string>(),
                        SNILS = reader.GetData<string>(),
                        Address = reader.GetData<string>(),
                        PassSeria = reader.GetData<string>(),
                        PassNumber = reader.GetData<string>(),
                        PassIssueDate = reader.GetData<DateTime?>(),
                        PassIssueAddr = reader.GetData<string>(),
                    };
                }
            }
        }

        internal static IEnumerable<PDelement> SearchPersonalDataBillingInd(string surname, string name, string patronymic)
        {
            if (!string.IsNullOrEmpty(surname))
            {
                surname = surname.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }
            else
            {
                surname = "NO_SURNAME";
            }

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            if (!string.IsNullOrEmpty(patronymic))
            {
                patronymic = patronymic.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(string.Empty, connection))
            using (SqlDataIterator reader = new SqlDataIterator(cmd))
            {
                cmd.CommandText = @"SELECT [Surname],
                                   [Name],
                            	   [Patronymic],
                            	   [DateBirth],
                            	   [PlaceBirth],
                            	   [Address] 
                            FROM [dbo].[BillingFL] AS [Pers]" +
                                    $"INNER JOIN CONTAINSTABLE([dbo].[BillingFL], [Surname], '\"{surname}\"') AS [FTS] ON [FTS].[KEY] = [Pers].[ID]" +
                                    "WHERE CONTAINS([Name], 'FORMSOF(THESAURUS, {0})')";

                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.CommandText = string.Format(cmd.CommandText, "\"" + name + "\"");

                if (!string.IsNullOrWhiteSpace(patronymic))
                {
                    cmd.CommandText += string.Format(" AND CONTAINS([Patronymic], 'FORMSOF(THESAURUS, \"{0}\")')", patronymic);
                }

                reader.InitReader();
                while (reader.Read())
                {
                    yield return new PDelement
                    {
                        Source = "Биллинг ФЛ",
                        Surname = reader.GetData<string>(0),
                        Name = reader.GetData<string>(1),
                        Patronymic = reader.GetData<string>(2),
                        DateBirth = reader.GetData<DateTime?>(3),
                        PlaceBirth = reader.GetData<string>(4),
                        Address = reader.GetData<string>(5),
                    };
                }
            }
        }

        internal static IEnumerable<PDelement> SearchPersonalDataHandy(string surname, string name, string patronymic)
        {
            if (!string.IsNullOrEmpty(surname))
            {
                surname = surname.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }
            else
            {
                surname = "NO_SURNAME";
            }

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            if (!string.IsNullOrEmpty(patronymic))
            {
                patronymic = patronymic.Replace('*', '_').Replace("/", "").Replace("\"", "");
            }

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(string.Empty, connection))
            using (SqlDataIterator reader = new SqlDataIterator(cmd))
            {
                cmd.CommandText = @"SELECT [Surname],
                                   [Name],
                            	   [Patronymic],
                            	   [DateBirth],
                            	   [PlaceBirth],
                            	   [Address],
                                   [INN],
                                   [SNILS],
                                   [PassSeria],
                                   [PassNumber],
                                   [PassIssueDate],
                                   [PassIssueAddr],
                                   [LC].[Comment]
                            FROM [dbo].[ProcessedPersonal] AS [Pers]" +
                                    $"INNER JOIN CONTAINSTABLE([dbo].[ProcessedPersonal], [Surname], '\"{surname}\"') AS [FTS] ON [FTS].[KEY] = [Pers].[ID]" +
                                    "LEFT OUTER JOIN [dbo].[load_comment] AS [LC] ON [LC].[ID] = [ID_Comment]" +
                                    "WHERE CONTAINS([Name], 'FORMSOF(THESAURUS, {0})')";

                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.CommandText = string.Format(cmd.CommandText, "\"" + name + "\"");

                if (!string.IsNullOrWhiteSpace(patronymic))
                {
                    cmd.CommandText += string.Format(" AND CONTAINS([Patronymic], 'FORMSOF(THESAURUS, \"{0}\")')", patronymic);
                }

                reader.InitReader();
                while (reader.Read())
                {
                    yield return new PDelement
                    {
                        Surname = reader.GetData<string>(),
                        Name = reader.GetData<string>(),
                        Patronymic = reader.GetData<string>(),
                        DateBirth = reader.GetData<DateTime?>(),
                        PlaceBirth = reader.GetData<string>(),
                        Address = reader.GetData<string>(),
                        INN = reader.GetData<string>(),
                        SNILS = reader.GetData<string>(),
                        PassSeria = reader.GetData<string>(),
                        PassNumber = reader.GetData<string>(),
                        PassIssueDate = reader.GetData<DateTime?>(),
                        PassIssueAddr = reader.GetData<string>(),
                        Source = reader.GetData<string>() ?? " << не задано >>",
                    };
                }
            }
        }
    }
}
