using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

    // TODO: Remove this when ready to implement
    //void GroupWith(IDragAndDropElement<T> element);

    public bool MoveInto(IDragAndDropElement<T> element, int? order = default) {
      if (element is null || !CanDrop(element.Name)) { return false; }

      if(element.Id == Parent?.Id) {

      } else {

      }
      Parent = element;
      // Get all draggable
      var elemChildren = ((List<IDraggableElement<T>>)element.Children.Where(x => x.GetType().GetTypeInfo().IsAssignableFrom(typeof(IDraggableElement<T>).GetTypeInfo())).Select(x => (IDraggableElement<T>)x).OrderBy(x => x.Order));
      for(var i = 0; i < elemChildren.Count(); i++) {
        elemChildren[i].Order = i;
      }
      if (!element.Children.Any(x => x.Id == Id)) { element.Children.Add(this); }
      if (order == default) {
        Order = element.Children.Count;
      } else {

      }

      return true;
    }

  }
}
