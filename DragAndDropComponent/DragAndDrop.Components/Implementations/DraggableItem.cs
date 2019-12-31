using DragAndDrop.Components.Interfaces;
using System.Collections.Generic;

namespace DragAndDrop.Components {
  public class DraggableItem<T> : IDraggableElement<T> {
    public string Name { get; set; } = "";
    public int Order { get; set; }
    public T Item { get; set; }

    public List<string> AllowedTargetNames { get; set; } = new List<string>() { "" };

    public bool DragEnabled { get; set; } = true;

    // From Interface:
    //public bool CanDrop(string inGroup) => inGroup != null && AllowedDropGroups != null && AllowedDropGroups.Contains(inGroup);

    public IDragAndDropElement<T> Parent { get; set; }
    public IList<IDragAndDropElement<T>> Children { 
      get { return null; }
      set { return; } 
    }
  }
}
