using System;

namespace DragAndDrop.Components {
  /// <summary> 
  /// The wrapper for a draggable item
  /// </summary>
  /// <typeparam name="T">The type of item this is wrapping</typeparam>
  public class DraggableItem<T> {
    public DraggableItem() {
      Id = Guid.NewGuid().ToString();
    }

    /// <summary>A unique identifier for this Draggable Item</summary>
    public string Id { get; private set; }
    /// <summary>The group this item belongs to</summary>
    public string GroupName { get; set; } = "";
    /// <summary>The order this item should be printed within the group</summary>
    public int Order { get; set; }
    /// <summary>The object being wrapped</summary>
    public T Item { get; set; }
  }
}
