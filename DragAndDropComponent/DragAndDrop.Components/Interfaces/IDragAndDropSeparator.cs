using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// A specification for a making an IDragAndDropElement into a separator element, 
  /// such as drop target (Drop Before, Drop After), or simply an element to 
  /// place between Children in a container.
  /// </summary>
  public interface IDragAndDropSeparator : IDragAndDropElement {

  }
}
