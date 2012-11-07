
$(document).ready(function () {
    var updateMenuPositions = function ($ol, prefix) {
        $("> li", $ol).each(function (i, e) {
            var npref = (prefix?prefix + ".":"") + (i+1);
            $(".menu-tree-pos", e).val(npref);
            updateMenuPositions($("> ol", e), npref);
        });
    };

    $("ol.menu-tree").nestedSortable({
        disableNesting: 'no-nest',
        forcePlaceholderSize: true,
        handle: 'div',
        helper: 'clone',
        items: 'li',
        opacity: .6,
        placeholder: 'menu-tree-placeholder',
        revert: 250,
        tabSize: 25,
        tolerance: 'pointer',
        toleranceElement: '> div',
        stop: function (event, ui) {
            updateMenuPositions($("ol.menu-tree"), "");
        }
    });
});
