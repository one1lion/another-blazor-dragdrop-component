﻿using System;
using System.Collections.Generic;
using System.Reflection;

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
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Name" /> to test
    /// whether this element can be dropped into/onto
    /// </param>
    /// <returns>Whether or not this element can be dropped into/onto the specified target name</returns>
    public bool CanDrop(IDragAndDropContainer container) => container is { } && AllowedTargetNames is { } && AllowedTargetNames.Contains(container.Name);

    /// <summary>
    /// Add this element to the target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="targetContainer">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> to
    /// add this element to
    /// </param>
    /// <param name="targetIndex">
    /// The index (position) within the list of the <paramref name="targetContainer"/> 
    /// this item should be added at.  If this is the default value for nullable int,
    /// it will be added to the end
    /// </param>
    /// <returns>Whether or not the add was successful</returns>
    public bool AddTo(IDragAndDropContainer targetContainer, int? targetIndex = default) {
      if (!CanDrop(targetContainer)) { return false; }
      targetContainer.AddChild(this, targetIndex);
      return true;
    }

    /// <summary>
    /// Copy this element to the target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="targetContainer">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> to
    /// copy this element to
    /// </param>
    /// <param name="targetIndex">
    /// The index (position) within the list of the <paramref name="targetContainer"/> 
    /// the copy of this item should be added at.  If this is the default value for nullable 
    /// int, it will be added to the end
    /// </param>
    /// <returns>Whether or not the add was successful</returns>
    public bool CopyTo(IDragAndDropContainer targetContainer, int? targetIndex = default) {
      var copiedElement = (IDraggableElement)Clone<IDragAndDropElement>();
      copiedElement.Parent = targetContainer;
      if (!copiedElement.AllowedTargetNames.Contains(targetContainer.Name)) { copiedElement.AllowedTargetNames.Add(targetContainer.Name); }
      targetContainer.AddChild(copiedElement, targetIndex);
      return true;
    }

    /// <summary>
    /// Remove this element from the target
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="targetContainer">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> to
    /// remove this element from
    /// </param>
    /// <returns>Whether or not the remove was successful</returns>
    public IDraggableElement RemoveFrom(IDragAndDropContainer targetContainer) {
      return (IDraggableElement)targetContainer.RemoveChild(this);
    }

    /// <summary>
    /// Move this element to or within the target 
    /// <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="targetContainer">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/> to
    /// move this element to or within
    /// </param>
    /// <param name="targetIndex">
    /// The index (position) within the list of the <paramref name="targetContainer"/> 
    /// this item should be added at.  If this is the default value for nullable int,
    /// it will be added to the end
    /// </param>
    public bool Move(IDragAndDropContainer targetContainer, int? targetIndex = default) {
      var movingWithinGroup = Parent.Name == targetContainer.Name;
      var originalIndex = Parent.Children.IndexOf(this);

      if (movingWithinGroup && originalIndex == targetIndex) { return true; }
      if (!CanDrop(targetContainer)) { return false; }

      if (movingWithinGroup) {
        targetContainer.MoveChild(this, targetIndex);
      } else {
        Parent.RemoveChild(this);
        targetContainer.AddChild(this, targetIndex);
      }
      // If here, no exceptions have occurred
      return true;
    }
  }

  public interface IDraggableElement<TItem> : IDraggableElement {
    TItem Item { get; set; }
  }
}
