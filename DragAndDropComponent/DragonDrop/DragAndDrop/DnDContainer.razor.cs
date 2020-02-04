using DragonDrop.DragAndDrop;
using DragonDrop.Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonDrop.DragAndDrop {
  public class DnDContainerBase<TListItem, TItem, TGroup> : ComponentBase {
    public DnDContainerBase() {

    }
    [Parameter] public List<TListItem> List { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public bool ShowTitle { get; set; }
    [Parameter] public bool AllowDropToGroup { get; set; }
    [Parameter] public bool ShowNewItem { get; set; } = true;
    [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
    [Parameter] public RenderFragment<TItem> EditItemTemplate { get; set; }
    [Parameter] public RenderFragment<TListItem> SeparatorItemTemplate { get; set; }
    [Parameter] public SeparatorDisplay SeparatorDisplay { get; set; } = SeparatorDisplay.None;
    [Parameter] public string ChildrenPropertyName { get; set; }
    [Parameter] public EventCallback<ListUpdateEventArgs> OnStatusUpdated { get; set; }
    [Parameter] public EventCallback<TListItem> OnItemAdded { get; set; }
    [Parameter] public DnDContainer<TListItem, TItem, TGroup> BaseContainer { get; set; }
    [Parameter] public DnDContainer<TListItem, TItem, TGroup> Parent { get; set; }
    [Parameter] public string Name { get; set; }
    [Parameter] public string Address { get; set; }
    [Parameter] public bool ShowDebugInfo { get; set; }

    protected TItem NewItem;
    protected string dropStyleCss = DropStyleCss.NotSet;
    protected bool showDropAtEnd;
    protected bool isDropBefore;
    protected bool isAddBefore;
    protected string[] dropCssStyles;
    protected bool[] showGroupWith;
    protected bool[] showNewItem;

    protected (DnDContainer<TListItem, TItem, TGroup> fromContainer, int fromIndex, string address, bool isContainer) payload = (default(DnDContainer<TListItem, TItem, TGroup>), -1, "", false);

    public async Task SetPayload(DnDContainer<TListItem, TItem, TGroup> fromContainer, int index, string address = null, bool isContainer = false) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Setting payload to {(string.IsNullOrWhiteSpace(address) ? "{empty}" : address)}"); }
      payload.fromContainer = fromContainer;
      payload.fromIndex = index;
      payload.address = address;
      payload.isContainer = isContainer;
      await InvokeAsync(() => StateHasChanged());
    }

    public (DnDContainer<TListItem, TItem, TGroup> fromContainer, int fromIndex, string address, bool isContainer) GetPayload() {
      return payload;
    }

    protected async Task HandleDragStart(DragEventArgs e, int index, DnDContainer<TListItem, TItem, TGroup> fromContainer, string address = null, bool isContainer = false) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drag started for {address}"); }
      if (index >= fromContainer.List.Count()) { Console.WriteLine("Item index was out of range"); return; }
      // Pause before setting the payload to allow the Drag event to register in the browser for the default action of picking up the item.
      await Task.Delay(20);
      await BaseContainer.SetPayload(fromContainer, index, address, isContainer);
      showDropAtEnd = true;
    }

    protected async Task HandleDragEnd(DragEventArgs e) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drag ended for {Address}"); }
      await BaseContainer.SetPayload(null, -1);
      showDropAtEnd = false;
    }

    protected void HandleDragEnter(DragEventArgs e) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drag entered for {Address}"); }
      if (BaseContainer is null) { return; }
      var pl = BaseContainer.GetPayload();
      if (Address.StartsWith(pl.address) && pl.isContainer) {
        dropStyleCss = DropStyleCss.NoDrop;
      } else {
        dropStyleCss = DropStyleCss.CanDrop;
      }
    }

    protected void HandleDragLeave(DragEventArgs e) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drag left for {Address}"); }
      dropStyleCss = DropStyleCss.NotSet;
    }

    protected async Task HandleMove(DragEventArgs e, int index, bool isDropOnExisting = false, bool dropIntoFirst = false) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Handling Move (Drop)"); }
      dropStyleCss = DropStyleCss.NotSet;
      showDropAtEnd = false;

      if (BaseContainer is null) { Console.Error.WriteLine("Could not retrieve the Base Container information."); return; }
      index = Math.Max(0, Math.Min(index, List.Count()));

      var pl = BaseContainer.GetPayload();
      if (string.IsNullOrWhiteSpace(pl.address)) {
        // Drop not allowed when there is no payload (the address is empty)
        Console.Error.WriteLine("Could not retrieve the payload information.");
        return;
      } else if (Address.StartsWith(pl.address) && pl.isContainer) {
        // when the payload is a DnDContainer and it is being dropped as a descendant of itself
        Console.Error.WriteLine("A container cannot be dropped as a child of itself.");
        return;
      }

      if (pl.fromContainer is null || pl.fromIndex < 0) { Console.Error.WriteLine("No item to drop"); return; }
      if (pl.fromIndex >= pl.fromContainer.List.Count()) { Console.Error.WriteLine("Index of the item being dragged was out of range"); return; }

      var itemToMove = pl.fromContainer.List[pl.fromIndex];
      // If dropping on empty, index out of range possibly
      var myItemAtIndex = List.Count() > 0 && index < List.Count() ? List[index] : default(TListItem);
      var childProp = string.IsNullOrWhiteSpace(ChildrenPropertyName) ? null : myItemAtIndex?.GetType().GetProperty(ChildrenPropertyName);
      if (pl.fromContainer == this && pl.fromIndex == index) {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine("The item was dropped onto itself."); }
        return;
      }

      if (ShowDebugInfo || overrideDebug) {
        Console.WriteLine($"itemToMove's address: {pl.address}");
        Console.WriteLine($"itemToMove's container's address: {pl.fromContainer.Address}");
        Console.WriteLine($"myItemAtIndex's address: {Address}.{index}");
        Console.WriteLine($"this's address: {Address}");
      }

      bool makeNewGroup = // The parameter to allow grouping when dropping on existing items is set to True AND
                          AllowDropToGroup &&
                          // The picked up item is being dropped on an existing item AND
                          isDropOnExisting &&
                          (
                            // The item being dropped is a container and the item at the drop target index is not an ancestor of the item being dropped OR
                            (pl.isContainer && !pl.fromContainer.Address.StartsWith($"{Address}.{index}")) ||
                            // The item being dropped is not a container and the item at the drop target index is not a container either
                            (!pl.isContainer && childProp is null)
                          );

      var updateEventArgs = new ListUpdateEventArgs();
      // TODO: Fill in the appropriate fields for the updateEventArgs
      if (makeNewGroup) {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Making a new group"); }

        // TODO: Add an argument to determine if this is a drop before/after
        var newGroup = Activator.CreateInstance<TGroup>();
        var children = new List<TListItem>();
        children.Add(dropIntoFirst ? itemToMove : myItemAtIndex);
        children.Add(dropIntoFirst ? myItemAtIndex : itemToMove);
        childProp = typeof(TGroup).GetProperty(ChildrenPropertyName);
        childProp.SetValue(newGroup, children);
        List.RemoveAt(index);
        List.Insert(index, (TListItem)(object)newGroup);
      } else {
        List.Insert(index, itemToMove);
      }
      UpdateArrayLengths();
      var removeIndex = pl.fromContainer == this && !makeNewGroup ? pl.fromIndex + (pl.fromIndex > index && !makeNewGroup ? 1 : 0) : pl.fromIndex;
      pl.fromContainer.List.RemoveAt(removeIndex);
      UpdateArrayLengths();
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine("Removing drop css styles"); }
      for (var i = 0; i < dropCssStyles.Length; i++) {
        dropCssStyles[i] = string.Empty;
      }
      // ================> Test for ungroup <================
      var containerToCheck = pl.fromContainer;
      // Test the fromContainer to see if there is <= 1 element and is not the BaseContainer and has a Parent container
      if (containerToCheck.List.Count() <= 1 && containerToCheck != BaseContainer && containerToCheck.Parent != null) {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"There {(containerToCheck.List.Count() == 0 ? "are 0 items" : "is 1 item")} in the container, so it should be ungrouped."); }
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

      await BaseContainer.SetPayload(null, -1);
      await BaseContainer.OnStatusUpdated.InvokeAsync(updateEventArgs);
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drop method complete"); }
    }

    protected void HandleDropTargetDragEnter(DragEventArgs e, int index, bool isBeforeTarget = false) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drop target drag entered for {Address}"); }
      isDropBefore = isBeforeTarget;
      UpdateArrayLengths();
      showGroupWith[index] = false;
      showNewItem[index] = false;
      var pl = BaseContainer.GetPayload();
      dropCssStyles[index] = $"{(Address.StartsWith(pl.address) ? $"no-drop{(pl.address == Address ? " drop-hovering" : "")}" : "can-drop drop-hovering")}";
    }

    protected void HandleDropTargetDragLeave(DragEventArgs e, int index) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Drop target drag left for {Address}"); }
      UpdateArrayLengths();
      if (index < dropCssStyles.Length) { dropCssStyles[index] = null; }
    }

    protected void HandleDelete(MouseEventArgs e) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Deleting for {Address}"); }
      if (Parent is null) { Console.Error.WriteLine("Parent is missing."); return; }

    }

    public void CheckForUngroup(DnDContainer<TListItem, TItem, TGroup> containerToCheck = null, int indexInParentContainer = -1) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Checking for ungroup for {Address}"); }

      if (containerToCheck is null) { containerToCheck = (DnDContainer<TListItem, TItem, TGroup>)this; }
      if (containerToCheck == BaseContainer) {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine("The container being checked for ungroup is the Base Container and will not be ungrouped."); }
        return;
      }

      if (containerToCheck.List.Count() > 1) {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine("The container has more than 1 element and should not be ungrouped."); }
        return;
      }

      if (ShowDebugInfo || overrideDebug) {
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
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Removing item at index {indexInParentContainer} from {parent.Address} due to ungroup"); }
      parent.List.RemoveAt(indexInParentContainer);
      if (containerToCheck.List.Count() == 1) {
        var item = containerToCheck.List[0];
        parent.List.Insert(indexInParentContainer, item);
      }
    }

    protected void AddItem(MouseEventArgs e, int index) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Adding item for {Address}"); }
      if (NewItem == null) {
        Console.Error.WriteLine("Failed to add the new item.");
      } else {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Hiding the Add New item for index: {index}, which is currently {showNewItem[index]}"); }
        if (index > showNewItem.Length - 1) {
          showNewItem[index - 1] = false;
          if (index > 1) { showNewItem[index - 2] = false; }
        } else {
          showNewItem[index] = false;
          if (index > 0) { showNewItem[index - 1] = false; }
        }

        List.Insert(index, (TListItem)(object)NewItem);
        UpdateArrayLengths();
        NewItem = Activator.CreateInstance<TItem>();
      }
    }

    protected void CancelAdd(MouseEventArgs e, int index) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Toggling show new item for {Address}"); }
      UpdateArrayLengths();
      if (index >= showNewItem.Length) { Console.Error.WriteLine($"Cannot show/hide new item at index {index}.  This index is out of range."); }
      showNewItem[index] = false;
    }

    protected void OnSaveComplete(DnDItem<TItem, TListItem, TGroup> itemComponent) {
      if (!int.TryParse(itemComponent.Address.Split('.').Last(), out var itemIndex)) {
        Console.Error.WriteLine("Could not determine the index for the new item.  Setting it to the end of the list.");
        itemIndex = List.Count() - 1;
      }
      NewItem = Activator.CreateInstance<TItem>();
      showNewItem[itemIndex] = false;
      if (itemIndex > 0) { showNewItem[itemIndex - 1] = false; }
    }

    protected void OnCancelComplete(DnDItem<TItem, TListItem, TGroup> itemComponent) {
      if (!int.TryParse(itemComponent.Address.Split('.').Last(), out var itemIndex)) {
        Console.Error.WriteLine("Could not determine the index of the new item being cancelled.");
        return;
      }
      if (itemComponent.IsAdd) {
        ToggleShowNewItem(null, itemIndex - (isAddBefore ? 0 : 1));
      } else {
        // TODO: Determine if anything needs to be done
      }
    }

    public async Task NotifyStateChanged(ListUpdateEventArgs e = null) {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine("Notifying state has changed"); }

      if (BaseContainer != null) {
        BaseContainer.StateHasChanged();
        if (BaseContainer.OnStatusUpdated.HasDelegate && e != null) { await BaseContainer.OnStatusUpdated.InvokeAsync(e); }
      } else {
        StateHasChanged();
        if (OnStatusUpdated.HasDelegate && e != null) { await OnStatusUpdated.InvokeAsync(e); }
      }
    }

    protected void ToggleShowNewItem(MouseEventArgs e, int index, bool addBefore = false) {
      var overrideDebug = false;
      var addBeforeChanging = isAddBefore != addBefore;
      isAddBefore = addBefore;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Toggling show new item for {Address}"); }
      UpdateArrayLengths();
      if (index >= showNewItem.Length) { Console.Error.WriteLine($"Cannot show/hide new item at index {index}.  This index is out of range."); }
      showNewItem[index] = (addBeforeChanging && showNewItem[index]) || !showNewItem[index];
      if (showNewItem[index]) {
        // Hide all other add elements for this container
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Since we are showng an Add New Item for this container, hiding all other Add New Item components for this container."); }
        for (int i = 0; i < showNewItem.Length; i++) {
          showNewItem[i] = (i == index);
        }
      }
    }

    public void UpdateArrayLengths() {
      var overrideDebug = false;
      if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Started process to update the array lengths for {Address}.  The current list length is: {List.Count()}"); }

      try {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Checking the length of dropCssStyles.  The current length is: {dropCssStyles.Length}"); }
        if (List.Count() != dropCssStyles.Length) {
          if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Updating array dropCssStyles array length from {dropCssStyles.Length} to {List.Count()}"); }
          var curStyles = new string[List.Count()];
          Array.Copy(dropCssStyles, 0, curStyles, 0, Math.Min(curStyles.Length, dropCssStyles.Length));
          dropCssStyles = curStyles;
          if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"dropCssStyles array length updated to {dropCssStyles.Length}"); }
        }
      } catch (Exception e) {
        Console.Error.WriteLine($"Encountered an error while trying to update the dropCssStyles array length for {Address}: {e.GetType()} - {e.Message}");
        while (e != null) {
          Console.Error.WriteLine(e.StackTrace);
          e = e.InnerException;
        }
      }
      try {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Checking the length of showGroupWith.  The current length is: {showGroupWith.Length}"); }
        if (List.Count() != showGroupWith.Length) {
          if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Updating array showGroupWith array length from {showGroupWith.Length} to {List.Count()}"); }
          var curShowGroupWiths = new bool[List.Count()];
          Array.Copy(showGroupWith, 0, curShowGroupWiths, 0, Math.Min(curShowGroupWiths.Length, showGroupWith.Length));
          showGroupWith = curShowGroupWiths;
          if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"showGroupWith array length updated to {showGroupWith.Length}"); }
        }
      } catch (Exception e) {
        Console.Error.WriteLine($"Encountered an error while trying to update the showGroupWith array length for {Address}: {e.GetType()} - {e.Message}");
        while (e != null) {
          Console.Error.WriteLine(e.StackTrace);
          e = e.InnerException;
        }
      }
      try {
        if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Checking the length of showNewItem.  The current length is: {showNewItem.Length}"); }
        if (List.Count() != showNewItem.Length) {
          if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"Updating array showNewItem array length from {showNewItem.Length} to {List.Count()}"); }
          var curShowNewItems = new bool[List.Count()];
          Array.Copy(showNewItem, 0, curShowNewItems, 0, Math.Min(curShowNewItems.Length, showNewItem.Length));
          showNewItem = curShowNewItems;
          if (ShowDebugInfo || overrideDebug) { Console.WriteLine($"showNewItem array length updated to {showNewItem.Length}"); }
        }
      } catch (Exception e) {
        Console.Error.WriteLine($"Encountered an error while trying to update the showNewItem array length for {Address}: {e.GetType()} - {e.Message}");
        while (e != null) {
          Console.Error.WriteLine(e.StackTrace);
          e = e.InnerException;
        }
      }
    }
  }
}
