using System;
using System.Collections.Generic;
using System.Linq;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// The basic element for a Drag And Drop Component of which all elements are derived
  /// </summary>
  /// <remarks>
  /// By default, Drag And Drop elements that implement this are static elements managed by
  /// this control.  To make an element draggable, it should implement
  /// <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
  /// </remarks>
  public interface IDragAndDropElement {
    /// <summary>A unique identifier for the element</summary>
    string Id { get; }
    /// <summary>The name for this element</summary>
    string Name { get; set; }
    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" /> 
    /// that this element is nested in
    /// </summary>
    IDragAndDropContainer Parent { get; set; }

    /// <summary>
    /// The other elements nested in the 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Parent"/> element
    /// </summary>
    public List<IDragAndDropElement> Siblings => Parent?.Children?.Where(s => s.Id != Id).ToList();
  }
}
