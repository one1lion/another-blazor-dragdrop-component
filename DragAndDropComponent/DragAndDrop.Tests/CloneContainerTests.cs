using DragAndDrop.Components;
using DragAndDrop.Components.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragAndDrop.Tests {
  public class CloneContainerTests {
    [SetUp]
    public void Setup() {

    }

    // TODO: split this into unit tests
    [Test]
    public void TestAllTheThings() {
      var element = new DragAndDropContainerViewModel() {
        Name = "Group 1",
        Children = new List<IDragAndDropElement>() {
          new DraggableItemViewModel<string>() {
            Name = "Item 1",
            Item = "Draggable Item 1",
            AllowedTargetNames = new List<string>() { "Group inside an item" }
          },
          new DragAndDropItemViewModel<string>() {
            Name = "Item 2",
            Item = "Non-draggable Item 2"
          },
          new DragAndDropContainerViewModel() {
            Name = "Item 3"
          },
          new DraggableContainerViewModel() {
            Name = "Item 4",
            AllowedTargetNames = new List<string>() { "Group inside an item" }
          }
        }
      };

      var clonedElement = element.Clone();
      clonedElement.Name = "Clone of Group 1";
      foreach (var child in clonedElement.Children) {
        child.Name = $"Clone of {child.Name}";
      }

      Assert.AreNotSame(element, clonedElement);
      Assert.AreNotEqual(element.Name, clonedElement.Name);
      Assert.IsInstanceOf(clonedElement.GetType(), typeof(DragAndDropContainerViewModel));
      Assert.AreNotSame(element.Children, clonedElement.Children);
      Assert.AreEqual(element.Children.Count, clonedElement.Children.Count);
      Assert.AreEqual(clonedElement.Children.Count, 4);
      Assert.AreNotSame(element.Children[0], clonedElement.Children[0]);
      Assert.AreNotEqual(element.Children[0].Name, clonedElement.Children[0].Name);
      Assert.IsInstanceOf(clonedElement.Children[0].GetType(), typeof(DraggableItemViewModel<string>));
      Assert.AreNotSame(element.Children[1], clonedElement.Children[1]);
      Assert.AreNotEqual(element.Children[1].Name, clonedElement.Children[1].Name);
      Assert.IsInstanceOf(clonedElement.Children[1].GetType(), typeof(DragAndDropItemViewModel<string>));
      Assert.AreNotSame(element.Children[2], clonedElement.Children[2]);
      Assert.AreNotEqual(element.Children[2].Name, clonedElement.Children[2].Name);
      Assert.IsInstanceOf(clonedElement.Children[2].GetType(), typeof(DragAndDropContainerViewModel));
      Assert.AreNotSame(element.Children[3], clonedElement.Children[3]);
      Assert.AreNotEqual(element.Children[3].Name, clonedElement.Children[3].Name);
      Assert.IsInstanceOf(clonedElement.Children[3].GetType(), typeof(DraggableContainerViewModel));

    }
  }
}
