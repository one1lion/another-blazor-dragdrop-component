using System;

namespace DragAndDrop.Data {
  /// <summary> 
  /// The wrapper for a draggable item
  /// </summary>
  /// <typeparam name="T">The type of item this is wrapping</typeparam>
  /// <remarks>
  /// This is the original version of DraggableItem used in the Sample.
  /// The version for the Component can be found in the DragAndDrop.Components
  /// library project.
  /// </remarks>
  public class DraggableItemSample<T> {
    public DraggableItemSample() {
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
