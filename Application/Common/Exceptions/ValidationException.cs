using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (IGrouping<string, string> failureGroup in failures.GroupBy(x => x.PropertyName, x => x.ErrorMessage))
            {
                string propertyName = failureGroup.Key;
                string[] propertyFailures = failureGroup.ToArray();
                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }
    }
}
