using DragAndDrop.Components.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Components {
  public class DragAndDropElementBase : ComponentBase, IDragAndDropElement {
    public string Id { get; }
    public DragAndDropElementBase() {
      Id = Guid.NewGuid().ToString();
    }

    [Parameter] 
    public string Name { get; set; }
    public IDragAndDropContainer Parent { get; set; }
  }
}
