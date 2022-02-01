

using FluentValidation;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using val = Ordering.Application.Exception;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TRespose> : IPipelineBehavior<TRequest, TRespose> 
        where TRequest : IRequest<TRespose>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<TRespose> Handle(TRequest request, CancellationToken cancellationToken, 
            RequestHandlerDelegate<TRespose> next)
        {
            if (_validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResult = await Task.WhenAll(_validator.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    throw new val.ValidationException(failures);
                }
            }
            return await next();
        }
    }
}
