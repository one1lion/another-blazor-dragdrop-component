using System;
using System.Collections.Generic;
using DragAndDrop.Components.Interfaces;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
  /// and <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
  /// that can be dragged onto/into other <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" />s,
  /// moving the entire group (with its children) into the 
  /// target <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" />
  /// </summary>
  public class DraggableGroup : DragAndDropContainer, IDraggableElement {
    /// <summary>The default constructor</summary>
    public DraggableGroup() : base() { }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.DragEnabled" />
    public bool DragEnabled { get; set; } = true;
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.AllowedTargetNames" />
    public List<string> AllowedTargetNames { get; set; }
  }
}
