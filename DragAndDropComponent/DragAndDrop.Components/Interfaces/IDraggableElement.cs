using System.Collections.Generic;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// An <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> that is
  /// specified to be draggable
  /// </summary>
  /// <remarks>
  /// An element that only implements this interface cannot contain children by default.
  /// To allow children, also implement the 
  /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> interface.
  /// </remarks>
  public interface IDraggableElement : IDragAndDropElement {
    /// <summary>
    /// A flag indicating whether or not this element should 
    /// be allowed to be dragged currently
    /// </summary>
    bool DragEnabled { get; set; }
    /// <summary>
    /// A list of target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Name" />s
    /// the item can be dropped into/onto
    /// </summary>
    List<string> AllowedTargetNames { get; set; }

    /// <summary>
    /// Indicates whether the current element can be dropped into or onto a specified target name
    /// </summary>
    /// <param name="container"> 
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Name" /> to test
    /// whether this element can be dropped into/onto
    /// </param>
    /// <returns>Whether or not this element can be dropped into/onto the specified target name</returns>
    public bool CanDrop(IDragAndDropContainer container) => container is { } && AllowedTargetNames is { } && AllowedTargetNames.Contains(container.Name);

    // TODO: Remove this when ready to implement
    //void GroupWith(IDragAndDropElement element);



    //public bool MoveInto(IDragAndDropContainer container, int? order = default) {
    //  if (container is null || !CanDrop(container)) { return false; }

    //  if(container.Id == Parent?.Id) {

    //  } else {

    //  }
    //  Parent = container;
    //  // Get all draggable
    //  var elemChildren = ((List<IDraggableElement>)container.Children.Where(x => x.GetType().GetTypeInfo().IsAssignableFrom(typeof(IDraggableElement).GetTypeInfo())).Select(x => (IDraggableElement)x).OrderBy(x => x.Order));
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
