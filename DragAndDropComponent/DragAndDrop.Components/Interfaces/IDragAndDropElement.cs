using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// The basic element for a Drag And Drop Component of which all elements are derived
  /// </summary>
  /// <remarks>
  /// By default, Drag And Drop elements that implement this are static elements managed by
  /// this control.  To make an element draggable, it should implement
  /// <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
  /// </remarks>
  public interface IDragAndDropElement {
    /// <summary>A unique identifier for the element</summary>
    string Id { get; }
    /// <summary>The name for this element</summary>
    string Name { get; set; }

    /// <summary>
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" /> 
    /// that this element is nested in
    /// </summary>
    IDragAndDropContainer Parent { get; set; }

    /// <summary>
    /// The other elements nested in this 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Parent"/>'s element
    /// </summary>
    public List<IDragAndDropElement> Siblings => Parent?.Children?.Where(s => s.Id != Id).ToList();

    /// <summary>
    /// Creates a deep copy of this element
    /// </summary>
    /// <returns>A deep copy of this element</returns>
    /// <remarks>
    /// When determining the logic for cloning, keep in mind that if a property or field is a reference type, 
    /// the reference is copied, so it might be best to invoke the "Clone" method for that field as well
    /// </remarks>
    IDraggableElement Clone();

    /// <summary>
    /// Creates a deep copy of this element
    /// </summary>
    /// <typeparam name="TDragAndDropElement">
    /// The implemented type if <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>
    /// </typeparam>
    /// <returns>A deep copy of this element.  This includes a copy of any reference elements</returns>
    public TDragAndDropElement Clone<TDragAndDropElement>() where TDragAndDropElement : IDragAndDropElement {
      // Create a new instance of TDragAndDropElement that will be the copy of this element
      var copiedElement = (TDragAndDropElement)Activator.CreateInstance(typeof(TDragAndDropElement));
      // Copy all properties that are not references

      // For each remaining property that is an implementation of IDragAndDropElement,
      //   set the value of the property to the Clone() method of the IDragAndDropElement
      // Note: need to check if the property is an enumerable/iterable of IDragAndDropElements and clone each
      //       element in the enumerable/iterable
      // For each remaining property
      // Try to use the Clone/Copy method for the referenced object
      // Then try to use MemberwiseClone
      // Otherwise, use the reference

      return copiedElement;
    }

    /// <summary>
    /// Groups this element with the specified <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>
    /// into a new <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <typeparam name="TDragAndDropContainer">
    /// The implemented type if <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </typeparam>
    /// <param name="element">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to group with
    /// </param>
    /// <param name="showFirst">Whether or not this item should appear first in the new group</param>
    /// <returns>The new <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/></returns>
    public TDragAndDropContainer GroupWith<TDragAndDropContainer>(IDragAndDropElement element, bool showFirst = false) where TDragAndDropContainer : IDragAndDropContainer {
      // Hold the parent reference of the element being grouped with (if the element has a parent)
      var containerParent = element.Parent;
      // Construct the Name for the new container
      var newContName = $"({(showFirst ? $"{Name}_{element.Name} Group" : $"{element.Name}_{Name} Group")})";
      // Hold the current index of this element
      var myIndex = Parent is null ? 0 : Parent.Children.IndexOf(this);
      // Hold the current index of the element being grouped with
      var targetIndex = (containerParent is null ? 0 : containerParent.Children.IndexOf(element));
      // Subtract 1 from the target index if this element's parent is the same as the element being grouped with's parent
      //   and this element's index is lower than the element being grouped with's index
      targetIndex -= (Parent is { } && Parent == containerParent && myIndex < targetIndex ? 1 : 0);

      // Determine if this element has a property named "AllowedTargetNames"
      Type myType = GetType();
      PropertyInfo myAllowedTargets = myType.GetProperty("AllowedTargetNames");
      // If this element has an AllowedTargetNames property, add the new group name to it if it doesn't already contain it
      if (myAllowedTargets is { }) {
        var curTargets = (List<string>)myAllowedTargets.GetValue(this);
        if (!curTargets.Contains(newContName)) { curTargets.Add(newContName); }
      }

      // Determine if the element being grouped with has a property named "AllowedTargetNames"
      Type elemType = element.GetType();
      PropertyInfo elemAllowedTargets = elemType.GetProperty("AllowedTargetNames");
      // If the target element has an AllowedTargetNames property, add the new group name to it if it doesn't already contain it
      if (elemAllowedTargets is { }) {
        var curTargets = (List<string>)elemAllowedTargets.GetValue(this);
        if (!curTargets.Contains(newContName)) { curTargets.Add(newContName); }
      }

      // Make a new container for the elements
      var newContainer = (TDragAndDropContainer)Activator.CreateInstance(typeof(TDragAndDropContainer));

      // Set the name value to name constructed above
      newContainer.Name = newContName;

      // Add the elements in order based on the "showFirst" flag
      newContainer.AddChild(element);
      newContainer.AddChild(this, showFirst ? 0 : 1);

      // If the element that this element is being grouped with had a parent,
      //   add the new container to that parent's children at the original element's index
      if (containerParent is { }) { containerParent.AddChild(newContainer, targetIndex); }

      // Return the newly created IDragAndDropContainer
      return newContainer;
    }
  }
}
