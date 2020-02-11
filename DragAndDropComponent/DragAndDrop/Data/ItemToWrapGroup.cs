using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragAndDrop.Data {
  public class ItemToWrapGroup : ItemToWrapBase {
    public List<ItemToWrapBase> Children { get; set; } = new List<ItemToWrapBase>();
  }
}
