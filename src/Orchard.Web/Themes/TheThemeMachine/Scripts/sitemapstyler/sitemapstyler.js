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

				// ���� ���� �����, �� 
				if (li.getElementsByTagName("ul").length > 0) 
				{
					var ul = li.getElementsByTagName("ul")[0]; // �������� ��������� ������ � �������� ���
					ul.style.display = "none";

					var span = document.createElement("span"); // ��������� ����� ��� ������� ������ 
					span.className = "collapsed"; //���������� ������


					ul.style.display = $(li).find(".current").length > 0 ? "block" : "none";
					span.className = ul.style.display == "none" ? "collapsed" : "expanded";

					span.onclick = function () // ��� ����� �� �������
					{
						ul.style.display = (ul.style.display == "none") ? "block" : "none"; //���������� ��������� ������ ��� �������� ���
						this.className = (ul.style.display == "none") ? "collapsed" : "expanded"; // ���������� �������
					};

					li.appendChild(span);
				};//end if
			});//end each

	};
});


