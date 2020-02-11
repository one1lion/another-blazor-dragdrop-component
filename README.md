# another-blazor-dragdrop-component
An approach to a reusable Blazor component library for a Drag And Drop user interface.

# Introduction
This project is for a Microsoft .NET Blazor Component Library that will provide an easy way to add a drag and drop feature to a Blazor project.  If you are interested in more information or about how the development process is going so far, take a look at the detailed [introduction on the Wiki](https://github.com/one1lion/another-blazor-dragdrop-component/wiki).  There is a link to the journal-style notes I have been making throughout the development process.  They are more for myself, but also for anyone interested in seeing it.

As a quick note: I named the component library project DragonDrop.  This is not to be confused with any existing products or companies that might already exist with this name.  It is just what my hands typed when naming the component library project.  As I get more confident in building components, I might consider packaging them together in a more robust component library and possibly brand it.  I have every intent to keep all of this open source; mostly so I can learn more from the community, but also to share my knowledge and experience as well.

# Current Status
The Drag And Drop component library is currently usable for a variety of scenarios.  There are further improvements that are being made to it via a project I am working on for my workplace.  I will update the GitHub project as I am able and am happy to receive any feedback, comments, or suggestions.  Feel free to email me or submit issues and/or pull requests.  This is my first official GitHub project, so I will do my best to respond in a correct fashion.

# Getting Started

Currently, the Drag And Drop component library is part of a .NET solution (DragAndDropComponent) that includes both the library project (DragonDrop) and a sample Blazor Server-Side project (DragAndDrop).  Clone or Download the project, then copy the DragonDrop folder to the project you would like to use it with.  Then, add the DragonDrop.csproj as an existing project.  Remember to add a reference to it from the project(s) that will be using the components.

Next, if this is a Blazor Server-side app, open the `{project_root}\Pages\_Host.cshtml` file.  If this is a Blazor WASM project, open the `{project_root}\wwwroot\index.html` file.  Add a reference to the DragonDrop stylesheet at the top.  

`<link rel="stylesheet" href="_content/DragonDrop/css/dragondrop.min.css" />`

In the future, I will provide SASS and CSS templates with the selectors that can be overwritten in a local css file.

# Sample Usage

The main focus of this component library is to provide a reusable component that allows for adding or loading items into a component that can be displayed as draggable elements that can be reordered and/or grouped together.  I hope to provide examples of each use case below or in other documentation, but for now, below is a sample of using the components for the specified purpose.

## Nested, Reorderable, Groupable Drag And Drop Component

The current, specific implementation of the Drag And Drop component library is shown below, along with the sample classes used as the items and groups.

On the `.razor` the component is to be used on:
```razor
@using DragonDrop.DragAndDrop
@* Note that this could also be added to the _Imports.razor file to be available to all `.razor` components *@

@* ... Other page code ... *@

@if (itemToWrapGroup.Children is null || itemToWrapGroup.Children.Count() == 0) {
  <p>Loading sample...</p>
} else {
  <DnDContainer TListItem="ItemToWrapBase"
                TItem="ItemToWrap"
                TGroup="ItemToWrapGroup"
                List="itemToWrapGroup.Children"
                Title="Nested Groups Sample"
                ShowTitle="false"
                AllowDropToGroup="true"
                ShowGlobalAddNew="true"
                SeparatorDisplay="SeparatorDisplay.Between"
                ChildrenPropertyName="@(nameof(ItemToWrapGroup.Children))"
                OnStatusUpdated="HandleStatusUpdated"
                ShowDebugInfo="false"
                Name="Top Level Container">
    <ItemTemplate Context="curItem">
      @curItem.ItemContent
    </ItemTemplate>
    <EditItemTemplate Context="curItem">
      <EditForm Model="curItem">
        <InputText @bind-Value="curItem.ItemContent" />
      </EditForm>
    </EditItemTemplate>
    <SeparatorItemTemplate Context="curItem">
      <hr />
    </SeparatorItemTemplate>
  </DnDContainer>
}

@* ... Other page code ... *@

@code {
  ItemToWrapGroup itemToWrapGroup;

  protected override void OnInitialized() {
    itemToWrapGroup = new ItemToWrapGroup() { Children = new List<ItemToWrapBase>() };
    // TODO: Pre-populate the Children if you would like to see items added
    //       An example of this can be seen in the accompanying sample project
  }
}
```

Both the Item class and the class to be used as a group should inherit the same base class.  This is similar to the way that all nested items in HTML are Elements.  Some of the elements can have other elements nested in them, while others cannot.  That is an oversimplification, but the point is that the DnDContainer component uses a `List<SomeBaseClass>` to determine how to display the items.  The way it knows whether to display an element in that list as an item or another container is by checking if the type of the element matches either TItem or TGroup, respectively.  A sample base class for the items could look like:

```csharp
  public class ItemToWrapBase {
    public ItemToWrapBase() {
      Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public ItemToWrapGroup Parent { get; set; }

  }
```

A class that represents a specific item to display as a draggable element (DnDItem) could look like: 

```csharp
  public class ItemToWrap : ItemToWrapBase {
    public string ItemContent { get; set; }
  }
```

A class that represents an object that is to be used to populate another DnDContainer could look like:

```csharp
  public class ItemToWrapGroup : ItemToWrapBase {
    public List<ItemToWrapBase> Children { get; set; } = new List<ItemToWrapBase>();
  }
```

# How to Contribute and Other Notes
