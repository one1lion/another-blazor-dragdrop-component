using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragAndDrop.Components.Interfaces {
  public interface IDragAndDropContainer<T> : IDragAndDropElement<T> {
    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}" />s 
    /// nested within this element
    /// </summary>
    IList<IDragAndDropElement<T>> Children { get; set; }
    // TODO: remove this when ready to implement
    //void AddChild(IDragAndDropElement<T> element);
  }
}
