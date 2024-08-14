using AutoMapper;
using BuildingBlocks.Abstractions.Queries;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Validation.Extensions;
using FluentValidation;
using FoodDelivery.Services.Identity.Shared.Exceptions;
using FoodDelivery.Services.Identity.Shared.Extensions;
using FoodDelivery.Services.Identity.Shared.Models;
using FoodDelivery.Services.Identity.Users.Dtos.v1;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Services.Identity.Users.Features.GettingUerByEmail.v1;

internal record GetUserByEmail(string Email) : IQuery<GetUserByEmailResult>
{
    /// <summary>
    /// GetUserByEmail with in-line validation.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static GetUserByEmail Of(string? email)
    {
        return new GetUserByIdValidator().HandleValidation(new GetUserByEmail(email!));
    }
}

internal class GetUserByIdValidator : AbstractValidator<GetUserByEmail>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email address is not valid");
    }
}

internal class GetUserByEmailHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    : IQueryHandler<GetUserByEmail, GetUserByEmailResult>
{
    public async Task<GetUserByEmailResult> Handle(GetUserByEmail query, CancellationToken cancellationToken)
    {
        query.NotBeNull();

        var identityUser = await userManager.FindUserWithRoleByEmailAsync(query.Email);
        identityUser.NotBeNull(new IdentityUserNotFoundException(query.Email));

        var userDto = mapper.Map<IdentityUserDto>(identityUser);

        return new GetUserByEmailResult(userDto);
    }
}

internal record GetUserByEmailResult(IdentityUserDto? UserIdentity);