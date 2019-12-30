using DragAndDrop.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragAndDrop.Pages {
  public class DragAndDropSampleBase : ComponentBase {
    protected override void OnInitialized() {
      // Initialize some items to display
      draggableItems = SampleDataService.GenerateSampleDraggableItems();
    }

    /// <summary>The list of draggable items to be displayed</summary>
    /// <remarks>We are currently using strings as the item type, but this can be any object</remarks>
    protected List<DraggableItem<string>> draggableItems = new List<DraggableItem<string>>();

    /// <summary>The most recent <see cref="DragAndDrop.Data.DraggableItem{T}"/> being dragged</summary>
    protected DraggableItem<string> curItem = null;
    /// <summary>Whether or not dragging is occurring</summary>
    protected bool dragging;

    /// <summary>
    /// On drag start, handles setting the currently dragged item and sets the "dragging" flag to true
    /// </summary>
    /// <param name="e">The arguments related to the drag event</param>
    /// <param name="draggedItem">The <see cref="DragAndDrop.Data.DraggableItem{T}"/> that is being picked up</param>
    protected void HandleDragStart(DragEventArgs e, DraggableItem<string> draggedItem) {
      curItem = draggedItem;
      dragging = true;
    }

    /// <summary>
    /// On drag end, handles turning off the "dragging" flag
    /// </summary>
    /// <param name="e">The arguments related to the drag event</param>
    protected void HandleDragEnd(DragEventArgs e) {
      Console.WriteLine("Drag Ended");
      dragging = false;
      curItem = null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="e">The arguments related to the drag event</param>
    /// <param name="newGroupName">
    /// The group the currently dragged <see cref="DragAndDrop.Data.DraggableItem{T}"/> is
    /// being dropped into
    /// </param>
    /// <param name="newOrder">
    /// The location number to move the <see cref="DragAndDrop.Data.DraggableItem{T}"/> to
    /// within the <paramref name="newGroupName"/>
    /// </param>
    /// <returns></returns>
    protected async Task HandleMove(DragEventArgs e, string newGroupName, int newOrder) {
      Console.WriteLine("Drag Move");
      if (curItem is null) { return; }
      var dragItem = draggableItems.SingleOrDefault(d => d.Id == curItem.Id);
      if (dragItem is { }) {
        MoveItem(dragItem.GroupName, newGroupName, dragItem.Order, newOrder);
        if(!dragging) { curItem = null; }
      }
      await InvokeAsync(() => StateHasChanged());
    }

    /// <summary>
    /// Handles moving a <see cref="DragAndDrop.Data.DraggableItem{T}"/>
    /// </summary>
    /// <param name="fromGroupName">
    /// The group name the <see cref="DragAndDrop.Data.DraggableItem{T}"/> is moving from
    /// </param>
    /// <param name="toGroupName">
    /// The group name the <see cref="DragAndDrop.Data.DraggableItem{T}"/> is moving to
    /// </param>
    /// <param name="fromOrder">
    /// The location number to move the <see cref="DragAndDrop.Data.DraggableItem{T}"/> from
    /// within the <paramref name="fromGroupName"/>
    /// </param>
    /// <param name="toOrder">
    /// The location number to move the <see cref="DragAndDrop.Data.DraggableItem{T}"/> to
    /// within the <paramref name="toGroupName"/>
    /// </param>
    protected void MoveItem(string fromGroupName, string toGroupName, int fromOrder, int toOrder) {
      var sameGroupName = fromGroupName == toGroupName;
      if (sameGroupName && fromOrder == toOrder) { return; }

      var dragItem = draggableItems.SingleOrDefault(d => d.Id == curItem.Id);
      if (sameGroupName) {
        // If moving within the same dropzone (re-ordering only)
        var items = draggableItems.Where(d => d.GroupName == fromGroupName);
        if (fromOrder < toOrder) {
          // When moving from a lower numbered position to a higher numbered position,
          // reduce the number of each item's position that is greater than the item's
          // current position, but less than or equal to the new position
          items.Where(d => d.Order > fromOrder && d.Order < toOrder).ToList().ForEach(d => {
            d.Order--;
          });
        } else {
          // When moving from a higher numbered position to a lower numbered position,
          // increase the number of each item's position that is less than the item's
          // current position, but greater than or equal to the new position
          items.Where(d => d.Order < fromOrder && d.Order >= toOrder).ToList().ForEach(d => {
            d.Order++;
          });
        }
      } else {
        // If moving from one group to another, decrease the position number for all
        // items in the source group whose position number is greater than the item's
        // current position
        draggableItems.Where(d => d.GroupName == fromGroupName && d.Order > fromOrder).ToList().ForEach(d => {
          d.Order--;
        });
        // Also, increase the position number for all items in the destination group
        // whose position number is greater than or equal to the position the item is being
        // dropped at
        draggableItems.Where(d => d.GroupName == fromGroupName && d.Order >= toOrder).ToList().ForEach(d => {
          d.Order++;
        });
      }

      // Set the group and order for the item being dragged
      dragItem.GroupName = toGroupName;
      dragItem.Order = toOrder;

      // Renumber the affected group item order to remove any gaps that may have been
      // caused by the move process
      RenumberList(fromGroupName);
      if (fromGroupName != toGroupName) {
        RenumberList(toGroupName);
      }
    }

    /// <summary>
    /// Renumbers the item order in the group with the specified name to remove gaps or
    /// duplicate numbers
    /// </summary>
    /// <param name="inGroupName"></param>
    protected void RenumberList(string inGroupName) {
      var items = draggableItems.Where(d => d.GroupName == inGroupName).OrderBy(d => d.Order);
      for (var i = 0; i < items.Count(); i++) {
        items.ElementAt(i).Order = i;
      }
    }
  }
}
