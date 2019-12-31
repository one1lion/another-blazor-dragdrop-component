using DragAndDrop.Components.Interfaces;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  public class DraggableGroup<T> : DragAndDropContainerElement<T>, IDraggableElement<T> {
    public int Order { get; set; }
    public bool DragEnabled { get; set; }
    public List<string> AllowedTargetNames { get; set; }
  }
}
