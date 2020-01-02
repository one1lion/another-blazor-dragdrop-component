using System;
using System.Collections.Generic;
using System.Text;
using DragAndDrop.Components.Interfaces;

namespace DragAndDrop.Components {
  public class DraggableGroup<T> : IDraggableElement<T>, IDragAndDropContainer<T> {
    public int Order { get; set; }
    public bool DragEnabled { get; set; } = true;
    public List<string> AllowedTargetNames { get; set; }
    public IDragAndDropContainer<T> Parent { get; set; }
    public string Name { get; set; }
    public IList<IDragAndDropElement<T>> Children { get; set; }
  }
}
