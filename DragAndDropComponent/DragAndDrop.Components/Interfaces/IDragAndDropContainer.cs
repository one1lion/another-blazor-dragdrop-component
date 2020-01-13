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
    /// <exception cref="System.ArgumentNullException">
    /// Throws an Argument Null Exception if the provided <paramref name="element"/> is null
    /// </exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Throws an Argument Exception if the provided <paramref name="targetIndex"/> value is
    /// negative or greater than the count of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Throws an Operation Canceled Exception if the <paramref name="element"/> already
    /// exists as a child
    /// </exception>
    public void AddChild(IDragAndDropElement element, int? targetIndex = default) {
      if (element is null) { throw new ArgumentNullException("The element to be added is null."); }

      if (Children is null) { Children = new List<IDragAndDropElement>(); }
      if (Children.Any(ce => ce.Id == element.Id)) { throw new OperationCanceledException("The element already exists as a child.  Use the MoveChild method to reposition an existing child element."); }
      if (!targetIndex.HasValue) { targetIndex = Children.Count(); }

      if (targetIndex.Value < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (targetIndex.Value > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than or equal to the count of children."); }

      if (element.Parent is { }) { element.Parent.RemoveChild(element); }
      Children.Insert(targetIndex.Value, element);
      element.Parent = this;
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
    public void AppendChild(IDragAndDropElement element) {
      AddChild(element);
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
    public void PrependChild(IDragAndDropElement element) {
      AddChild(element, 0);
    }
    #endregion

    #region Move an element within this list to a different index
    /// <summary>
    /// Move an existing element within the list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> 
    /// to a target index
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
    public void MoveChild(IDragAndDropElement existingChild, int? targetIndex = default) {
      if (Children is null) { Children = new List<IDragAndDropElement>(); }

      if (existingChild is null) { throw new ArgumentNullException("The element to be added is null."); }
      if (!Children.Any(ce => ce.Id == existingChild.Id)) { throw new ArgumentNullException("The element does not exist as a child.  Use the AddChild method to add a new child element to the list of Children."); }

      if (targetIndex == default) { targetIndex = Children.Count(); }
      if (targetIndex < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (targetIndex > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than or equal to the count of children."); }

      var originalIndex = existingChild.Parent.Children.IndexOf(existingChild);
      // If the from and to indexes are the same, there is nothing more to be done
      if (originalIndex == targetIndex.Value) { return; }

      // Insert the element into the list of children at the new index
      Children.Insert(targetIndex.Value, existingChild);

      // Remove the element from the children.  When doing so, add 1
      // to the index to remove from if the original element is
      // moving from a higher numbered index to a lower numbered index
      Children.RemoveAt(originalIndex + (originalIndex > targetIndex ? 1 : 0));
    }

    /// <summary>
    /// Move an existing element within the list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> 
    /// to a target index
    /// </summary>
    /// <param name="index">
    /// The index for an existing <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> 
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
    public void MoveChild(int index, int? targetIndex = default) {
      if (Children is null) { Children = new List<IDragAndDropElement>(); }

      if (index < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (index > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than the count of children."); }

      var existingChild = Children[index];
      if (existingChild is null) { throw new ArgumentNullException("The element to be added is null."); }
      if (!Children.Any(ce => ce.Id == existingChild.Id)) { throw new ArgumentNullException("The element does not exist as a child.  Use the AddChild method to add a new child element to the list of Children."); }

      if (targetIndex == default) { targetIndex = Children.Count(); }
      if (targetIndex < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (targetIndex > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than or equal to the count of children."); }

      var originalIndex = existingChild.Parent.Children.IndexOf(existingChild);
      // If the from and to indexes are the same, there is nothing more to be done
      if (originalIndex == targetIndex.Value) { return; }

      // Insert the element into the list of children at the new index
      Children.Insert(targetIndex.Value, existingChild);

      // Remove the element from the children.  When doing so, add 1
      // to the index to remove from if the original element is
      // moving from a higher numbered index to a lower numbered index
      Children.RemoveAt(originalIndex + (originalIndex > targetIndex ? 1 : 0));
    }
    #endregion

    #region Remove
    /// <summary>
    /// Remove an existising child of this 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="element">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to be removed
    /// </param>
    /// <returns>
    /// The removed child if the requested element exists in the list of children, otherwise null 
    /// </returns>
    public IDragAndDropElement RemoveChild(IDragAndDropElement element) {
      if (Children is null) { Children = new List<IDragAndDropElement>(); }

      if (element is null || !Children.Contains(element)) { return null; }

      Children.Remove(element);
      element.Parent = null;

      return element;
    }

    /// <summary>
    /// Remove an existising child of this 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> at the
    /// specified index
    /// </summary>
    /// <param name="index">
    /// The index of the child <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to be removed
    /// </param>
    /// <returns>
    /// The removed child if the requested element exists in the list of children, otherwise null 
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Throws an Argument Exception if the provided <paramref name="index"/> value is
    /// negative or greater than or equal to the count of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/>
    /// </exception>
    public IDragAndDropElement RemoveChild(int index) {
      if (Children is null) { return null; }

      if (index < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (index > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than the count of children."); }

      var element = Children[index];
      Children.Remove(element);
      element.Parent = null;

      return element;
    }
    #endregion

    #region Copy a child element
    /// <summary>
    /// Copies a child element to a target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// at the specified index
    /// </summary>
    /// <param name="existingChild">
    /// An existing <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> 
    /// in this container's list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> 
    /// </param>
    /// <param name="toContainer">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> to add
    /// the copy of the <paramref name="existingChild"/> to
    /// </param>
    /// <param name="targetIndex">
    /// The index the copied element should appear in the <paramref name="toContainer"/>.
    /// If this is default, the copy will be added as the last element
    /// </param>
    /// <returns></returns>
    public IDragAndDropElement CopyChild(IDragAndDropElement existingChild, IDragAndDropContainer toContainer, int? targetIndex = default) {
      var copyOfChild = existingChild.Clone<IDragAndDropElement>();
      toContainer.AddChild(copyOfChild, targetIndex);
      return copyOfChild;
    }

    /// <summary>
    /// Copies a child element by its index to a target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// at the specified index
    /// </summary>
    /// <param name="index">
    /// The index for an existing <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> 
    /// in this container's list of 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer.Children"/> 
    /// </param>
    /// <param name="toContainer">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> to add
    /// the copy of the child located at the specified <paramref name="index"/>
    /// </param>
    /// <param name="targetIndex">
    /// The index the copied element should appear in the <paramref name="toContainer"/>.
    /// If this is default, the copy will be added as the last element
    /// </param>
    /// <returns></returns>
    public IDragAndDropElement CopyChild(int index, IDragAndDropContainer toContainer, int? targetIndex = default) {
      if (Children is null) { return null; }

      if (index < 0) { throw new ArgumentOutOfRangeException("The specified target index must be greater than or equal to 0."); }
      if (index > Children.Count()) { throw new ArgumentOutOfRangeException("The specified target index must be less than the count of children."); }

      var existingChild = Children[index];
      var copyOfChild = existingChild.Clone<IDragAndDropElement>();
      toContainer.AddChild(copyOfChild, targetIndex);
      return copyOfChild;
    }
    #endregion
  }
}
