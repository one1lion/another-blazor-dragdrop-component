using DragonDrop.DragAndDrop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonDrop.Lib {
  public class DnDState<TListItem, TItem, TGroup> {
    public DnDState() {
      Id = Guid.NewGuid().ToString();
    }
    public string Id { get; }

    public List<TListItem> List { get; set; }

    public DnDContainer<TListItem, TItem, TGroup> BaseContainer { get; set; }
    public TItem NewItem { get; set; } = Activator.CreateInstance<TItem>();
    public bool ShowDropAtEnd { get; set; }
    public bool IsDropBefore { get; set; }
    public bool IsAddBefore { get; set; }
    public bool ShowGlobalAddNew { get; set; }
    public string ChildrenPropertyName { get; set; }
    public bool ShowTitle { get; set; }
    public bool AllowDropToGroup { get; set; }
    public SeparatorDisplay SeparatorDisplay { get; set; } = SeparatorDisplay.None;
    public Dictionary<string, string> DropCssStylesDict { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, bool> ShowGroupWithDict { get; set; } = new Dictionary<string, bool>();
    public Dictionary<string, bool> ShowNewItemDict { get; set; } = new Dictionary<string, bool>();

    public DnDPayload<TListItem, TItem, TGroup> Payload { get; set; } = new DnDPayload<TListItem, TItem, TGroup>();

    public bool ShowGroupDropFirst { get; set; }
    public bool ShowGroupDropSecond { get; set; }
    public bool ShowDebugInfo { get; set; }

    public async Task SetPayload(DnDContainer<TListItem, TItem, TGroup> fromContainer, int index, string address = null, bool isContainer = false) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Setting payload to {(string.IsNullOrWhiteSpace(address) ? "{empty}" : address)}"); }
      Payload.FromContainer = fromContainer;
      Payload.FromIndex = index;
      Payload.Address = address;
      Payload.IsContainer = isContainer;
      await BaseContainer.NotifyStateChanged();
    }

    public async Task ClearPayload() {
      Payload.FromContainer = null;
      Payload.FromIndex = -1;
      Payload.Address = string.Empty;
      Payload.IsContainer = false;
      await BaseContainer.NotifyStateChanged();
    }

    public async Task SetDropCssStyles(string targetAddress, string value, bool clearDictFirst = false) {
      if (clearDictFirst) { DropCssStylesDict.Clear(); }
      if (string.IsNullOrWhiteSpace(targetAddress)) { return; }
      if (!DropCssStylesDict.ContainsKey(targetAddress)) {
        DropCssStylesDict.Add(targetAddress, value);
      } else {
        DropCssStylesDict[targetAddress] = value;
      }
      await BaseContainer.NotifyStateChanged();
    }

    public async Task SetShowGroupWith(string targetAddress, bool value, bool clearDictFirst = false) {
      if (clearDictFirst) { ShowGroupWithDict.Clear(); }
      if (string.IsNullOrWhiteSpace(targetAddress)) { return; }
      if (!ShowGroupWithDict.ContainsKey(targetAddress)) {
        ShowGroupWithDict.Add(targetAddress, value);
      } else {
        ShowGroupWithDict[targetAddress] = value;
      }
      await BaseContainer.NotifyStateChanged();
    }

    public async Task SetShowNewItem(string targetAddress, bool value, bool clearDictFirst = false) {
      if (clearDictFirst) { ShowNewItemDict.Clear(); }
      if (string.IsNullOrWhiteSpace(targetAddress)) { return; }
      if (!ShowNewItemDict.ContainsKey(targetAddress)) {
        ShowNewItemDict.Add(targetAddress, value);
      } else {
        ShowNewItemDict[targetAddress] = value;
      }
      await BaseContainer.NotifyStateChanged();
    }

    public string GetDropCssStyle(string forAddress) {
      if (string.IsNullOrWhiteSpace(forAddress) || !DropCssStylesDict.ContainsKey(forAddress)) {
        return "";
      } else {
        return DropCssStylesDict[forAddress];
      }
    }

    public bool GetShowGroupWith(string forAddress) {
      if (string.IsNullOrWhiteSpace(forAddress) || !ShowGroupWithDict.ContainsKey(forAddress)) {
        return false;
      } else {
        return ShowGroupWithDict[forAddress];
      }
    }

    public bool GetShowNewItem(string forAddress) {
      if (string.IsNullOrWhiteSpace(forAddress) || !ShowNewItemDict.ContainsKey(forAddress)) {
        return false;
      } else {
        return ShowNewItemDict[forAddress];
      }
    }
  }
}
