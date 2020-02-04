using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragAndDrop.Data {
  public class ItemToWrapBase {
    public ItemToWrapBase() {
      Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public ItemToWrapGroup Parent { get; set; }

  }
}
