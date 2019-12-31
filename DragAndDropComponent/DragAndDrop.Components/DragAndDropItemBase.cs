using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Components {
  public class DragAndDropItemBase<TItem> : ComponentBase {
    [CascadingParameter] public DragAndDropContainer<TItem> Container { get; set; }
    [CascadingParameter] public DragAndDropGroup<TItem> DragAndDropGroup { get; set; }

    [Parameter] public DraggableItem<TItem> Model { get; set; }
    [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

    protected void HandleDragStart(DragEventArgs e, DraggableItem<TItem> selectedItem) {

    }

    protected void HandleDragEnd(DragEventArgs e) {

    }

    protected void HandleMove(DragEventArgs e, string newGroupName, int newOrder) {

    }
  }
}
