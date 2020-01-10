using System.Collections.Generic;

namespace DragAndDrop.Data {
  public class SampleDataService {
    public static List<DraggableItemSample<string>> GenerateSampleDraggableItems() {
      var draggableItems = new List<DraggableItemSample<string>>();
      for (var i = 0; i < 3; i++) {
        for (var j = 0; j < 7; j++) {
          draggableItems.Add(new DraggableItemSample<string>() {
            GroupName = $"Group {i + 1}",
            Order = j,
            Item = $"Group {i + 1}, Item {j + 1}"
          });
        }
      }
      return draggableItems;
    }

    public static List<Foo> GenerateSampleFooItems() {
      var items = new List<Foo>();
      for (var i = 0; i < 3; i++) {
        for (var j = 0; j < 7; j++) {
          items.Add(new Foo() {
            GroupName = $"Group {i + 1}",
            Order = j,
            ForDisplay = $"Group {i + 1}, Item {j + 1}"
          });
        }
      }
      return items;
    }
  }
}
