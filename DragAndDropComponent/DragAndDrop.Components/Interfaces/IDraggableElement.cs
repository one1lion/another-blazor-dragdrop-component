using System.Collections.Generic;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// An <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}"/> that is
  /// specified to be draggable
  /// </summary>
  /// <typeparam name="T">The type of data contained within the element</typeparam>
  public interface IDraggableElement<T> : IDragAndDropElement<T> {
    int Order { get; set; }
    bool DragEnabled { get; set; }
    List<string> AllowedTargetNames { get; set; }

    public bool CanDrop(string targetName) => targetName != null && AllowedTargetNames != null && AllowedTargetNames.Contains(targetName);
  }
}
