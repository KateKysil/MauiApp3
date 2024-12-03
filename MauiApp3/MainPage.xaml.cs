﻿using Microsoft.Maui.Animations;
using Microsoft.Maui.Storage;
using OfficeOpenXml;
using Maui.Calc;
using Calculator = Maui.Calc.Calculator;
using System.Linq.Expressions;
using Saver;
namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {
        int columns = 15;
        int rows = 15;
        public MainPage()
        {
            InitializeComponent();
            CreateTable(rows, columns);
        }

        string currCell = "";
        string currCellExpr = "";
        Entry currEntry = new Entry();
        private void CreateTable(int _rowCount, int _columnCount)
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();
            for (int i = 0; i <= _rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            for (int j = 0; j <= _columnCount; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            for (int i = 0; i <= _rowCount; i++)
            {
                for (int j = 0; j <= _columnCount; j++)
                {
                    if (i == 0 && j == 0) continue;

                    if (i == 0)
                    {
                        var label = new Label
                        {
                            Text = GetColumnName(j),
                            HorizontalTextAlignment = TextAlignment.Center
                        };
                        Grid.SetRow(label, i);
                        Grid.SetColumn(label, j);
                        grid.Children.Add(label);
                    }
                    else if (j == 0) 
                    {
                        var label = new Label
                        {
                            Text = i.ToString(),
                            HorizontalTextAlignment = TextAlignment.Center
                        };
                        Grid.SetRow(label, i);
                        Grid.SetColumn(label, j);
                        grid.Children.Add(label);
                    }
                    else 
                    {
                        var entry = new Entry
                        {
                            AutomationId = $"{GetColumnName(j)}{i}",
                            Placeholder = "",
                            Text = Calculator.sheet.Cells.TryGetValue($"{GetColumnName(j)}{i}", out var cell) ? cell.Value : ""
                        };
                        entry.Unfocused += Entry_Unfocused;
                        entry.Focused += Entry_Focused;
                        Grid.SetRow(entry, i);
                        Grid.SetColumn(entry, j);
                        grid.Children.Add(entry);
                    }
                }
            }
        }

        private async void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                currCell = entry.AutomationId;
                currEntry = entry;
                if (Calculator.sheet.Cells.TryGetValue(currCell, out var cell))
                {
                    textInput.Text = cell.Expression;
                    entry.Text = cell.Expression;
                }
                else
                {
                    textInput.Text = string.Empty;
                }
            }
        }
        private string GetColumnName(int index)
        {
            string columnName = "";
            while (index > 0)
            {
                columnName = (char)('A' + (index - 1) % 26) + columnName;
                index = (index - 1) / 26;
            }
            return columnName;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                currCell = entry.AutomationId;
                currCellExpr = entry.Text;
                currEntry = entry;
                if (Calculator.sheet.Cells.TryGetValue(currCell, out var cell))
                {
                    entry.Text = cell.Value;
                    if(cell.Value == "" && cell.Expression != "")
                    {
                        Calculator.sheet.Cells[currCell].Expression = "";
                        Calculator.sheet.Cells[currCell].Value = "";
                        Calculator.sheet.Cells[currCell].linkInCell = new List<string>();
                        Calculator.sheet.Cells[currCell].linkedIn = new List<string>();
                    }
                }
                else
                {
                    textInput.Text = string.Empty;
                    
                }
            }
        }
        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool isNewCell = !Calculator.sheet.Cells.ContainsKey(currCell); 
                bool isExpressionChanged = isNewCell ||
                                           Calculator.sheet.Cells[currCell].Expression != currCellExpr;

                if (isExpressionChanged)
                {
                    Calculator.sheet.EditCell(currCell, currCellExpr);
                }
                RecalculateDependencies(currCell);
                currEntry.Text = Calculator.sheet.Cells[currCell].Value;
            }
            catch
            {
                MarkAsError(currCell);
            }
        }
        private void RecalculateDependencies(string cellName)
        {
            if (!Calculator.sheet.Cells.ContainsKey(cellName)) return;

            var visitedCells = new HashSet<string>();
            var stack = new Stack<string>();
            stack.Push(cellName);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visitedCells.Add(current)) continue;

                try
                {
                    Calculator.sheet.RefreshRecursively(current);
                    UpdateEntryText(current);
                }
                catch
                {
                    MarkAsError(current);
                }
                foreach (var dependent in Calculator.sheet.Cells[current].linkedIn)
                {
                    stack.Push(dependent);
                }
            }
        }

        private void MarkAsError(string cellName)
        {
            if (!Calculator.sheet.Cells.ContainsKey(cellName)) return;

            var visitedCells = new HashSet<string>();
            var stack = new Stack<string>();
            stack.Push(cellName);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visitedCells.Add(current)) continue;

                if (Calculator.sheet.Cells.TryGetValue(current, out var cell))
                {
                    cell.Value = string.Empty;
                    UpdateEntryText(current, "ERROR");
                }
                foreach (var dependent in Calculator.sheet.Cells[current].linkedIn)
                {
                    stack.Push(dependent);
                }
            }
        }

        private void UpdateEntryText(string cellName, string textOverride = null)
        {
            var entry = grid.Children.OfType<Entry>().FirstOrDefault(e => e.AutomationId == cellName);
            if (entry != null)
            {
                entry.Text = textOverride ?? Calculator.sheet.Cells[cellName].Value;
            }
        }
        private void AddRowButton_Clicked(object sender, EventArgs e)
        {
            int newRowIndex = grid.RowDefinitions.Count;
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var rowHeader = new Label
            {
                Text = $"{newRowIndex}",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(2)
            };
            rows++;
            grid.Children.Add(rowHeader);
            Grid.SetRow(rowHeader, newRowIndex);
            Grid.SetColumn(rowHeader, 0);
            for (int column = 1; column < grid.ColumnDefinitions.Count; column++)
            {
                var entry = new Entry
                {
                    Placeholder = "",
                    Margin = new Thickness(2)
                };
                grid.Children.Add(entry);
                Grid.SetRow(entry, newRowIndex);
                Grid.SetColumn(entry, column);
            }
        }
        private void AddColumnButton_Clicked(object sender, EventArgs e)
        {
            int newColumnIndex = grid.ColumnDefinitions.Count;
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            string columnName = GetColumnName(newColumnIndex);
            var columnHeader = new Label
            {
                Text = columnName,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(2)
            };
            columns++;
            grid.Children.Add(columnHeader);
            Grid.SetRow(columnHeader, 0);
            Grid.SetColumn(columnHeader, newColumnIndex);
            for (int row = 1; row < grid.RowDefinitions.Count; row++)
            {
                var entry = new Entry
                {
                    Placeholder = "",
                    Margin = new Thickness(2)
                };
                grid.Children.Add(entry);
                Grid.SetRow(entry, row);
                Grid.SetColumn(entry, newColumnIndex);
            }
        }
        private void DeleteRowButton_Clicked(object sender, EventArgs e)
        {
            if (grid.RowDefinitions.Count > 3)
            {
                int lastRowIndex = grid.RowDefinitions.Count - 1;
                bool allCellsEmpty = true;
                
                for (int column = 1; column < grid.ColumnDefinitions.Count; column++)
                {
                    var entry = grid.Children
                        .OfType<Entry>()
                        .FirstOrDefault(e => Grid.GetRow(e) == lastRowIndex && Grid.GetColumn(e) == column);

                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Text))
                    {
                        allCellsEmpty = false;
                        break;
                    }
                }
                if (allCellsEmpty)
                {
                    rows--;
                    grid.RowDefinitions.RemoveAt(lastRowIndex);
                    for (int i = grid.Children.Count - 1; i >= 0; i--)
                    {
                        var child = grid.Children[i];
                        if (grid.GetRow(child) == lastRowIndex)
                        {
                            grid.Children.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    DisplayAlert("Error", "Cannot delete the row because it contains non-empty cells.", "OK");
                }
            }
        }
        private void DeleteColumnButton_Clicked(object sender, EventArgs e)
        {
            if (grid.ColumnDefinitions.Count > 3)
            {
                int lastColumnIndex = grid.ColumnDefinitions.Count - 1;
                bool allCellsEmpty = true;
                
                for (int row = 1; row < grid.RowDefinitions.Count; row++)
                {
                    var entry = grid.Children
                        .OfType<Entry>()
                        .FirstOrDefault(e => Grid.GetColumn(e) == lastColumnIndex && Grid.GetRow(e) == row);

                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Text))
                    {
                        allCellsEmpty = false;
                        break;
                    }
                }
                if (allCellsEmpty)
                {
                    columns--;
                    grid.ColumnDefinitions.RemoveAt(lastColumnIndex);
                    for (int i = grid.Children.Count - 1; i >= 0; i--)
                    {
                        var child = grid.Children[i];
                        if (grid.GetColumn(child) == lastColumnIndex)
                        {
                            grid.Children.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    DisplayAlert("Error", "Cannot delete the column because it contains non-empty cells.", "OK");
                }
            }
        }
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Confirm Action","Are you sure you want to proceed?","OK","Cancel");
            if (!result)
                return;
            var googleDriveSaver = new GoogleDriveSaver();
            await googleDriveSaver.SaveToGoogleDriveAsync(Calculator.sheet.Cells, rows, columns);
            await DisplayAlert("Success", $"File saved to Google Drive", "OK");
        }
        private async void ReadButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var googleDriveService = new GoogleDriveService();
                var files = await googleDriveService.ListFilesAsync();

                if (files.Count == 0)
                {
                    await DisplayAlert("Error", "No files found on Google Drive.", "OK");
                    return;
                }
                var fileNames = files.Select(f => f.Name).ToArray();
                string selectedFileName = await DisplayActionSheet("Select a file to load:", "Cancel", null, fileNames);
                if (selectedFileName == "Cancel")
                    return;
                var selectedFile = files.FirstOrDefault(f => f.Name == selectedFileName);
                if (selectedFile == null)
                {
                    await DisplayAlert("Error", "File not found.", "OK");
                    return;
                }
                string xmlContent = await googleDriveService.DownloadFileAsync(selectedFile.Id);
                var xmlSaver = new XmlSaver();
                var (cells, rows, columns) = xmlSaver.ParseContent(xmlContent);
                Calculator.sheet.Cells.Clear();
                foreach (var cell in cells)
                {
                    Calculator.sheet.Cells[cell.Key] = cell.Value;
                }
                this.rows = rows;
                this.columns = columns;
                CreateTable(rows, columns);

                await DisplayAlert("Success", "File loaded successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Підтвердження", "Ви дійсно хочете вийти?", "Так", "Ні");
            if (answer)
            {
                System.Environment.Exit(0);
            }
        }
        private async void HelpButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Довідка", "Лабораторна робота 1*. Виконавиця - Кисіль Катерина, К25", "OK");
        }
        
        
    }

}
