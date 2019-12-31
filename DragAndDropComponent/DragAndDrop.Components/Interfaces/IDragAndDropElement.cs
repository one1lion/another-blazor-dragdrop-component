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
    string Id { get => string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString() : Id; }
    string Name { get; set; }
    IDragAndDropElement<T> Parent { get; set; }
    IList<IDragAndDropElement<T>> Children { get; set; }
    public List<IDragAndDropElement<T>> Sibblings => Parent?.Children?.Where(s => s.Id != Id).ToList();
  }
}
