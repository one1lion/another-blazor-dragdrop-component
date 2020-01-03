using System.Collections.Generic;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// Extends the basic <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>
  /// by adding a <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
  /// property, enabling this element to contain child 
  /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>s
  /// </summary>
  public interface IDragAndDropContainer : IDragAndDropElement {
    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement" />s 
    /// nested within this element
    /// </summary>
    IList<IDragAndDropElement> Children { get; set; }
    // TODO: remove comment when ready to implement
    //void AddChild(IDragAndDropElement element);
  }
}
