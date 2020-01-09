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
    public IList<IDragAndDropElement> Siblings => Parent?.Children?.Where(s => s.Id != Id).ToList();

    /// <summary>
    /// Creates a deep copy of this element
    /// </summary>
    /// <typeparam name="TDragAndDropElement">
    /// The implemented type if <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>
    /// </typeparam>
    /// <returns>A deep copy of this element.  This includes a copy of any reference elements</returns>
    public TDragAndDropElement Clone<TDragAndDropElement>() where TDragAndDropElement : IDragAndDropElement {
      // Create a new instance of TDragAndDropElement that will be the copy of this element
      var cloneOfElement = (TDragAndDropElement)Activator.CreateInstance(typeof(TDragAndDropElement));

      // Get all of the properties for this element
      var props = cloneOfElement.GetType().GetProperties();

      foreach (var prop in props) {
        if (!prop.CanWrite) { continue; }

        switch (prop) {
          // If the current property is anything that implements IDrangAndDropElement, then
          //   use the Clone method on this object's corresponding property's value to copy 
          //   the returned element as the new object's property's value
          case var p when p.PropertyType.GetTypeInfo().IsAssignableFrom(typeof(IDragAndDropElement).GetTypeInfo()): {
              // With the exception when the current property is named "Parent", do not clone 
              //   or copy it.  I.e. leave it null
              if (p.Name == "Parent") { continue; }
              p.SetValue(cloneOfElement, ((IDragAndDropElement)p.GetValue(this)).Clone<IDragAndDropElement>());
              break;
            }

          // If the current property type implements IList<IDragAndDropElement> (both IEnumerable<...> and ICollection<...>)
          case var p when p.PropertyType.GetTypeInfo().IsAssignableFrom(typeof(IList<IDragAndDropElement>).GetTypeInfo()): {
              var newList = (IList<IDragAndDropElement>)Activator.CreateInstance(p.PropertyType);
              var curList = (IList<IDragAndDropElement>)p.GetValue(this);
              for (var i = 0; i < curList.Count(); i++) {
                newList.Add(curList[i].Clone<IDragAndDropElement>());
              }
              p.SetValue(cloneOfElement, newList);
              break;
            }


          // If the current property type only implements IEnumerable<IDragAndDropElement>
          case var p when p.PropertyType.GetTypeInfo().IsAssignableFrom(typeof(ICollection<IDragAndDropElement>).GetTypeInfo()): {
              // TODO: Fill in logic
              break;
            }

          // If the current property type only implements ICollection<IDragAndDropElement>
          case var p when p.PropertyType.GetTypeInfo().IsAssignableFrom(typeof(ICollection<IDragAndDropElement>).GetTypeInfo()): {
              // TODO: Fill in logic
              break;
            }

          // If the current property type is a value type, set the new object's corresponding
          //   property's to the value of this object's corresponding value
          case var p when !p.PropertyType.IsPointer: {
              p.SetValue(cloneOfElement, p.GetValue(this));
              break;
            }

          default: {
              // TODO: Fill in logic for all other property types
              //   These could be arbitrary classes, other IList, IEnumerable, or ICollections of non IDragAndDropElements

              // For each remaining property type, we could
              // Try to use the Clone/Copy method for the referenced object
              // Then try to use MemberwiseClone
              break;
            }
        }

      }

      return cloneOfElement;
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

      // Make a new container for the elements
      var newContainer = (TDragAndDropContainer)Activator.CreateInstance(typeof(TDragAndDropContainer));

      // Set the name value to name constructed above
      newContainer.Name = newContName;

      // Determine if the newContainer has a property named "AllowedTargetNames"
      PropertyInfo newGroupAllowedTargetsProp = typeof(TDragAndDropContainer).GetProperty("AllowedTargetNames");

      // Prepare to accumulate allowed drop target names
      var newContAllowedTargetNames = new List<string>();

      // Determine if this element has a property named "AllowedTargetNames"
      Type myType = GetType();
      PropertyInfo myAllowedTargetsProp = myType.GetProperty("AllowedTargetNames");
      // If this element has an AllowedTargetNames property, add the new group name to it if it doesn't already contain it
      if (myAllowedTargetsProp is { }) {
        var curTargets = (List<string>)myAllowedTargetsProp.GetValue(this);
        // Accumulate the current allowed target names
        newContAllowedTargetNames.AddRange(curTargets);
        if (!curTargets.Contains(newContName)) { curTargets.Add(newContName); }
      }

      // Determine if the element being grouped with has a property named "AllowedTargetNames"
      Type elemType = element.GetType();
      PropertyInfo elemAllowedTargetsProp = elemType.GetProperty("AllowedTargetNames");
      // If the target element has an AllowedTargetNames property, add the new group name to it if it doesn't already contain it
      if (elemAllowedTargetsProp is { }) {
        var curTargets = (List<string>)elemAllowedTargetsProp.GetValue(this);
        // Accumulate the current allowed target names
        newContAllowedTargetNames.AddRange(curTargets);
        if (!curTargets.Contains(newContName)) { curTargets.Add(newContName); }
      }

      // If the new group has the AllowedTargetNames property (in other words, it is Draggable), initilize the property
      // to a new List<string>
      if (newGroupAllowedTargetsProp is { }) {
        newGroupAllowedTargetsProp.SetValue(newContainer, newContAllowedTargetNames.Distinct().ToList());
      }

      // Add the elements in order based on the "showFirst" flag
      newContainer.AddChild(element);
      newContainer.AddChild(this, showFirst ? 0 : 1);

      // If the element that this element is being grouped with had a parent,
      //   add the new container to that parent's children at the original element's index
      if (containerParent is { }) {
        containerParent.AddChild(newContainer, targetIndex);
        // Also, add the container parent's name as an allowed target name if the new group has the AllowedTargetNames property
        if (newGroupAllowedTargetsProp is { }) {
          var curTargets = (List<string>)newGroupAllowedTargetsProp.GetValue(this);
          if (!curTargets.Contains(containerParent.Name)) { curTargets.Add(containerParent.Name); }
        }
      }

      // Return the newly created IDragAndDropContainer
      return newContainer;
    }

    /// <summary>
    /// A convenience method for returning either a 
    /// <see cref="DragAndDrop.Components.DragAndDropContainer" /> or a
    /// <see cref="DragAndDrop.Components.DraggableGroup" />
    /// </summary>
    /// <param name="element">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
    /// to group this element with
    /// </param>
    /// <param name="makeDraggable">
    /// If true, this will return a <see cref="DragAndDrop.Components.DraggableGroup" />, otherwise it will return
    /// a <see cref="DragAndDrop.Components.DragAndDropContainer" />
    /// </param>
    /// <param name="showFirst">Whether or not this item should appear first in the new group</param>
    /// <returns>
    /// If <paramref name="makeDraggable"/> is true, a <see cref="DragAndDrop.Components.DraggableGroup" />, 
    /// otherwise a <see cref="DragAndDrop.Components.DragAndDropContainer" />
    /// </returns>
    public IDragAndDropContainer GroupWith(IDragAndDropElement element, bool makeDraggable, bool showFirst = false) {
      return makeDraggable ?
        GroupWith<DraggableGroup>(element, showFirst) :
        GroupWith<DragAndDropContainer>(element, showFirst);
    }
  }
}
