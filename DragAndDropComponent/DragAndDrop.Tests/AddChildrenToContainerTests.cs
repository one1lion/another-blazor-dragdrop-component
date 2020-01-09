using DragAndDrop.Components;
using DragAndDrop.Components.Interfaces;
using NUnit.Framework;

namespace DragAndDrop.Tests {
  public class AddChildrenToContainerTests {
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void AddingANewElementToAContainer_SetsTheElementsParent() {
      IDragAndDropContainer container = new DragAndDropContainer();
      var element = new DragAndDropItem<string>();

      container.AddChild(element);
      
      Assert.IsNotNull(element.Parent);
      Assert.IsTrue(element.Parent == container);
    }

    [Test]
    public void AddingANewElementToAContainer_AddsElementToChildOfContainer() {
      IDragAndDropContainer container = new DragAndDropContainer();
      var element = new DragAndDropItem<string>();

      container.AddChild(element);

      Assert.IsNotNull(container.Children);
      Assert.IsTrue(container.Children.Contains(element));
    }

    [Test]
    public void AddingAnExistingElementToDifferentContainer_ChangesElementsParent() {
      Assert.IsTrue(false);
    }

    [Test]
    public void AddingAnExistingElementToDifferentContainer_RemovesFromChildrenOfOriginalParentContainer() {
      Assert.IsTrue(false);
    }

    [Test]
    public void AddingAnExistingElementToDifferentContainer_AddsToChildrenOfNewParentContainer() {
      Assert.IsTrue(false);
    }
  }
}