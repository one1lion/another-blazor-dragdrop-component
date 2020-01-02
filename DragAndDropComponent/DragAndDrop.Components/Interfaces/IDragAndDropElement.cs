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
    string Id { get => string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString() : Id; }
    /// <summary>The name for this element</summary>
    string Name { get; set; }
    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}" /> 
    /// that this element is nested in
    /// </summary>
    IDragAndDropElement<T> Parent { get; set; }
    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}" />s 
    /// nested within this element
    /// </summary>
    IList<IDragAndDropElement<T>> Children { get; set; }
    /// <summary>
    /// The other elements nested in the 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}.Parent"/> element
    /// </summary>
    public List<IDragAndDropElement<T>> Sibblings => Parent?.Children?.Where(s => s.Id != Id).ToList();

    // TODO: remove this when ready to implement
    //void AddChild(IDragAndDropElement<T> element);
  }
}
