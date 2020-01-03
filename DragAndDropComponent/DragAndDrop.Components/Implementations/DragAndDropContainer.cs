using DragAndDrop.Components.Interfaces;
using System;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
  /// that allows <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>s to be
  /// added as child elements
  /// </summary>
  public class DragAndDropContainer : IDragAndDropContainer {
    public DragAndDropContainer() {
      Id = Guid.NewGuid().ToString();
    }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Id" />
    public string Id { get; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Name" />
    public string Name { get; set; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Parent" />
    public IDragAndDropContainer Parent { get; set; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children" />
    public IList<IDragAndDropElement> Children { get; set; }
  }
}
