using DragonDrop.DragAndDrop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonDrop.Lib {
  public class DnDContainerBase<TListItem, TItem, TGroup> : ComponentBase {
    public DnDContainerBase() {

    }
    [Parameter] public List<TListItem> List { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public bool ShowTitle { get; set; }
    [Parameter] public bool AllowDropToGroup { get; set; }
    [Parameter] public bool ShowGlobalAddNew { get; set; } = true;
    [Parameter] public string ChildrenPropertyName { get; set; }
    [Parameter] public SeparatorDisplay SeparatorDisplay { get; set; } = SeparatorDisplay.None;
    [Parameter] public bool ShowDebugInfo { get; set; }
    [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
    [Parameter] public RenderFragment<TItem> EditItemTemplate { get; set; }
    [Parameter] public RenderFragment<TListItem> SeparatorItemTemplate { get; set; }
    [Parameter] public EventCallback<ListUpdateEventArgs> OnStatusUpdated { get; set; }
    [Parameter] public EventCallback<TListItem> OnItemAdded { get; set; }
    [Parameter] public DnDContainer<TListItem, TItem, TGroup> Parent { get; set; }
    [Parameter] public string Name { get; set; }
    [Parameter] public string Address { get; set; }
    [CascadingParameter] protected DnDState<TListItem, TItem, TGroup> DnDState { get; set; }

    public async Task SetPayload(DnDContainer<TListItem, TItem, TGroup> fromContainer, int index, string address = null, bool isContainer = false) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Setting payload to {(string.IsNullOrWhiteSpace(address) ? "{empty}" : address)}"); }
      await DnDState.SetPayload(fromContainer, index, address, isContainer);
    }

    public DnDPayload<TListItem, TItem, TGroup> GetPayload() {
      return DnDState.Payload;
    }

    protected async Task HandleDragStart(DragEventArgs e, int index, DnDContainer<TListItem, TItem, TGroup> fromContainer, string address = null, bool isContainer = false) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drag started for {address}"); }
      if (index >= fromContainer.List.Count()) { Console.WriteLine("Item index was out of range"); return; }
      // Pause before setting the payload to allow the Drag event to register in the browser for the default action of picking up the item.
      await Task.Delay(20);
      await SetPayload(fromContainer, index, address, isContainer);
      DnDState.ShowDropAtEnd = true;
    }

    protected async Task HandleDragEnd(DragEventArgs e) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drag ended for {Address}"); }
      await SetPayload(null, -1);
      await SetPayload(null, -1);
      DnDState.ShowDropAtEnd = false;
      DnDState.DropCssStylesDict.Clear();
      DnDState.ShowGroupWithDict.Clear();
    }

    protected async Task HandleMove(DragEventArgs e, int index, bool isDropOnExisting = false, bool dropIntoFirst = false) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Handling Move (Drop)"); }
      DnDState.ShowDropAtEnd = false;
      if (DnDState.BaseContainer is null) { Console.Error.WriteLine("Could not retrieve the Base Container information."); return; }
      index = Math.Max(0, Math.Min(index, List.Count()));

      var pl = DnDState.Payload;
      if (string.IsNullOrWhiteSpace(pl.Address)) {
        // Drop not allowed when there is no payload (the address is empty)
        Console.Error.WriteLine("Could not retrieve the payload information.");
        return;
      } else if (Address.StartsWith(pl.Address) && pl.IsContainer) {
        // when the payload is a DnDContainer and it is being dropped as a descendant of itself
        Console.WriteLine("A container cannot be dropped as a child of itself.");
        return;
      }

      if (pl.FromContainer is null || pl.FromIndex < 0) { Console.Error.WriteLine("No item to drop"); return; }
      if (pl.FromIndex >= pl.FromContainer.List.Count()) { Console.Error.WriteLine("Index of the item being dragged was out of range"); return; }

      var itemToMove = pl.FromContainer.List[pl.FromIndex];
      // If dropping on empty, index out of range possibly
      var myItemAtIndex = List.Count() > 0 && index < List.Count() ? List[index] : default(TListItem);
      var childProp = string.IsNullOrWhiteSpace(DnDState.ChildrenPropertyName) ? null : myItemAtIndex?.GetType().GetProperty(DnDState.ChildrenPropertyName);
      if (pl.FromContainer == this && pl.FromIndex == index) {
        if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine("The item was dropped onto itself."); }
        return;
      }

      if (DnDState.ShowDebugInfo || overrideDebug) {
        Console.WriteLine($"itemToMove's address: {pl.Address}");
        Console.WriteLine($"itemToMove's container's address: {pl.FromContainer.Address}");
        Console.WriteLine($"myItemAtIndex's address: {Address}.{index}");
        Console.WriteLine($"this's address: {Address}");
      }

      bool makeNewGroup = // The parameter to allow grouping when dropping on existing items is set to True AND
                          AllowDropToGroup &&
                          // The picked up item is being dropped on an existing item AND
                          isDropOnExisting &&
                          (
                            // The item being dropped is a container and the item at the drop target index is not an ancestor of the item being dropped OR
                            (pl.IsContainer && !pl.FromContainer.Address.StartsWith($"{Address}.{index}")) ||
                            // The item being dropped is not a container and the item at the drop target index is not a container either
                            (!pl.IsContainer && childProp is null)
                          );

      var updateEventArgs = new ListUpdateEventArgs();
      // TODO: Fill in the appropriate fields for the updateEventArgs
      if (makeNewGroup) {
        if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Making a new group"); }

        // TODO: Add an argument to determine if this is a drop before/after
        var newGroup = Activator.CreateInstance<TGroup>();
        var children = new List<TListItem>();
        children.Add(dropIntoFirst ? itemToMove : myItemAtIndex);
        children.Add(dropIntoFirst ? myItemAtIndex : itemToMove);
        childProp = typeof(TGroup).GetProperty(DnDState.ChildrenPropertyName);
        childProp.SetValue(newGroup, children);
        List.RemoveAt(index);
        List.Insert(index, (TListItem)(object)newGroup);
      } else {
        List.Insert(index, itemToMove);
      }
      var removeIndex = pl.FromContainer == this && !makeNewGroup ? pl.FromIndex + (pl.FromIndex > index && !makeNewGroup ? 1 : 0) : pl.FromIndex;
      pl.FromContainer.List.RemoveAt(removeIndex);

      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine("Removing drop css styles"); }
      DnDState.DropCssStylesDict.Clear();
      DnDState.ShowGroupWithDict.Clear();

      // ================> Test for ungroup <================
      var containerToCheck = pl.FromContainer;
      // Test the fromContainer to see if there is <= 1 element and is not the DnDState.BaseContainer and has a Parent container
      if (containerToCheck.List.Count() <= 1 && containerToCheck != DnDState.BaseContainer && containerToCheck.Parent != null) {
        if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"There {(containerToCheck.List.Count() == 0 ? "are 0 items" : "is 1 item")} in the container, so it should be ungrouped."); }
        // Determine the index within the parent container's list using the provided container's Address
        if (!int.TryParse(containerToCheck.Address.Split('.').Last(), out int indexInParentContainer)) {
          Console.Error.WriteLine("Could not determine the container's index in the parent container.");
        } else {
          var parent = containerToCheck.Parent;
          // Add 1 to the index if the item being moved is being dropped into its previous parent's parent at an index before or equal to its previous parent's index
          if (this == containerToCheck.Parent && index <= indexInParentContainer) {
            indexInParentContainer += 1;
          }

          CheckForUngroup(containerToCheck, indexInParentContainer);
        }
      }

      await SetPayload(null, -1);
      await DnDState.BaseContainer.OnStatusUpdated.InvokeAsync(updateEventArgs);
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drop method complete"); }
    }

    protected async Task HandleDropTargetDragEnter(DragEventArgs e, int index, bool isBeforeTarget = false, bool checkForGroup = false) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drop target (Drop {(isBeforeTarget ? "Before" : "After")}) drag entered for {Address}"); }
      DnDState.IsDropBefore = isBeforeTarget;
      var pl = DnDState.Payload;

      var itemAtIndexIsContainer = List[index].GetType().IsAssignableFrom(typeof(TGroup));
      var addressForItem = AddressForItem(index);
      if (checkForGroup) {
        var canGroupWith = checkForGroup && !string.IsNullOrWhiteSpace(pl.Address) && AllowDropToGroup && pl.Address != addressForItem && (pl.IsContainer || (!pl.IsContainer && !itemAtIndexIsContainer));
        await DnDState.SetShowGroupWith(addressForItem, canGroupWith);
      } else {
        DnDState.ShowGroupWithDict.Clear();
        await DnDState.SetDropCssStyles(addressForItem, $"{(string.IsNullOrWhiteSpace(pl.Address) || Address.StartsWith(pl.Address) ? $"no-drop{(pl.Address == Address ? " drop-hovering" : "")}" : $"can-drop drop-hovering")}");
      }
    }

    protected async Task HandleDropTargetDragLeave(DragEventArgs e, int index) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drop target drag left for {Address}"); }
      var addressForItem = AddressForItem(index);
      DnDState.ShowGroupDropFirst = false;
      DnDState.ShowGroupDropSecond = false;
      await DnDState.SetShowGroupWith(addressForItem, false);
      await DnDState.SetDropCssStyles(addressForItem, null);

    }

    protected void HandleDragOverGroup(MouseEventArgs e, bool groupFirst = false, bool groupSecond = false) {
      DnDState.ShowGroupDropFirst = groupFirst;
      DnDState.ShowGroupDropSecond = groupSecond;
      StateHasChanged();
    }

    protected void AddItem(MouseEventArgs e, int index) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Adding item for {Address}"); }

      if (DnDState.NewItem == null) {
        Console.Error.WriteLine("Failed to add the new item.");
      } else {
        if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Hiding the Add New item for index: {index}, which is currently {DnDState.ShowNewItemDict[AddressForItem(index)]}"); }
        List.Insert(index, (TListItem)(object)DnDState.NewItem);
        DnDState.ShowNewItemDict.Clear();
        DnDState.NewItem = Activator.CreateInstance<TItem>();
      }
    }

    protected void CancelAdd(MouseEventArgs e, int index) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Toggling show new item for {Address}"); }
      DnDState.ShowNewItemDict.Clear();
    }

    protected void OnSaveComplete(DnDItem<TListItem, TItem, TGroup> itemComponent) {
      if (!int.TryParse(itemComponent.Address.Split('.').Last(), out var itemIndex)) {
        Console.Error.WriteLine("Could not determine the index for the new item.  Setting it to the end of the list.");
        itemIndex = List.Count() - 1;
      }
      DnDState.NewItem = Activator.CreateInstance<TItem>();

      if (itemComponent.IsAdd) {
        DnDState.NewItem = Activator.CreateInstance<TItem>();
        DnDState.ShowNewItemDict.Clear();
      }
    }

    protected void OnCancelComplete(DnDItem<TListItem, TItem, TGroup> itemComponent) {
      if (!int.TryParse(itemComponent.Address.Split('.').Last(), out var itemIndex)) {
        Console.Error.WriteLine("Could not determine the index of the new item being cancelled.");
        return;
      }
      if (itemComponent.IsAdd) {
        DnDState.ShowNewItemDict.Clear();
      } else {
        // TODO: Determine if anything needs to be done
      }
    }

    public async Task NotifyStateChanged(ListUpdateEventArgs e = null) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine("Notifying state has changed"); }

      if (DnDState.BaseContainer != null) {
        DnDState.BaseContainer.StateHasChanged();
        if (DnDState.BaseContainer.OnStatusUpdated.HasDelegate && e != null) { await DnDState.BaseContainer.OnStatusUpdated.InvokeAsync(e); }
      } else {
        StateHasChanged();
        if (OnStatusUpdated.HasDelegate && e != null) { await OnStatusUpdated.InvokeAsync(e); }
      }
    }

    protected async Task ToggleShowNewItem(MouseEventArgs e, int index, bool addBefore = false) {
      var overrideDebug = false;
      var addBeforeChanging = DnDState.IsAddBefore != addBefore;
      DnDState.IsAddBefore = addBefore;

      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Toggling show new item for {Address}"); }

      var addressOfItem = AddressForItem(index);
      var curShowNewItem = DnDState.GetShowNewItem(addressOfItem);
      await DnDState.SetShowNewItem(addressOfItem, (addBeforeChanging && curShowNewItem) || !curShowNewItem, true);
    }

    protected void HandleDelete(MouseEventArgs e) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Deleting for {Address}"); }
      if (Parent is null) { Console.Error.WriteLine("Parent is missing."); return; }
      // TODO: Reserved for handling deletion of a container
    }

    #region Helpers
    public void CheckForUngroup(DnDContainer<TListItem, TItem, TGroup> containerToCheck = null, int indexInParentContainer = -1) {
      var overrideDebug = false;
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Checking for ungroup for {Address}"); }

      if (containerToCheck is null) { containerToCheck = (DnDContainer<TListItem, TItem, TGroup>)this; }
      if (containerToCheck == DnDState.BaseContainer) {
        if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine("The container being checked for ungroup is the Base Container and will not be ungrouped."); }
        return;
      }

      if (containerToCheck.List.Count() > 1) {
        if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine("The container has more than 1 element and should not be ungrouped."); }
        return;
      }

      if (DnDState.ShowDebugInfo || overrideDebug) {
        Console.WriteLine($"Passed in indexInParentContainer: {indexInParentContainer}");
        Console.WriteLine($"containerToCheck's address: {containerToCheck.Address}");
      }

      if (indexInParentContainer < 0 && !int.TryParse(containerToCheck.Address.Split('.').Last(), out indexInParentContainer)) {
        Console.Error.WriteLine("Could not determine the container's index in the parent container during ungroup.");
      }
      var parent = containerToCheck.Parent;
      if (parent is null) {
        Console.Error.WriteLine("The parent container could not be determined.");
        return;
      }
      // Remove the item from the group and place it at the group's original index
      if (DnDState.ShowDebugInfo || overrideDebug) { Console.WriteLine($"Removing item at index {indexInParentContainer} from {parent.Address} due to ungroup"); }
      parent.List.RemoveAt(indexInParentContainer);
      if (containerToCheck.List.Count() == 1) {
        var item = containerToCheck.List[0];
        parent.List.Insert(indexInParentContainer, item);
      }
    }

    string AddressForItem(int index) {
      return $"{Address}.{index}";
    }
    #endregion
  }
}
