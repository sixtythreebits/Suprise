﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.DB
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Suprise")]
	public partial class DBCoreDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DBCoreDataContext() : 
				base(global::Core.Properties.Settings.Default.SupriseConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DBCoreDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBCoreDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBCoreDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBCoreDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.tsp_Orders")]
		public int tsp_Orders([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="TinyInt")] System.Nullable<byte> iud, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="OrderID", DbType="Int")] ref System.Nullable<int> orderID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsLovePack", DbType="Bit")] System.Nullable<bool> isLovePack, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Price", DbType="Money")] System.Nullable<decimal> price, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipient", DbType="NVarChar(100)")] string recipient, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Address", DbType="NVarChar(500)")] string address, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ZipCode", DbType="VarChar(20)")] string zipCode, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Note", DbType="NVarChar(1000)")] string note, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsPaid", DbType="Bit")] System.Nullable<bool> isPaid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsDelivered", DbType="Bit")] System.Nullable<bool> isDelivered)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), iud, orderID, isLovePack, price, recipient, address, zipCode, note, isPaid, isDelivered);
			orderID = ((System.Nullable<int>)(result.GetParameterValue(1)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.List_Orders", IsComposable=true)]
		public IQueryable<List_OrdersResult> List_Orders()
		{
			return this.CreateMethodCallQuery<List_OrdersResult>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
		}
	}
	
	public partial class List_OrdersResult
	{
		
		private int _OrderID;
		
		private bool _IsLovePack;
		
		private decimal _Price;
		
		private string _Recipient;
		
		private string _Address;
		
		private string _ZipCode;
		
		private string _Note;
		
		private bool _IsPaid;
		
		private bool _IsDelivered;
		
		private System.DateTime _CRTime;
		
		public List_OrdersResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrderID", DbType="Int NOT NULL")]
		public int OrderID
		{
			get
			{
				return this._OrderID;
			}
			set
			{
				if ((this._OrderID != value))
				{
					this._OrderID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsLovePack", DbType="Bit NOT NULL")]
		public bool IsLovePack
		{
			get
			{
				return this._IsLovePack;
			}
			set
			{
				if ((this._IsLovePack != value))
				{
					this._IsLovePack = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Price", DbType="Money NOT NULL")]
		public decimal Price
		{
			get
			{
				return this._Price;
			}
			set
			{
				if ((this._Price != value))
				{
					this._Price = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Recipient", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Recipient
		{
			get
			{
				return this._Recipient;
			}
			set
			{
				if ((this._Recipient != value))
				{
					this._Recipient = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Address", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				if ((this._Address != value))
				{
					this._Address = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ZipCode", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string ZipCode
		{
			get
			{
				return this._ZipCode;
			}
			set
			{
				if ((this._ZipCode != value))
				{
					this._ZipCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Note", DbType="NVarChar(1000) NOT NULL", CanBeNull=false)]
		public string Note
		{
			get
			{
				return this._Note;
			}
			set
			{
				if ((this._Note != value))
				{
					this._Note = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsPaid", DbType="Bit NOT NULL")]
		public bool IsPaid
		{
			get
			{
				return this._IsPaid;
			}
			set
			{
				if ((this._IsPaid != value))
				{
					this._IsPaid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDelivered", DbType="Bit NOT NULL")]
		public bool IsDelivered
		{
			get
			{
				return this._IsDelivered;
			}
			set
			{
				if ((this._IsDelivered != value))
				{
					this._IsDelivered = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CRTime", DbType="DateTime NOT NULL")]
		public System.DateTime CRTime
		{
			get
			{
				return this._CRTime;
			}
			set
			{
				if ((this._CRTime != value))
				{
					this._CRTime = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
