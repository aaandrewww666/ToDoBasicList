using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ToDoBasicList.Core.Models;

namespace ToDoBasicList.Core.Contracts
{
    /// <summary>
    /// Persists the user task list between application runs.
    /// </summary>
    public interface ITaskStorageService
    {
        /// <summary>
        /// Loads previously saved tasks. On success returns the stored tasks
        /// (empty when nothing was saved yet); on failure carries the error message.
        /// </summary>
        Task<Result<IReadOnlyList<TaskDto>>> LoadAsync();

        /// <summary>
        /// Persists the current set of tasks, overwriting any previous data.
        /// Returns a failed <see cref="Result"/> when the data could not be written.
        /// </summary>
        Task<Result> SaveAsync(IEnumerable<TaskDto> tasks);
    }
}
