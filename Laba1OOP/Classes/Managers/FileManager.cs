using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Windows;
using Microsoft.Win32;

namespace Laba1OOP.Classes.Managers
{
    public class FileManager<T>
    {
        private List<T> items;

        public FileManager(List<T> items)
        {
            this.items = items;
        }

        public void SaveElementsToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json",
                Title = "Сохранить фигуры"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        Formatting = Formatting.Indented,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    string json = JsonConvert.SerializeObject(items, settings);
                    File.WriteAllText(saveFileDialog.FileName, json);

                    MessageBox.Show("Фигуры успешно сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении в файл: {ex.Message}",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        public void LoadElementsFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json",
                Title = "Загрузить фигуры"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog.FileName);

                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    var loadedItems = JsonConvert.DeserializeObject<List<T>>(json, settings);

                    if (loadedItems != null)
                    {
                        items.Clear();
                        items.AddRange(loadedItems);
                        MessageBox.Show("Фигуры успешно загружены!", "Загрузка", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Файл пустой или содержит некорректные данные.", "Загрузка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при загрузке из файла: {ex.Message}",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

    }
}
