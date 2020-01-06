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
    /// Groups this element with the specified <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>
    /// into a new <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <typeparam name="TDragAndDropContainer"></typeparam>
    /// <param name="element">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to group with
    /// </param>
    /// <param name="showFirst">Whether or not this item should appear first in the new group</param>
    /// <returns>The new <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/></returns>
    public TDragAndDropContainer GroupWith<TDragAndDropContainer>(IDragAndDropElement element, bool showFirst = false) where TDragAndDropContainer : IDragAndDropContainer {
      // Hold the parent reference of the element being grouped with (if the element has a parent)
      var containerParent = element.Parent;
      // Construct the Name for the new container
      var newContName = showFirst ? $"{Name}_{element.Name} Group" : $"{element.Name}_{Name} Group";
      var myIndex = Parent.Children.IndexOf(this);
      Type myType = this.GetType();
      PropertyInfo myAllowedTargets = myType.GetProperty("AllowedTargetNames");
      Type elemType = element.GetType();
      PropertyInfo elemAllowedTargets = elemType.GetProperty("AllowedTargetNames");
      // If this element has an AllowedTargetNames property, add the new group name to it if it doesn't already contain it
      if (myAllowedTargets is { }) {
        var curTargets = (List<string>)myAllowedTargets.GetValue(this);
        if (!curTargets.Contains(newContName)) { curTargets.Add(newContName); }
      }
      // If the target element has an AllowedTargetNames property, add the new group name to it if it doesn't already contain it
      if (elemAllowedTargets is { }) {
        var curTargets = (List<string>)elemAllowedTargets.GetValue(this);
        if (!curTargets.Contains(newContName)) { curTargets.Add(newContName); }
      }

      // Make a new container for the elements
      Type newContType = typeof(TDragAndDropContainer);
      PropertyInfo nameProp = newContType.GetProperty("Name");
      var newContainer = (TDragAndDropContainer)Activator.CreateInstance(typeof(TDragAndDropContainer));

      newContainer.Name = newContName;

      Parent?.RemoveChild(this);
      newContainer.AddChild(element);
      newContainer.AddChild(this, showFirst ? 0 : 1);
      if (containerParent is { }) { containerParent.AddChild(newContainer, myIndex); }

      return newContainer;
    }
  }
}
