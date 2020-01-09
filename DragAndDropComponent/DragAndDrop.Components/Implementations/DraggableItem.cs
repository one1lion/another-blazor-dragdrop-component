using DragAndDrop.Components.Interfaces;
using System;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
  /// that can be dragged onto/into <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" />s
  /// </summary>
  /// <typeparam name="T">The type of data contained within the element</typeparam>
  public class DraggableItem<T> : DragAndDropItem<T>, IDraggableElement {
    /// <summary>The default constructor</summary>
    public DraggableItem() : base() { }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.AllowedTargetNames" />
    public List<string> AllowedTargetNames { get; set; } = new List<string>();

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.DragEnabled" />
    public bool DragEnabled { get; set; } = true;

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Clone"/>
    public new DraggableItem<T> Clone() {
      return ((IDragAndDropElement)this).Clone<DraggableItem<T>>();
    }

  }
}
