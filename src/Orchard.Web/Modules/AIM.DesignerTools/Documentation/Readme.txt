AIM DesignerTools
=================

License
=======

The AIM.DesignerTools Orchard module is based on code from Orchard itself, 
and is therefor licensed under the same license (New BSD License) and terms.
You can review the terms online at http://orchard.codeplex.com/license or read 
the attached License.txt file.

Changelog
=========

v1.4.0.0 - 01/03/2012
---------------------

* Added: Action Alternates


Manual
======

Action Alternates
-----------------

Let's say that you have a module named My.Module that has a controller named
'Hello' and an action 'World', and you wish to use a layout that differs from
the default one.

When you enable the 'AIM Action Alternates' feature, you simply have to create
a new file named 'Layout-action-My.Module-Hello-World.cshtml' in your theme
and make the required changes.

So whenever your page is rendered, it'll use this layout instead of the
default one.

In this case, it'll try to use one of the following alternates:

- Layout-action-My.Module-Hello-World.cshtml
- Layout-action-My.Module-Hello.cshtml
- Layout-action-My.Module.cshtml
