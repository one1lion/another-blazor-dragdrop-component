using System;
using System.Collections.Generic;
using System.Linq;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// The basic element for a Drag And Drop Component of which all elements are derived
  /// </summary>
  /// <typeparam name="T">The type of data contained within the element</typeparam>
  /// <remarks>
  /// By default, Drag And Drop elements that implement this 
  /// </remarks>
  public interface IDragAndDropElement<T> {
    /// <summary>A unique identifier for the element</summary>
    public string Id { get => string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString() : Id; }
    /// <summary>The name for this element</summary>
    string Name { get; set; }
    /// <summary>The order this element should be displayed</summary>
    int Order { get; set; }

    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer{T}" /> 
    /// that this element is nested in
    /// </summary>
    IDragAndDropContainer<T> Parent { get; set; }

    /// <summary>
    /// The other elements nested in the 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer{T}.Parent"/> element
    /// </summary>
    public List<IDragAndDropElement<T>> Siblings => Parent?.Children?.Where(s => s.Id != Id).ToList();
  }
}
