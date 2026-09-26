using AutoMapper;
using OmegaExplorer.Server.Services._Game.Events;
using OmegaExplorer.Server.Services._Game.Events.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Contracts;
using OmegaExplorer.Server.Services._Game.GameResources;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapEventToResponseEvent : IMappingAction<Event, ResponseEvent>
{
    private readonly EventDataProvider _eventDataProvider;
    private readonly GameResourceDataProvider _gameResourceDataProvider;

    public AfterMapEventToResponseEvent(EventDataProvider eventDataProvider, GameResourceDataProvider gameResourceDataProvider)
    {
        _eventDataProvider = eventDataProvider;
        _gameResourceDataProvider = gameResourceDataProvider;
    }

    public void Process(Event src, ResponseEvent dest, ResolutionContext ctx)
    {
        var data = _eventDataProvider.GetByIndex(src.EventItemIndex);

        if (data == null)
        {
            return;
        }

        var dataMapped = ctx.Mapper.Map<ResponseDynamicItemEvent>(data);
        dest.Data = dataMapped;


        foreach (var choice in data.Choices)
        {
            var choiceMapped = ctx.Mapper.Map<ResponseDynamicItemEventChoice>(choice);

            //Map resource for choice
            foreach (var modifier in choiceMapped.Modifiers)
            {
                var dataGameResource = _gameResourceDataProvider.GetByIndex(modifier.Index);
                var dataGameResourceMapped = ctx.Mapper.Map<ResponseDynamicItemGameResource>(dataGameResource);

                modifier.Data = dataGameResourceMapped;
            }

            foreach (var oneTimeModifier in choiceMapped.OneTimeModifiers)
            {
                var dataGameResource = _gameResourceDataProvider.GetByIndex(oneTimeModifier.Index);
                var dataGameResourceMapped = ctx.Mapper.Map<ResponseDynamicItemGameResource>(dataGameResource);

                oneTimeModifier.Data = dataGameResourceMapped;
            }

            dataMapped.Choices.Add(choiceMapped);
        }
    }
}