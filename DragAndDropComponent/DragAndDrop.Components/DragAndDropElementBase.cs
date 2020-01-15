using DragAndDrop.Components.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> that
  /// wraps content that extends Blazor's <see cref="Microsoft.AspNetCore.Components.ComponentBase"/>.  
  /// This element is not draggable.
  /// </summary>
  public class DragAndDropElementBase : ComponentBase, IDragAndDropElement {
    /// <summary>
    /// The default constructor
    /// </summary>
    public DragAndDropElementBase() {
      Id = Guid.NewGuid().ToString();
    }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Id"/>
    public string Id { get; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Name"/>
    [Parameter] public string Name { get; set; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Parent"/>
    [Parameter] public IDragAndDropContainer Parent { get; set; }
  }
}
