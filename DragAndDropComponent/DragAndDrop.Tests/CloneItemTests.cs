using DragAndDrop.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Tests {
  public class CloneItemTests {
    [SetUp]
    public void Setup() {

    }

    [Test]
    public void CloningADragAndDropItemWithPrimitive_ReturnsNewDragAndDropItemWithPrimitive() {
      var element = new DragAndDropItem<int>() {
        Name = "Item",
        Item = 1
      };

      var clonedElement = element.Clone();

      Assert.AreEqual(clonedElement.Item, 1);
      Assert.AreEqual(clonedElement.Name, "Item");
      Assert.AreNotEqual(clonedElement.Id, element.Id);
    }
  }
}
