/* 

Sitemap Styler v0.1
written by Alen Grakalic, provided by Css Globe (cssglobe.com)
visit http://cssglobe.com/lab/sitemap_styler/
	
*/


$(document).ready(function () {

	var sitemap = document.getElementsByClassName("menu-my-menu");

	if (sitemap) {

		// rename tag nav to div
		$("nav:has(.menu-my-menu)").each(function () {
			var $this = $(this);
			$this.replaceWith($("<div>" + $this.html() + "</div>"));
		});

		var mysitemap = $(".menu-my-menu").attr('id', 'sitemap');

		// all li inside menu
		var myitemsCount = $(".menu-my-menu li").length;
		var myitems = $(".menu-my-menu li")
			.each(function (x, li) 
			{

				// если есть чилды, то 
				if (li.getElementsByTagName("ul").length > 0) 
				{
					var ul = li.getElementsByTagName("ul")[0]; // получаем вложенный список и скрываем его
					ul.style.display = "none";

					var span = document.createElement("span"); // добавляем место для плюсика вперед 
					span.className = "collapsed"; //отображаем плюсик


					ul.style.display = $(li).find(".current").length > 0 ? "block" : "none";
					span.className = ul.style.display == "none" ? "collapsed" : "expanded";

					span.onclick = function () // при клике по плюсику
					{
						ul.style.display = (ul.style.display == "none") ? "block" : "none"; //отображаем вложенный список или скрываем его
						this.className = (ul.style.display == "none") ? "collapsed" : "expanded"; // отображеем минусик
					};

					li.appendChild(span);
				};//end if
			});//end each

	};
});


