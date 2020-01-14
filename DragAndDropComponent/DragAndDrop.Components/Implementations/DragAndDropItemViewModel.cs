﻿using DragAndDrop.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> that
  /// wraps content.  This element is not draggable.
  /// </summary>
  public class DragAndDropItemViewModel : IDragAndDropElement {
    /// <summary>
    /// The default constructor
    /// </summary>
    public DragAndDropItemViewModel() {
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
    public virtual dynamic Item { get; set; }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Clone"/>
    public DragAndDropItemViewModel Clone() {
      return ((IDragAndDropElement)this).Clone<DragAndDropItemViewModel>();
    }
  }

  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> that
  /// wraps content.  This element is not draggable.
  /// </summary>
  /// <typeparam name="TItem">The type of data contained within the element</typeparam>
  public class DragAndDropItemViewModel<TItem> : IDragAndDropElement<TItem> {
    /// <summary>
    /// The default constructor
    /// </summary>
    public DragAndDropItemViewModel() {
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
    public TItem Item { get; set; }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Clone"/>
    public DragAndDropItemViewModel<TItem> Clone() {
      return ((IDragAndDropElement)this).Clone<DragAndDropItemViewModel<TItem>>();
    }

  }
}
