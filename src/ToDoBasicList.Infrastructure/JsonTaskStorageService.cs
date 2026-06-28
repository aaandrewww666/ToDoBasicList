using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ToDoBasicList.Core.Contracts;
using ToDoBasicList.Core.Models;

namespace ToDoBasicList.Infrastructure
{
    /// <summary>
    /// Stores tasks as a JSON file under the per-user application data folder
    /// (%AppData% on Windows, ~/.config on Linux), so they survive restarts.
    /// </summary>
    public sealed class JsonTaskStorageService : ITaskStorageService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

        private readonly string _filePath;

        public JsonTaskStorageService()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ToDoBasicList");
            _filePath = Path.Combine(directory, "tasks.json");
        }

        public async Task<Result<IReadOnlyList<TaskDto>>> LoadAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return Result.Success<IReadOnlyList<TaskDto>>([]);

                await using var stream = File.OpenRead(_filePath);
                var tasks = await JsonSerializer.DeserializeAsync<List<TaskDto>>(stream) ?? [];
                return Result.Success<IReadOnlyList<TaskDto>>(tasks);
            }
            catch (Exception ex)
            {
                return Result.Failure<IReadOnlyList<TaskDto>>($"Failed to load tasks from '{_filePath}': {ex.Message}");
            }
        }

        public async Task<Result> SaveAsync(IEnumerable<TaskDto> tasks)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

                await using var stream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(stream, tasks, SerializerOptions);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to save tasks to '{_filePath}': {ex.Message}");
            }
        }
    }
}
