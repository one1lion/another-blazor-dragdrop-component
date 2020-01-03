using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Components.Interfaces {
  /// <summary>
  /// A specification for a making an IDragAndDropElement groupable 
  /// </summary>
  public interface IDragAndDropGroupable {
    /// <summary>
    /// Groups this element with the specified <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/>
    /// into a new <see cref="DragAndDrop.Components.Interfaces.IDragAndDropContainer"/>
    /// </summary>
    /// <param name="element">
    /// The <see cref="DragAndDrop.Components.Interfaces.IDragAndDropElement"/> to group with
    /// </param>
    /// <param name="showFirst">Whether or not this item should appear first in the new group</param>
    /// <returns></returns>
    IDragAndDropContainer GroupWith(IDragAndDropElement element, bool showFirst = false);
  }
}
