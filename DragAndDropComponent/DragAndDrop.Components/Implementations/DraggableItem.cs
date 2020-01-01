using DragAndDrop.Components.Interfaces;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDraggableElement{T}"/>
  /// that can be dragged onto/into other elements
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class DraggableItem<T> : IDraggableElement<T> {
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}.Name" />
    public string Name { get; set; } = "";
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement{T}.Order" />
    public int Order { get; set; }
    /// <summary>
    /// The item being wrapped by this draggable element
    /// </summary>
    public T Item { get; set; }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement{T}.AllowedTargetNames" />
    public List<string> AllowedTargetNames { get; set; } = new List<string>() { "" };

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement{T}.DragEnabled" />
    public bool DragEnabled { get; set; } = true;

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}.Parent" />
    public IDragAndDropElement<T> Parent { get; set; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}.Children" />
    public IList<IDragAndDropElement<T>> Children { 
      get { return null; }
      set { return; } 
    }
  }
}
