#region

using Microsoft.AspNetCore.Components;
using OmegaExplorer.Toolkit.API;
using System.Reflection;

#endregion

namespace OmegaExplorer.Client.Models.Card;

public class CardDefinition
{
    public string Title { get; set; } = "Title";
    public string Subtitle { get; set; } = "Subtitle";

    /// <summary>
    /// Main text of the card
    /// </summary>
    public string Text { get; set; }

    public List<RenderFragment> Fragments { get; set; } = new();

    /// <summary>
    /// Raw object boxing
    /// </summary>
    public object? Value { get; set; }

    public string ImageHeader { get; set; }

    public ResponseUser? Owner { get; set; }

    /// <summary>
    ///     If card encounter an error, this property will be set
    /// </summary>
    public Exception? Exception { get; set; } = null;
    public EnumImageMode ImageMode { get; set; } = EnumImageMode.Img;

    public enum EnumImageMode
    {
        Img,
        Lottie
    }

    public Progress? Progress { get; set; }

    public EnumRarity? Rarity { get; set; } = null;

    public bool SubtitleMultiline { get; set; } = false;

    public List<CardContent> Contents { get; set; } = new();
    public List<CardAction> Actions { get; set; } = new();

    public CardSettings Settings { get; set; } = new();

    /// <summary>
    /// TRUE if need to refresh
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> InitializeAsync(bool needRefresh = false)
    {
        return await Task.FromResult(needRefresh);
    }

    /// <summary>
    /// Call an action from the card
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    /// <param name="parameters"></param>
    public void CallMethod(string name, object? obj = null, Components.Card.Card[] parameters = null!)
    {
        Type type = GetType();

        MethodInfo? myMethod = type.GetMethod(name);

        if (myMethod != null)
        {
            try
            {
                myMethod.Invoke(this, parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        else
        {
            Console.WriteLine("Method not found " + name);
        }
    }

    public void SetLocked(string reason)
    {
        Settings.IsLocked = true;
        Settings.LockedReason = reason;
    }
}
