using DragAndDrop.Components.Interfaces;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  public class DragAndDropContainerElement<T> : IDragAndDropElement<T> {
    public string Name { get; set; }
    public IDragAndDropElement<T> Parent { get; set; }
    public IList<IDragAndDropElement<T>> Children { get; set; }
  }
}
