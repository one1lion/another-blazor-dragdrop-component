/* 
 * This uses MutationObserver to detect DOM changes (since page navigation in Razor 
 * does not fire load/unload events) to add the ondragstart event. Sample adapted from
 * https://stackoverflow.com/questions/3219758/detect-changes-in-the-dom
 */
var observeDOM = (function () {
  var MutationObserver = window.MutationObserver || window.WebKitMutationObserver;

  return function (obj, callback) {
    if (!obj || !obj.nodeType === 1) return;

    if (MutationObserver) {
      var obs = new MutationObserver(function (mutations, observer) {
        callback(mutations);
      });
      obs.observe(obj, { childList: true, subtree: true });
    } else if (window.addEventListener) {
      obj.addEventListener('DOMNodeInserted', callback, false);
      obj.addEventListener('DOMNodeRemoved', callback, false);
    }
  };
})();

observeDOM(document, function (m) {
  var dnds = document.querySelectorAll("*[draggable=true]");
  for (var i = 0; i < dnds.length; i++) {
    dnds[i].ondragstart = function (e) {
      e.dataTransfer.setData("text/plain", "Dragging");
    };
    dnds[i].ondrop = function (e) {
      e.preventDefault();
      return false;
    };
  }
});