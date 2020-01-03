using System;
using System.Collections.Generic;
using System.Linq;

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

    #region Add to list
    /// <summary>
    /// Add an element that does not already exist as a child of this 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="element">
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to be added as a 
    /// child
    /// </param>
    /// <param name="targetIndex">
    /// The index position in the list of
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> to
    /// add the <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> at.  If
    /// this is not specified (default), it will be added at the end
    /// </param>
    /// <returns>Whether adding the child was successful</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Throws an Argument Exception if the provided <paramref name="targetIndex"/> value is
    /// negative or greater than the count of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
    /// </exception>
    public bool AddChild(IDragAndDropElement element, int? targetIndex = default) {
      if (Children is null) { Children = new List<IDragAndDropElement>(); }

      if (element is null || Children.Any(ce => ce.Id == element.Id)) { return false; }
      if (targetIndex == default) { targetIndex = Children.Count(); }

      if (targetIndex < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (targetIndex > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than or equal to the count of children."); }

      Children.Insert(targetIndex.Value, element);
      element.Parent = this;

      return true;
    }

    /// <summary>
    /// Append an element that does not already exist as a child of this 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// to the end of the list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
    /// </summary>
    /// <param name="element">
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to be added as a 
    /// child
    /// </param>
    /// <returns>Whether appending the child was successful</returns>
    public bool AppendChild(IDragAndDropElement element) {
      return AddChild(element);
    }

    /// <summary>
    /// Prepend an element that does not already exist as a child of this 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// to the beginning of the list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
    /// </summary>
    /// <param name="element">
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to be added as a 
    /// child
    /// </param>
    /// <returns>Whether prepending the child was successful</returns>
    public bool PrependChild(IDragAndDropElement element) {
      return AddChild(element, 0);
    }
    #endregion

    #region Move an element within this list to a different index
    /// <summary>
    /// Move an existing element within this list to a target index
    /// </summary>
    /// <param name="existingChild">
    /// An existing <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> 
    /// in this container's list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> 
    /// </param>
    /// <param name="targetIndex">
    /// The index position in the list of
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> to
    /// move the <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to.  If
    /// this is not specified (default), it will be moved to the end
    /// </param>
    /// <returns>Whether moving the child was successful</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Throws an Argument Exception if the provided <paramref name="targetIndex"/> value is
    /// negative or greater than the count of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
    /// </exception>
    public bool MoveElement(IDragAndDropElement existingChild, int? targetIndex = default) {
      if (Children is null) { Children = new List<IDragAndDropElement>(); }

      if (existingChild is null || !Children.Any(ce => ce.Id == existingChild.Id)) { return false; }

      if (targetIndex == default) { targetIndex = Children.Count(); }
      if (targetIndex < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (targetIndex > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than or equal to the count of children."); }

      var originalIndex = existingChild.Parent.Children.IndexOf(existingChild);
      if (originalIndex == targetIndex.Value) { return true; }

      // Insert the element into the list of children at the new index
      Children.Insert(targetIndex.Value, existingChild);

      // Remove the element from the children.  When doing so, add 1
      // to the index to remove from if the original element is
      // moving from a higher numbered index to a lower numbered index
      Children.RemoveAt(originalIndex + (originalIndex > targetIndex ? 1 : 0));

      return true;
    }
    #endregion
  }
}
