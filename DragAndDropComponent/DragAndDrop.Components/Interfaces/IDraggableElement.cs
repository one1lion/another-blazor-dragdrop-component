using System.Collections.Generic;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// An <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}"/> that is
  /// specified to be draggable
  /// </summary>
  /// <typeparam name="T">The type of data contained within the element</typeparam>
  public interface IDraggableElement<T> : IDragAndDropElement<T> {
    /// <summary>The order this element should be displayed</summary>
    int Order { get; set; }
    /// <summary>
    /// A flag indicating whether or not this element should 
    /// be allowed to be dragged currently
    /// </summary>
    bool DragEnabled { get; set; }
    /// <summary>
    /// A list of target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}.Name" /> 
    /// the item can be dropped into/onto
    /// </summary>
    List<string> AllowedTargetNames { get; set; }

    /// <summary>
    /// Indicates whether the current element can be dropped into or onto a specified target name
    /// </summary>
    /// <param name="targetName">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement{T}.Name" /> to test
    /// whether this element can be dropped into/onto
    /// </param>
    /// <returns>Whether or not this element can be dropped into/onto the specified target name</returns>
    public bool CanDrop(string targetName) => targetName != null && AllowedTargetNames != null && AllowedTargetNames.Contains(targetName);

    void GroupWith(IDragAndDropElement<T> element);
    void DropInto(IDragAndDropElement<T> element);
  }
}
