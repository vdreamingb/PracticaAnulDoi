using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System;


namespace Practica.Classes
{
    public class EditDataService
    {
        private string tableName { get; set; }
        private System.Windows.Controls.DataGrid grid { get; set; }
        public EditDataService() { }
        public EditDataService(string tableName, System.Windows.Controls.DataGrid grid)
        {
            this.tableName = tableName;
            this.grid = grid;
        }

        public void deleteData()
        {
            DataBase db = new DataBase();

            using var connection = db.GetConnection();
            connection.Open();

            string idColumn = tableName == "Localitati" ? "CodLoc" : "CodBen";

            string query = $"DELETE FROM {tableName} WHERE {idColumn} = @Id";




            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (grid.SelectedItem == null)
                    return;
                dynamic selectedItem = grid.SelectedItem;

                command.Parameters.AddWithValue("@Id", selectedItem[idColumn]);

                command.ExecuteNonQuery();
            }
        }
        public void updateData()
        {
            grid.CommitEdit();
            grid.CommitEdit();

            if (grid.SelectedItem == null)
                return;

            DataBase db = new DataBase();

            using var connection = db.GetConnection();

            connection.Open();

            dynamic selectedItem = grid.SelectedItem;

            string query = "";

            if (tableName == "Localitati")
            {
                query = @"UPDATE Localitati
                  SET NumeLoc = @NumeLoc,
                      Tip = @Tip,
                      Judet = @Judet
                  WHERE CodLoc = @Id";
            }
            else
            {
                query = @"UPDATE Beneficiari
                  SET NrBen = @NrBen,
                      Nume = @Nume,
                      Prenume = @Prenume,
                      Adresa = @Adresa,
                      Telefon = @Telefon,
                      Email = @Email,
                      CodLoc = @CodLoc
                  WHERE CodBen = @Id";
            }

            using SqlCommand command = new SqlCommand(query, connection);

            if (tableName == "Localitati")
            {
                command.Parameters.AddWithValue("@NumeLoc", selectedItem["NumeLoc"]);
                command.Parameters.AddWithValue("@Tip", selectedItem["Tip"]);
                command.Parameters.AddWithValue("@Judet", selectedItem["Judet"]);

                command.Parameters.AddWithValue("@Id", selectedItem["CodLoc"]);
            }
            else
            {
                command.Parameters.AddWithValue("@NrBen", selectedItem["NrBen"]);
                command.Parameters.AddWithValue("@Nume", selectedItem["Nume"]);
                command.Parameters.AddWithValue("@Prenume", selectedItem["Prenume"]);
                command.Parameters.AddWithValue("@Adresa", selectedItem["Adresa"]);
                command.Parameters.AddWithValue("@Telefon", selectedItem["Telefon"]);
                command.Parameters.AddWithValue("@Email", selectedItem["Email"]);
                command.Parameters.AddWithValue("@CodLoc", selectedItem["CodLoc"]);

                command.Parameters.AddWithValue("@Id", selectedItem["CodBen"]);
            }

            command.ExecuteNonQuery();
        }

        public void exportExcel()
        {
            try
            {
                DataBase db = new DataBase();

                DataTable data = db.SelectData(tableName);

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                    FileName = tableName + "_Date.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(data, "Date " + tableName);
                        wb.SaveAs(saveFileDialog.FileName);
                    }
                    System.Windows.MessageBox.Show("Datele pentru excel au fost exportate cu success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A aparut o eroare in timpul exportarii datelor in excel" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


public void ExportWord()
    {
        try
        {
            DataBase db = new DataBase();
            DataTable dt = db.SelectData(tableName);

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                FileName = tableName + "_Data.docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                using (WordprocessingDocument wordDoc =
                    WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();

                    Paragraph titleParagraph = new Paragraph(
                        new Run(
                            new Text("Datele Exportate")
                        )
                    );

                    RunProperties titleRunProps = new RunProperties();
                    titleRunProps.FontSize = new FontSize() { Val = "32" };
                    titleParagraph.GetFirstChild<Run>().PrependChild(titleRunProps);

                    body.Append(titleParagraph);

                    body.Append(new Paragraph(new Run(new Text(""))));

                    Table table = new Table();

                    TableProperties tblProps = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = BorderValues.Single, Size = 6 },
                            new BottomBorder { Val = BorderValues.Single, Size = 6 },
                            new LeftBorder { Val = BorderValues.Single, Size = 6 },
                            new RightBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                        )
                    );

                    table.AppendChild(tblProps);

                    TableRow headerRow = new TableRow();

                    foreach (DataColumn col in dt.Columns)
                    {
                        TableCell cell = new TableCell(
                            new Paragraph(
                                new Run(
                                    new RunProperties(),
                                    new Text(col.ColumnName)
                                )
                            )
                        );

                        headerRow.Append(cell);
                    }

                    table.Append(headerRow);

                    foreach (DataRow row in dt.Rows)
                    {
                        TableRow dataRow = new TableRow();

                        foreach (DataColumn col in dt.Columns)
                        {
                            string text = row[col]?.ToString() ?? "";

                            TableCell cell = new TableCell(
                                new Paragraph(
                                    new Run(new Text(text))
                                )
                            );

                            dataRow.Append(cell);
                        }

                        table.Append(dataRow);
                    }

                    body.Append(table);

                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }

                System.Windows.MessageBox.Show(
                    "Datele au fost exportate cu succes în Word.",
                    "Succes",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "A apărut o eroare în timpul exportării în Word:\n" + ex.Message,
                "Eroare",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
        public void ExportFullExcel()
        {
            try
            {
                DataBase db = new DataBase();

                DataTable dtLocalitati = db.SelectData("Localitati");
                DataTable dtBeneficiari = db.SelectData("Beneficiari");

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                    FileName = "Export_Complet_Baza_Date.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dtLocalitati, "Localitati");
                        wb.Worksheets.Add(dtBeneficiari, "Beneficiari");

                        wb.SaveAs(saveFileDialog.FileName);
                    }
                    System.Windows.MessageBox.Show("Exportul complet în Excel a fost realizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A apărut o eroare la exportul complet în Excel:\n" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void ExportFullWord()
        {
            try
            {
                DataBase db = new DataBase();

                var tablesToExport = new Dictionary<string, DataTable>
                {
                    { "Localitati", db.SelectData("Localitati") },
                    { "Beneficiari", db.SelectData("Beneficiari") }
                };

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Word Document (*.docx)|*.docx",
                    FileName = "Export_Complet_Baza_Date.docx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = new Body();

                        Paragraph mainTitle = new Paragraph(new Run(new Text("Raport Complet Gestiune")));
                        RunProperties mainTitleProps = new RunProperties { FontSize = new FontSize() { Val = "40" }};
                        mainTitle.GetFirstChild<Run>().PrependChild(mainTitleProps);
                        body.Append(mainTitle);
                        body.Append(new Paragraph(new Run(new Text("")))); 

                        foreach (var kvp in tablesToExport)
                        {
                            string currentTableName = kvp.Key;
                            DataTable dt = kvp.Value;

                            Paragraph sectionTitle = new Paragraph(new Run(new Text($"Tabel: {currentTableName}")));
                            RunProperties sectionProps = new RunProperties { FontSize = new FontSize() { Val = "28" } };
                            sectionTitle.GetFirstChild<Run>().PrependChild(sectionProps);
                            body.Append(sectionTitle);

                            DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                            TableProperties tblProps = new TableProperties(
                                new TableBorders(
                                    new TopBorder { Val = BorderValues.Single, Size = 6 },
                                    new BottomBorder { Val = BorderValues.Single, Size = 6 },
                                    new LeftBorder { Val = BorderValues.Single, Size = 6 },
                                    new RightBorder { Val = BorderValues.Single, Size = 6 },
                                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                                )
                            );
                            table.AppendChild(tblProps);

                            TableRow headerRow = new TableRow();
                            foreach (DataColumn col in dt.Columns)
                            {
                                headerRow.Append(new TableCell(new Paragraph(new Run(new Text(col.ColumnName)))));
                            }
                            table.Append(headerRow);

                            foreach (DataRow row in dt.Rows)
                            {
                                TableRow dataRow = new TableRow();
                                foreach (DataColumn col in dt.Columns)
                                {
                                    string text = row[col]?.ToString() ?? "";
                                    dataRow.Append(new TableCell(new Paragraph(new Run(new Text(text)))));
                                }
                                table.Append(dataRow);
                            }

                            body.Append(table);

                            body.Append(new Paragraph(new Run(new Text(""))));
                            body.Append(new Paragraph(new Run(new Text(""))));
                        }

                        mainPart.Document.Append(body);
                        mainPart.Document.Save();
                    }

                    System.Windows.MessageBox.Show("Exportul complet în Word a fost realizat cu succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A apărut o eroare în timpul exportării complete în Word:\n" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
