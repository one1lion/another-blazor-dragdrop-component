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
    /// <param name="container"> 
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer{T}.Name" /> to test
    /// whether this element can be dropped into/onto
    /// </param>
    /// <returns>Whether or not this element can be dropped into/onto the specified target name</returns>
    public bool CanDrop(IDragAndDropContainer<T> container) => container is { } && AllowedTargetNames is { } && AllowedTargetNames.Contains(container.Name);

    // TODO: Remove this when ready to implement
    //void GroupWith(IDragAndDropElement<T> element);



    //public bool MoveInto(IDragAndDropContainer<T> container, int? order = default) {
    //  if (container is null || !CanDrop(container)) { return false; }

    //  if(container.Id == Parent?.Id) {

    //  } else {

    //  }
    //  Parent = container;
    //  // Get all draggable
    //  var elemChildren = ((List<IDraggableElement<T>>)container.Children.Where(x => x.GetType().GetTypeInfo().IsAssignableFrom(typeof(IDraggableElement<T>).GetTypeInfo())).Select(x => (IDraggableElement<T>)x).OrderBy(x => x.Order));
    //  for(var i = 0; i < elemChildren.Count(); i++) {
    //    elemChildren[i].Order = i;
    //  }
    //  if (!container.Children.Any(x => x.Id == Id)) { container.Children.Add(this); }
    //  if (order == default) {
    //    Order = container.Children.Count;
    //  } else {

    //  }

    //  return true;
    //}

  }
}
