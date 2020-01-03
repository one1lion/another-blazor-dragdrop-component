using DragAndDrop.Components.Interfaces;
using System;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
  /// that can be dragged onto/into <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" />s
  /// </summary>
  /// <typeparam name="T">The type of data contained within the element</typeparam>
  public class DraggableItem<T> : IDraggableElement {
    public DraggableItem() {
      Id = Guid.NewGuid().ToString();
    }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Id" />
    public string Id { get; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Name" />
    public string Name { get; set; }
    /// <summary>
    /// The item being wrapped by this draggable element
    /// </summary>
    public T Item { get; set; }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.AllowedTargetNames" />
    public List<string> AllowedTargetNames { get; set; } = new List<string>();

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.DragEnabled" />
    public bool DragEnabled { get; set; } = true;

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Parent" />
    public IDragAndDropContainer Parent { get; set; }
  }
}
