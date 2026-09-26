using Bogus;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Personalities;

public class PersonalityService
{
    private readonly PersonalityRepository _personalityRepository;

    public PersonalityService(PersonalityRepository personalityRepository)
    {
        _personalityRepository = personalityRepository;
    }

    public async Task<Personality> Create(Guid ownerId)
    {
        Faker faker = new();

        var firstname = faker.Name.FirstName();
        var lastname = faker.Name.LastName();
        var portalIndex = faker.Random.Number(0, 6);
        var rarity = GetRandomRarity();

        Personality newPersonality = new(ownerId, firstname, lastname, portalIndex, rarity);

        await _personalityRepository.Add(newPersonality);

        return newPersonality;
    }

    private EnumRarity GetRandomRarity()
    {
        var rdm = Random.Shared.Next(0, 100);

        return rdm switch
        {
            > 98 => EnumRarity.Origin,
            > 95 => EnumRarity.Legendary,
            > 90 => EnumRarity.Epic,
            > 75 => EnumRarity.Rare,
            > 40 => EnumRarity.Uncommon,
            _ => EnumRarity.Common
        };
    }
}