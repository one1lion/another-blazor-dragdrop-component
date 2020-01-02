using DragAndDrop.Components.Interfaces;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  public class DragAndDropContainer<T> : IDragAndDropContainer<T> {
    public string Name { get; set; }
    public int Order { get; set; }
    public IDragAndDropContainer<T> Parent { get; set; }
    public IList<IDragAndDropElement<T>> Children { get; set; }
  }
}
