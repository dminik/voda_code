using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

// -----------------------------------------------------------------------
// <copyright file="$safeitemrootname$.cs" company="$registeredorganization$">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Themes
{
	public abstract class ProductBase : ItemBase
	{
		/// <summary>
		/// For Item view
		/// </summary>
		public string FullName
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		/// <summary>
		/// For List view
		/// </summary>
		public string ShortName
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public double Price
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		/// <summary>
		/// Only model name
		/// </summary>
		public string ModelName
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public CatalogType CatalogType
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public ProductType ProductType
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public Manufacturer Manufacturer
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}
	}
}
