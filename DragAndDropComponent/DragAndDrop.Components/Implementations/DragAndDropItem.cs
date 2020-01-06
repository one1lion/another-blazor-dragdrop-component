using DragAndDrop.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> that
  /// wraps content.  This element is not draggable.
  /// </summary>
  /// <typeparam name="T">The type of data contained within the element</typeparam>
  public class DragAndDropItem<T> : IDragAndDropElement {
    /// <summary>
    /// The default constructor
    /// </summary>
    public DragAndDropItem() {
      Id = Guid.NewGuid().ToString();
    }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Id"/>
    public string Id { get; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Name"/>
    public string Name { get; set; }
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Parent"/>
    public IDragAndDropContainer Parent { get; set; }

    /// <summary>
    /// The item being wrapped by this draggable element
    /// </summary>
    public T Item { get; set; }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Clone"/>
    public IDraggableElement Clone() {
      var newElem = new DragAndDropItem<T>() {
        Name = $"{Name}(1)",
        Item = Item, // TODO: Clone the wrapped item
        Parent = Parent
      };
      Parent.Children.Add(newElem);
      return (IDraggableElement)newElem;
    }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.GroupWith(IDragAndDropElement, bool)"/>
    public IDragAndDropContainer GroupWith(IDragAndDropElement element, bool makeDraggableGroup, bool showFirst = false) {
      // Hold the parent reference of the element being grouped with
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
      IDragAndDropContainer newContainer = makeDraggableGroup ?
          (IDragAndDropContainer)new DraggableGroup() :
          (IDragAndDropContainer)new DragAndDropContainer();

      newContainer.Name = newContName;

      Parent.RemoveChild(this);
      newContainer.AddChild(element);
      newContainer.AddChild(this, showFirst ? 0 : 1);
      containerParent.AddChild(newContainer, myIndex);

      return newContainer;
    }
  }
}
