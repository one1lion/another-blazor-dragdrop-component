﻿using System;
using System.Collections.Generic;
using DragAndDrop.Components.Interfaces;

namespace DragAndDrop.Components {
  /// <summary>
  /// An implementation of <see cref="DragAndDrop.Components.Interfaces.IDraggableElement"/>
  /// and <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
  /// that can be dragged onto/into other <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" />s,
  /// moving the entire group (with its children) into the 
  /// target <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer" />
  /// </summary>
  public class DraggableContainerViewModel : DragAndDropContainerViewModel, IDraggableElement {
    /// <summary>The default constructor</summary>
    public DraggableContainerViewModel() : base() { }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.DragEnabled" />
    public bool DragEnabled { get; set; } = true;
    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDraggableElement.AllowedTargetNames" />
    public List<string> AllowedTargetNames { get; set; }

    /// <inheritdoc cref="DragAndDrop.Components.Interfaces.IDragAndDropElement.Clone"/>
    public new DraggableContainerViewModel Clone() {
      return ((IDragAndDropElement)this).Clone<DraggableContainerViewModel>();
    }

  }
}