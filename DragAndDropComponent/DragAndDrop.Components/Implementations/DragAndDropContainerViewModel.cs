using DragAndDrop.Components.Interfaces;
using System;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
  /// that allows <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>s to be
  /// added as child elements
  /// </summary>
  public class DragAndDropContainerViewModel : IDragAndDropContainer {
    /// <summary>The default constructor</summary>
    public DragAndDropContainerViewModel() {
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

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Clone"/>
    public DragAndDropContainerViewModel Clone() {
      return ((IDragAndDropElement)this).Clone<DragAndDropContainerViewModel>();
    }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.GroupWith(IDragAndDropElement, bool)"/>
    public IDragAndDropContainer GroupWith(IDragAndDropElement element, bool showFirst = false) {
      // TODO: Implement
      throw new NotImplementedException();
    }
  }
}
